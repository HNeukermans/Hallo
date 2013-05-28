using System;
using System.Net;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using Hallo.Sip.Util;
using Hallo.Util;
using NLog;

namespace Hallo.Sip.Stack.Dialogs
{
    public sealed class SipInviteServerDialog : SipAbstractDialog, ISipDialog
    {
        private ITimerFactory _timerFactory;
        private readonly SipDialogTable _dialogTable;
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private SipResponse _firstResponse;
        private SipResponse _okResponse;
        private ITimer _retransmitOkTimer;
        private ITimer _ackWaitTimeOutTimer;

        public SipInviteServerDialog(
            ISipInviteServerTransaction transaction, 
            SipDialogTable dialogTable,
            ITimerFactory timerFactory,
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
            Check.Require(timerFactory, "timerFactory");
            Check.Require(transaction.Request, "transaction.Request");
            ValidateRequest(transaction.Request);
            
            _dialogTable = dialogTable;
            _state = DialogState.Null;
            _timerFactory = timerFactory;
            
            _routeSet = transaction.Request.RecordRoutes.ToList();
            _remoteTag = transaction.Request.From.Tag;
            _remoteTarget = transaction.Request.Contacts.GetTopMost().SipUri;
            _remoteSequenceNr = transaction.Request.CSeq.Sequence;
            _callId = transaction.Request.CallId.Value;
            _remoteUri = transaction.Request.From.SipUri;
            _localUri = transaction.Request.To.SipUri;
            //(only ?) localtag is set on firstresponse
            //localtarget is not defined, because is has no use, (every user agent knows it local address)

            transaction.SetDialog(this);
            
            if(_logger.IsInfoEnabled) _logger.Info("Dialog created at server. State:'{0}'", _state);
        }

        private void ValidateRequest(SipRequest request)
        {
            Check.IsTrue(request.Contacts.Count == 1, "An invite request must always have one contact header");
            Check.IsTrue(!string.IsNullOrEmpty(request.From.Tag), "An invite request from header must have a tag");
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
            
            if (_firstResponse == null)
            {
                CheckFirstResponse(response);
                _firstResponse = response;
                _localTag = _firstResponse.To.Tag;
                
                if (!_dialogTable.TryAdd(GetId(), this))
                {
                    _logger.Warn("could not add dialog to table, because it already exists");
                }
            }

            var lastResponseState = GetCorrespondingState(response.StatusLine.StatusCode);

            if (lastResponseState > _state)
            {
                _state = lastResponseState;
            }

            if(response.StatusLine.StatusCode == 200)
            {
                _okResponse = response;
                /*start retransmittimer*/
                _retransmitOkTimer = _timerFactory.CreateInviteCtxRetransmitTimer(OnReTransmit);
                _ackWaitTimeOutTimer = _timerFactory.CreateInviteCtxTimeOutTimer(OnWaitForAckTimeOut);
            }
        }

        public override void Terminate()
        {
            SipAbstractDialog removed;
            if (!_dialogTable.TryRemove(GetId(), out removed))
            {
                _logger.Warn("could not remove dialog with id: {0}", GetId());
            }

            _state = DialogState.Terminated;

            _retransmitOkTimer.Dispose();
            _ackWaitTimeOutTimer.Dispose();

        }

        private void OnWaitForAckTimeOut()
        {
            /*ack not been received after 64 * T1 end dialog. TODO:terminate session*/
            Terminate();
        }

        private void OnReTransmit()
        {
            /*double timer*/
            _retransmitOkTimer.Interval = Math.Min(_retransmitOkTimer.Interval*2, SipConstants.T2);
            _messageSender.SendResponse(_okResponse);
        }

       
        public bool HasAckReceived { get; set; }

        protected void CheckFirstResponse(SipResponse response)
        {
            Check.Require(response, "response");
            Check.IsTrue(response.CSeq.Command == SipMethods.Invite, "The response can not have a command other then 'INVITE'");
            Check.IsTrue(response.StatusLine.StatusCode != 100, "The response can not be 'TRYING'");
            Check.IsTrue(response.StatusLine.StatusCode / 100 == 1, "The response must be provisonal");
            Check.Require(response.From.Tag != null, "From must have a tag");
            Check.Require(response.To.Tag != null, "To must have a tag");
            Check.Require(response.Contacts.GetTopMost() != null, "The response must have a Contact header.");
        }

        
    }
}