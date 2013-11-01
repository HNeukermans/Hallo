using System;
using System.Net;
using System.Reactive.Linq;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.Sip.Util;
using Hallo.Util;
using NLog;

namespace Hallo.Sip.Stack.Dialogs
{
    public class SipInviteClientDialog : SipAbstractDialog, ISipDialog
    {
        private readonly SipDialogTable _dialogTable;
        private readonly SipRequest _firstRequest;
        private SipResponse _firstResponse;
        private bool _isAckSent = false;
        private object _lock = new object();
        private int _lastOKSequenceNr;

        public SipInviteClientDialog(
             ISipClientTransaction transaction, 
             SipDialogTable dialogTable, 
             SipHeaderFactory headerFactory,
             SipMessageFactory messageFactory,
             SipAddressFactory addressFactory,
             ISipMessageSender messageSender,
             ISipListener listener,
             IPEndPoint listeningPoint)
            : base(headerFactory, messageFactory, addressFactory, messageSender, listener, listeningPoint)
         {
             Check.Require(transaction, "transaction");
             Check.Require(dialogTable, "dialogTable");
             Check.Require(transaction.Request, "transaction.Request");
             
             _logger = NLog.LogManager.GetCurrentClassLogger();

             _dialogTable = dialogTable;
             _state = DialogState.Null;
             _firstRequest = transaction.Request;
             _topMostVia = (SipViaHeader)transaction.Request.Vias.GetTopMost().Clone();
             
         }
        
       
        public SipRequest CreateAck()
        {
            if (_state != DialogState.Confirmed)
            {
                throw new SipCoreException("The dialog can not create 'ACK' requests. Only dialogs in 'CONFIRMED' state can create 'ACK' requests.");
            }
            /*12.2.1.1. Generating the Request*/
            /*The sequence number of the CSeq header field MUST be the same as the OK being acknowledged,
             * but the CSeq method MUST be ACK. The ACK MUST contain a single Via header field, and
             * this MUST be equal to the top Via header field of the original
             * request. */
            var ackRequest = CreateRequest(SipMethods.Ack, _firstRequest.Vias.GetTopMost());
            var cseqHeader = _headerFactory.CreateSCeqHeader(SipMethods.Ack, _lastOKSequenceNr);
            ackRequest.CSeq = cseqHeader;

            return ackRequest;
        }
        
        public void SendAck(SipRequest ackRequest)
        {
            Check.Require(ackRequest, "ackRequest");
            Check.IsTrue(ackRequest.RequestLine.Method == SipMethods.Ack, "Can not send a request other then 'ACK'");
            
            _messageSender.SendRequest(ackRequest);
            
            _isAckSent = true;
        }

        public bool HasSentAck
        {
            get { return _isAckSent; }
        }


        public override void SendRequest(ISipClientTransaction transaction)
        {
            Check.Require(transaction, "tx");
            Check.Require(transaction.Request, "tx.Request");
            
            var method = transaction.Request.RequestLine.Method;

            if (method == SipMethods.Ack)
            {
                 throw new SipCoreException("'ACK' requests can not be send statefull. All 'ACK' requests must be send statelessly. Use the SendAck method instead.");
            }

            if (method == SipMethods.Cancel)
            {
                if (_state != DialogState.Early)
                    throw new SipCoreException("'CANCEL' request can not be sent. Only dialogs in 'EARLY' state can send 'CANCEL' requests.");
            }

            var seqNrToUse = 0;
            /*CSeq are incremented in each direction, excepting ACK and CANCEL, whose numbers equal the requests being acknowledged or cancelled*/

            if (method == SipMethods.Cancel)
            {
                seqNrToUse = _firstRequest.CSeq.Sequence;
            }
            else
            {

                lock (_lock) ++_localSequenceNr;
                seqNrToUse = _localSequenceNr;
            }
            
            SendRequest(transaction, seqNrToUse);

            if (method == SipMethods.Cancel)
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Dialog[Id={0}] has send '{1}' request. Terminating dialog...", GetId(), method);

                Terminate();
            }
        }

        public override void SetLastResponse(SipResponse response)
        {
            if (_logger.IsDebugEnabled) _logger.Debug("ClientDialog[Id={0}]. Reponse[StatusCode:'{1}']", GetId(), response.StatusLine.StatusCode);

             Check.Require(response, "response");

             if (response.StatusLine.StatusCode == 100)
             {
                 if (_logger.IsDebugEnabled)
                     _logger.Debug("StatusCode == 100. Ignoring 'TRYING' response");
                 return;
             }

            bool terminate = false;
             lock (_lock)
             {
                 var newResponseState = GetCorrespondingState(response.StatusLine.StatusCode);

                 if (_firstResponse == null)
                 {
                     CheckFirstResponse(response);
                     _firstResponse = response;
                     SetDialogProps();
                 }
                 if (newResponseState > _state)
                 {
                     if (_logger.IsInfoEnabled)
                         _logger.Info("ClientDialog[Id={0}]: State Transition: '{1}'-->'{2}'", GetId(), _state, newResponseState);

                     if (_state == DialogState.Null && 
                         (newResponseState == DialogState.Early || newResponseState == DialogState.Confirmed)) /*it is possible to receive an 'OK' without first receiving a 'RINGING'.*/
                     {
                         if (!_dialogTable.TryAdd(GetId(), this))
                         {
                             throw new SipCoreException("Could not add ClientDialog[Id={0}] to table, because it already exists.", GetId());
                         }

                         if (_logger.IsDebugEnabled) _logger.Debug("ClientDialog[Id={0}] added to table.", GetId());
                     }
                     else if (newResponseState == DialogState.Terminated)
                     {
                         terminate = true;
                     }

                     _state = newResponseState;
                 }
             }

             if (terminate)
             {
                 //terminate outside of lock, to prevent from deadlock !! 
                 //terminate is public method and uses a lock !!
                 Terminate();
             }
         }

        private void SetDialogProps()
        {
             _localSequenceNr = _firstRequest.CSeq.Sequence;
            /*_remoteSequenceNr remains empty */
            _callId = _firstRequest.CallId.Value;
            _localTag = _firstRequest.From.Tag;
            _remoteUri = _firstRequest.To.SipUri;
            _localUri = _firstRequest.From.SipUri;

            _remoteTag = _firstResponse.To.Tag;
            _remoteTarget = _firstResponse.Contacts.GetTopMost().SipUri;
            _routeSet = _firstResponse.RecordRoutes.ToList();//refuse looseroutin-less recordroutes
            _routeSet.Reverse();
        }

        public override void Terminate()
        {
            lock(_lock)
            {
                SipAbstractDialog removed;
                if (!_dialogTable.TryRemove(GetId(), out removed))
                {
                    _logger.Warn("could not remove dialog with id: {0}", GetId());
                }

                _state = DialogState.Terminated;
            }
        }

        protected override void ProcessRequestOverride(DialogResult result, SipRequestEvent requestEvent)
        {
            
        }

        protected override void ProcessResponseOverride(DialogResult result, SipResponseEvent responseEvent)
        {
            if(responseEvent.Response.StatusLine.StatusCode / 100 == 2)
            {
                _lastOKSequenceNr = responseEvent.Response.CSeq.Sequence;

                if(HasSentAck)
                {
                    if (_logger.IsDebugEnabled) _logger.Debug("ClientDialog[Id={0}]. Received retransmitted OK. Resending ACK...", GetId());
                    
                    /*it's an ok retransmit. Resend ack*/
                    result.InformToUser = false;
                    var acKRequest = CreateAck();
                    SendAck(acKRequest);
                    if (_logger.IsDebugEnabled) _logger.Debug("ClientDialog[Id={0}]. ACK sent.", GetId());
                }
            }
        }
    }
}