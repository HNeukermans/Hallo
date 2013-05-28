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
    public class SipInviteClientDialog : AbstractDialog, ISipDialog
    {
        private readonly SipDialogTable _dialogTable;
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly SipRequest _firstRequest;
        private SipResponse _firstResponse;
        private bool _isAckSent = false;

         public SipInviteClientDialog(
             SipInviteClientTransaction transaction, 
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

             _dialogTable = dialogTable;
             _state = DialogState.Null;
             _firstRequest = transaction.Request;
             _topMostVia = (SipViaHeader)transaction.Request.Vias.GetTopMost().Clone();
             transaction.SetDialog(this);
             transaction
                 .Observe()
                 .Where(s => s.CurrentState == SipTransactionStateName.Terminated)
                 .Subscribe((s) => OnInviteTxTerminated());
         }

        private void OnInviteTxTerminated()
        {
            if(_state == DialogState.Null)
            {
                /*the tx has terminated and a provisional response hasn't been received yet.*/
                Dispose();
            }
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

        public override void SetLastResponse(SipResponse response)
        {
             Check.Require(response, "response");

             if (response.StatusLine.StatusCode == 100)
             {
                 if (_logger.IsDebugEnabled)
                     _logger.Debug("StatusCode == 100. Ignoring 'TRYING' response");
                 return;
             }
             if (string.IsNullOrEmpty(response.To.Tag))
             {
                 if (_logger.IsDebugEnabled)
                     _logger.Debug("To tag is null. Ignoring '{0}' response", response.StatusLine.FormatToString());
                 return;
             }

             if (_firstResponse == null)
             {
                 CheckFirstResponse(response);

                 _firstResponse = response;

                 response.RecordRoutes.ToList().ForEach(rr =>
                 {
                    if (!rr.SipUri.IsLooseRouting)
                        throw new SipException("Strict routing is not supported. Use loose routing only.");
                 });
                
                 _routeSet = response.RecordRoutes.ToList();
                 _routeSet.Reverse();
                 _remoteTarget = response.Contacts.GetTopMost().SipUri;
                 _localSequenceNr = _firstRequest.CSeq.Sequence;
                 /*_remoteSequenceNr remains empty */
                 _callId = _firstRequest.CallId.Value;
                 _localTag = _firstRequest.From.Tag;
                 _remoteTag = response.To.Tag;
                 _remoteUri = response.To.SipUri;
                 _localUri = _firstRequest.From.SipUri;
                
                 if (!_dialogTable.TryAdd(GetId(), this))
                 {
                     _logger.Warn("could not add dialog to table, because it already exists");
                 }
             }


             var lastResponseState = GetCorrespondingState(response.StatusLine.StatusCode);

             if (lastResponseState > _state)
             {
                 _state = lastResponseState;

                 //if (_state == DialogState.Early)
                 //{

                 //}
             }
         }

        public override void Terminate()
        {
            AbstractDialog removed;
            if(!_dialogTable.TryRemove(GetId(), out removed))
            {
                _logger.Warn("could not remove dialog with id: {0}", GetId());
            }

            _state = DialogState.Terminated;
        }

        protected void CheckFirstResponse(SipResponse response)
        {
             Check.Require(response, "response");
             Check.IsTrue(response.CSeq.Command == SipMethods.Invite, "The response can not have a command other then 'INVITE'");
             Check.IsTrue(response.StatusLine.StatusCode != 100, "The response can not be 'TRYING'");
             Check.Require(response.From.Tag != null, "From must have a tag");
             Check.Require(response.To.Tag != null, "To must have a tag");
        }
    }
}