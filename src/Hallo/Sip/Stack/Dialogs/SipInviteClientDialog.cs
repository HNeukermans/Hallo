using System;
using System.Net;
using System.Reactive.Linq;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.Sip.Util;
using Hallo.Util;
using NLog;
using Ninject.Activation;

namespace Hallo.Sip.Stack.Dialogs
{
    public class SipInviteClientDialog : SipAbstractDialog, ISipDialog
    {
        private readonly SipDialogTable _dialogTable;
        private readonly SipRequest _firstRequest;
        private SipResponse _firstResponse;
        private bool _isAckSent = false;
        private object _lock = new object();

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
        
        private void Dispose()
        {
            
        }

        public SipRequest CreateAck()
        {
            /*12.2.1.1. Generating the Request*/
            /*The sequence number of the CSeq header field MUST be the same as the INVITE being acknowledged,
             * but the CSeq method MUST be ACK. */
            var ackRequest = CreateRequest(SipMethods.Ack);
            var cseqHeader = _headerFactory.CreateSCeqHeader(SipMethods.Ack, _firstRequest.CSeq.Sequence);
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


        /// <summary>
        /// TODO: needs to be locked!!!!
        /// </summary>
        /// <param name="response"></param>
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

             var newResponseState = GetCorrespondingState(response.StatusLine.StatusCode);
             DialogState? previousState = null;
             lock (_lock)
             {
                 if (_firstResponse == null)
                 {
                     CheckFirstResponse(response);
                     _firstResponse = response;
                     SetDialogProps();
                 }
                 if (newResponseState > _state)
                 {
                     previousState = _state;
                     _state = newResponseState;
                 }
             }

             /*use prevState as a flag for state transition*/
             if (previousState.HasValue)
             {
                 if (_logger.IsInfoEnabled)
                     _logger.Info("ClientDialog[Id={0}]: State Transition: '{1}'-->'{2}'", GetId(), previousState,
                                  _state);

                 if (_state == DialogState.Early)
                 {
                     if (!_dialogTable.TryAdd(GetId(), this))
                     {
                         throw new SipCoreException("Could not add ClientDialog[Id={0}] to table, because it already exists.", GetId());
                     }

                     if (_logger.IsDebugEnabled) _logger.Debug("ClientDialog[Id={0}] added to table.", GetId());
                 }
             }
            
             //if (lastResponseState > _state)
             //{
             //    _state = lastResponseState;

             //    if (_logger.IsInfoEnabled)
             //        _logger.Info("ClientDialog[Id={0}]: State Transition: '{1}'-->'{2}'", GetId(), _state,
             //                     lastResponseState);

             //    if (_state == DialogState.Early)
             //    {
             //    }
             //    else if (_state == DialogState.Confirmed)
             //    {
             //        //_okResponse = response;
             //        ///*start timers*/
             //        //_retransmitOkTimer.Start();
             //        //_endWaitForAckTimer.Start();
             //    }
             //}
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
            SipAbstractDialog removed;
            if(!_dialogTable.TryRemove(GetId(), out removed))
            {
                _logger.Warn("could not remove dialog with id: {0}", GetId());
            }

            _state = DialogState.Terminated;
        }

        protected override void ProcessRequestOverride(DialogResult result, SipRequestEvent requestEvent)
        {
            
        }

        protected override void ProcessResponseOverride(DialogResult result, SipResponseEvent responseEvent)
        {
            if(responseEvent.Response.StatusLine.StatusCode / 100 == 2)
            {
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