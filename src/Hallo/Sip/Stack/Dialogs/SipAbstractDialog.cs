using System;
using System.Collections.Generic;
using System.Net;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using Hallo.Sip.Util;
using Hallo.Util;
using NLog;

namespace Hallo.Sip.Stack.Dialogs
{
    public abstract class SipAbstractDialog : ISipListener
    {
        protected Logger _logger;

        protected readonly ISipListener _listener;
        protected readonly IPEndPoint _listeningPoint;
        protected List<SipResponse> _responses;
        protected volatile DialogState _state;
        protected readonly SipHeaderFactory _headerFactory;
        protected readonly SipMessageFactory _messageFactory;
        protected readonly SipAddressFactory _addressFactory;
        protected readonly ISipMessageSender _messageSender;
        protected SipViaHeader _topMostVia;

        #region properties
        
        protected string _callId;
        public string CallId
        {
            get { return _callId; }

        }

        protected string _localTag;

        /// <summary>
        /// The tag that was created locally
        /// </summary>
        public string LocalTag
        {
            get { return _localTag; }

        }

        protected string _remoteTag;

        /// <summary>
        /// The tag that was received from the remote side
        /// </summary>
        public string RemoteTag
        {
            get { return _remoteTag; }
        }

        protected int _localSequenceNr;
        public int LocalSequenceNr
        {
            get { return _localSequenceNr; }
        }

        protected int _remoteSequenceNr;
        public int RemoteSequenceNr
        {
            get { return _remoteSequenceNr; }
        }

        protected SipUri _localUri;

        /// <summary>
        /// The uri that contains 'who' is the local side
        /// </summary>
        public SipUri LocalUri
        {
            get { return _localUri; }
        }

        protected SipUri _remoteUri;

        /// <summary>
        /// The uri that contains 'who' is the remote side
        /// </summary>
        public SipUri RemoteUri
        {
            get { return _remoteUri; }
        }

        protected SipUri _remoteTarget;

        /// <summary>
        /// The uri that contains 'where' is the remote side
        /// </summary>
        public SipUri RemoteTarget
        {
            get { return _remoteTarget; }
            set { _remoteTarget = value; }
        }

        public const bool IsSecure = false;
        protected List<SipRecordRouteHeader> _routeSet;
        public List<SipRecordRouteHeader> RouteSet
        {
            get { return _routeSet; }
        }

        protected DateTime _createDate;
       
        public DateTime CreateDate
        {
            get { return _createDate; }
       }

        public DialogState State
        {
            get { return _state; }
        }
        
        #endregion

        protected SipAbstractDialog(SipHeaderFactory headerFactory,
             SipMessageFactory messageFactory,
             SipAddressFactory addressFactory,
             ISipMessageSender messageSender,
             ISipListener listener,
             IPEndPoint listeningPoint)
             
        {
            Check.Require(headerFactory, "headerFactory");
            Check.Require(messageFactory, "messageFactory");
            Check.Require(addressFactory, "addressFactory");
            Check.Require(messageSender, "messageSender");
            Check.Require(listener, "listener");
            Check.Require(listeningPoint, "listeningPoint");

            _routeSet = new List<SipRecordRouteHeader>();
            _createDate = DateTime.Now;
            _localSequenceNr = -1;
            _remoteSequenceNr = -1;

            _listener = listener;
            _listeningPoint = listeningPoint;
            _headerFactory = headerFactory;
            _messageFactory = messageFactory;
            _addressFactory = addressFactory;
            _messageSender = messageSender;
        }

        public abstract void SetLastResponse(SipResponse response);

        public string GetId()
        {
            bool missingTag = string.IsNullOrEmpty(LocalTag) || string.IsNullOrEmpty(RemoteTag);
            if (missingTag) return string.Empty;
            //in comment due to logging dificulties. if(_state == DialogState.Null) throw new SipCoreException("GetId Failed. The dialog can not be in Null state.");
            return CallId + ":" + LocalTag + ":" + RemoteTag;
        }

        public SipRequest CreateRequest(string method)
        {
            if (State < DialogState.Early) throw new InvalidOperationException("The dialog is unable to create the request. A dialog in 'NULL' state can not create requests.");

            Check.IsTrue(SipMethods.IsMethod(method), "method argument must be a SIP method");

            var cseqHeader = _headerFactory.CreateSCeqHeader(method, _localSequenceNr);
            var toAddress = _addressFactory.CreateAddress(null, _remoteUri);
            var toHeader = _headerFactory.CreateToHeader(toAddress, _remoteTag);
            var fromAddress = _addressFactory.CreateAddress(null, _localUri);
            var fromHeader = _headerFactory.CreateFromHeader(fromAddress, _localTag);
            var callIdheader = _headerFactory.CreateCallIdHeader(_callId);
            var viaHeader = _headerFactory.CreateViaHeader(_listeningPoint, SipConstants.Udp, SipUtil.CreateBranch());
            var requestUri = _remoteUri.Clone();

            var maxForwardsHeader = _headerFactory.CreateMaxForwardsHeader();
            var request = _messageFactory.CreateRequest(
                requestUri,
                method,
                callIdheader,
                cseqHeader,
                fromHeader,
                toHeader,
                viaHeader,
                maxForwardsHeader);

            foreach (var route in _routeSet)
            {
                request.Routes.Add(new SipRouteHeader() { SipUri = route.SipUri, Parameters = route.Parameters });
            }

            return request;
        }

        protected DialogState GetCorrespondingState(int statusCode)
        {
            return (DialogState)Math.Min(3, statusCode / 100);
        }

        public abstract void Terminate();

        public abstract void SendRequest(ISipClientTransaction transaction);

        protected void SendRequest(ISipClientTransaction transaction, int sequenceNr)
        {
            Check.Require(transaction, "tx");
            Check.Require(transaction.Request, "tx.Request");

            if (transaction.Request.RequestLine.Method == SipMethods.Bye)
            {
                if (_state != DialogState.Confirmed)
                    throw new SipCoreException("'BYE' request can not be sent. Only dialogs in 'CONFIRMED' state can send 'BYE' requests.");
            }
            
            var method = transaction.Request.RequestLine.Method;

            transaction.Request.CSeq.Sequence = sequenceNr;

            transaction.SendRequest();
            
            if (method == SipMethods.Bye)
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Dialog[Id={0}] has send '{1}' request. Terminating dialog...", GetId(), method);

                Terminate();
            }
        }

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            if (_logger.IsDebugEnabled) _logger.Debug("Dialog[Id={0}] received as request[method={1}]", GetId(), requestEvent.Request.RequestLine.Method);
            var result = new DialogResult();
            result.InformToUser = true;

            var seqNr = requestEvent.Request.CSeq.Sequence;

            /* If the remote sequence number is empty, it MUST be set to the value
              of the sequence number in the CSeq header field value in the request.
              If the remote sequence number was not empty, but the sequence number
              of the request is lower than the remote sequence number, the request
              is out of order and MUST be rejected with a 500 (Server Internal
              Error) response. remoteSequenceNr = unset value = -1*/
            
            if (seqNr < _remoteSequenceNr)
            {
                throw new SipException(SipResponseCodes.x500_Server_Internal_Error);
            }

            _remoteSequenceNr = seqNr;

            ProcessRequestOverride(result, requestEvent);

            if(result.InformToUser)
            {
                /*forward TODO: check rfc*/
                _listener.ProcessRequest(requestEvent);
            }
        }

        protected abstract void ProcessRequestOverride(DialogResult result, SipRequestEvent requestEvent);

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
            if(_logger.IsDebugEnabled) _logger.Debug("Dialog[Id={0}] received response[StatusCode={1}].", GetId(), responseEvent.Response.StatusLine.StatusCode);

            /*moved invocation out of ctx, to here. ctx now invokes ProcessResponse*/
            SetLastResponse(responseEvent.Response);

            var r = new DialogResult();
            r.InformToUser = true;
            ProcessResponseOverride(r, responseEvent);

            if (r.InformToUser)
            {
                /*forward TODO: check rfc*/
                _listener.ProcessResponse(responseEvent);
            }
        }

        protected abstract void ProcessResponseOverride(DialogResult result, SipResponseEvent responseEvent);

        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {
            if (_logger.IsDebugEnabled) _logger.Debug("Dialog[Id={0}] received a timeout. Forwarding to listener...", GetId());

            /*A UAC SHOULD also terminate a dialog if
               no response at all is received for the request (the client
               transaction would inform the TU about the timeout.) (==> and not the dialog)*/

            _listener.ProcessTimeOut(timeOutEvent);
        }
        
        protected void CheckFirstResponse(SipResponse response)
        {
            Check.Require(response, "response");
            Check.IsTrue(response.CSeq.Command == SipMethods.Invite, "The response can not have a command other then 'INVITE'");
            Check.IsTrue(response.StatusLine.StatusCode != 100, "The response can not be 'TRYING'");
            Check.IsTrue(response.StatusLine.StatusCode / 100 == 1, "The response must be provisonal");
            Check.IsTrue(response.From.Tag != null, "From must have a tag");
            Check.IsTrue(response.To.Tag != null, "To must have a tag");
            Check.IsTrue(response.Contacts.GetTopMost() != null, "The response must have a Contact header.");
        }
    }
}