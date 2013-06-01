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
        private SipResponse _firstResponse;
        private SipResponse _okResponse;
        private ITimer _retransmitOkTimer;
        private ITimer _endWaitForAckTimer;

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
            
            _logger = NLog.LogManager.GetCurrentClassLogger();

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

            _retransmitOkTimer = _timerFactory.CreateInviteCtxRetransmitTimer(OnReTransmit);
            _endWaitForAckTimer = _timerFactory.CreateInviteCtxTimeOutTimer(OnWaitForAckTimeOut);
            
            if (_logger.IsInfoEnabled) _logger.Info("ServerDialog[Id={0}] created.", GetId());
        }

        private void ValidateRequest(SipRequest request)
        {
            Check.IsTrue(request.Contacts.Count == 1, "An invite request must always have one contact header");
            Check.IsTrue(!string.IsNullOrEmpty(request.From.Tag), "An invite request from header must have a tag");
        }

        public override void SetLastResponse(SipResponse response)
        {
            if (_logger.IsDebugEnabled) _logger.Debug("ServerDialog[Id={0}]. SetLastResponse() invocation with argument Reponse[StatusCode:'{1}']", GetId(), response.StatusLine.StatusCode);

            Check.Require(response, "response");
            
            if (response.StatusLine.StatusCode == 100)
            {
                if (_logger.IsDebugEnabled)
                    _logger.Debug("ServerDialog[Id={0}]. StatusCode == 100. Ignoring 'TRYING' response");
                return;
            }
            
            if (_firstResponse == null)
            {
                CheckFirstResponse(response);
                _firstResponse = response;
                _localTag = _firstResponse.To.Tag;
            }

            var lastResponseState = GetCorrespondingState(response.StatusLine.StatusCode);

            /*check state transition*/
            if (lastResponseState > _state)
            {
                if (_logger.IsInfoEnabled)
                    _logger.Info("ServerDialog[Id={0}]: State Transition: '{1}'-->'{2}'", GetId(), _state,
                                 lastResponseState);

                _state = lastResponseState;

                if (_state == DialogState.Early)
                {
                    if (!_dialogTable.TryAdd(GetId(), this))
                    {
                        throw new SipCoreException("Could not add ServerDialog[Id={0}] to table, because it already exists.", GetId());
                    }

                    if (_logger.IsDebugEnabled) _logger.Debug("ServerDialog[Id={0}] added to table.", GetId());
                }
                else if(_state == DialogState.Confirmed)
                {
                    if (_logger.IsDebugEnabled)
                        _logger.Debug("ServerDialog[Id={0}]. RETRANSMIT_OK & WAIT_FOR_ACK timers started.", GetId());

                    _okResponse = response;
                    /*start timers*/
                    _retransmitOkTimer.Start();
                    _endWaitForAckTimer.Start();
                }
            }
        }

        public override void Terminate()
        {
            SipAbstractDialog removed;
            
            if (_logger.IsDebugEnabled) _logger.Debug("Trying to remove ServerDialog[Id={0}] from the table...", GetId());
            
            if (!_dialogTable.TryRemove(GetId(), out removed))
            {
                _logger.Warn("Could not remove dialog with id: {0}", GetId());
            }
            else
            {
                if (_logger.IsDebugEnabled) _logger.Debug("ServerDialog[Id={0}] removed from table.", GetId());
            }

            _state = DialogState.Terminated;

            _retransmitOkTimer.Dispose();
            _endWaitForAckTimer.Dispose();
        }

        private void OnWaitForAckTimeOut()
        {
            if (_logger.IsInfoEnabled) _logger.Info("ServerDialog[Id={0}], ACK has not been received after 64 * T1. Terminating dialog...", GetId());
            
            /*ack not been received after 64 * T1 end dialog. TODO:terminate session*/
            Terminate();
        }

        private void OnReTransmit()
        {
            if (_logger.IsDebugEnabled) _logger.Info("ServerDialog[Id={0}], Retransmitting OK.", GetId());
            
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