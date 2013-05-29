using System;
using System.Collections.Generic;
using System.Net;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using Hallo.Sip.Util;
using Hallo.Util;

namespace Hallo.Sip.Stack.Dialogs
{
    public abstract class SipAbstractDialog : ISipListener
    {
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
            return CallId + ":" + LocalTag + ":" + RemoteTag;
        }

        public SipRequest CreateRequest(string method)
        {
            if (State < DialogState.Early) throw new InvalidOperationException("The dialog is unable to create an 'ACK' Request. To be able to create an 'ACK' request, the Tx can not be in 'Null' state.");

            Check.IsTrue(SipMethods.IsMethod(method), "method argument must be a SIP method");

            var cseqHeader = _headerFactory.CreateSCeqHeader(method, _localSequenceNr++);
            var toAddress = _addressFactory.CreateAddress(null, _remoteUri);
            var toHeader = _headerFactory.CreateToHeader(toAddress, _remoteTag);
            var fromAddress = _addressFactory.CreateAddress(null, _localUri);
            var fromHeader = _headerFactory.CreateFromHeader(fromAddress, _localTag);
            var callIdheader = _headerFactory.CreateCallIdHeader(_callId);
            var viaHeader = _headerFactory.CreateViaHeader(_listeningPoint, SipConstants.Udp, SipUtil.CreateBranch());
            var requestUri = _remoteUri.Clone();

            //var viaHeader = _topMostVia; 
            var maxForwardsHeader = _headerFactory.CreateMaxForwardsHeader();
            var request = _messageFactory.CreateRequest(
                requestUri,
                SipMethods.Ack,
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

        public void SendRequest(ISipClientTransaction tx)
        {
            tx.SendRequest();

            if(tx.Request.RequestLine.Method == SipMethods.Bye)
            {
                Terminate();
            }
        }

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            throw new NotImplementedException();
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
            throw new NotImplementedException();
        }

        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {
            throw new NotImplementedException();
        }
    }
}