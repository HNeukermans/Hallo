using System;
using System.Net;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack.Transactions;
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
        //private ITimer _endWaitForAckTimer;
        private object _lock = new object();
        private readonly SipRequest _firstRequest;

        public SipInviteServerDialog(
             ISipServerTransaction transaction, 
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

            _firstRequest = transaction.Request;
            //(only ?) localtag is set on firstresponse
            //localtarget is not defined, because is has no use, (every user agent knows it local address)

            _retransmitOkTimer = _timerFactory.CreateInviteCtxRetransmitTimer(OnOkReTransmit);
            //_endWaitForAckTimer = _timerFactory.CreateInviteCtxTimeOutTimer(OnWaitForAckTimeOut);
            
            if (_logger.IsInfoEnabled) _logger.Info("ServerDialog[Id={0}] created.", GetId());
        }

        private void ValidateRequest(SipRequest request)
        {
            Check.IsTrue(request.Contacts.Count == 1, "An invite request must always have one contact header");
            Check.IsTrue(!string.IsNullOrEmpty(request.From.Tag), "An invite request from header must have a tag");
        }

        public override void SendRequest(ISipClientTransaction transaction)
        {
            Check.Require(transaction, "tx");
            Check.Require(transaction.Request, "tx.Request");

            var method = transaction.Request.RequestLine.Method;
            if (method == SipMethods.Cancel || method == SipMethods.Ack)
            {
                /*throw exception*/
                throw new SipCoreException("The dialog can not send a '{0}' request. '{0}' requests can only be send by ClientDialogs");
            }

            lock (_lock) ++_localSequenceNr;

            SendRequest(transaction, _localSequenceNr);
        }


        public override void SetLastResponse(SipResponse response)
        {
            if (_logger.IsDebugEnabled) _logger.Debug("ServerDialog[Id={0}]. Reponse[StatusCode:'{1}']", GetId(), response.StatusLine.StatusCode);

            Check.Require(response, "response");
            
            if (response.StatusLine.StatusCode == 100)
            {
                if (_logger.IsDebugEnabled)
                    _logger.Debug("ServerDialog[Id={0}]. StatusCode == 100. Ignoring 'TRYING' response");
                return;
            }

            bool terminate = false;
            lock(_lock)
            {
                var newResponseState = GetCorrespondingState(response.StatusLine.StatusCode);

                if (_firstResponse == null)
                {
                    CheckFirstResponse(response);
                    _firstResponse = response;
                    SetDialogProps();
                }
                if(newResponseState > _state)
                {
                    if (_logger.IsInfoEnabled)
                        _logger.Info("ServerDialog[Id={0}]: State Transition: '{1}'-->'{2}'", GetId(), _state, newResponseState);
                    
                    _state = newResponseState;
                    
                    if (_state == DialogState.Early)
                    {
                        if (!_dialogTable.TryAdd(GetId(), this))
                        {
                            throw new SipCoreException("Could not add ServerDialog[Id={0}] to table, because it already exists.", GetId());
                        }

                        if (_logger.IsDebugEnabled) _logger.Debug("ServerDialog[Id={0}] added to table.", GetId());
                    }
                    else if (_state == DialogState.Confirmed)
                    {
                        if (_logger.IsDebugEnabled)
                            _logger.Debug("ServerDialog[Id={0}]. RETRANSMIT_OK & WAIT_FOR_ACK timers started.", GetId());

                        _okResponse = response;
                        /*start timers*/
                        _retransmitOkTimer.Start();
                        //_endWaitForAckTimer.Start();
                    }
                    else if (_state == DialogState.Terminated)
                    {
                        terminate = true;
                    }
                }
            }
            
            if(terminate)
            {
                //terminate outside of lock, to prevent from deadlock !!
                //(terminate is a public method and uses a lock)
                Terminate();
            }
        }

        public override void Terminate()
        {
            lock(_lock)
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
            }

            _retransmitOkTimer.Dispose();
            //_endWaitForAckTimer.Dispose();

        }

        protected override void ProcessRequestOverride(DialogResult result, SipRequestEvent requestEvent)
        {
            
        }

        protected override void ProcessResponseOverride(DialogResult result, SipResponseEvent responseEvent)
        {
            
        }       

        private void OnOkReTransmit()
        {
            if (_logger.IsDebugEnabled) _logger.Info("ServerDialog[Id={0}], Retransmitting OK.", GetId());
            
            /*double timer*/
            _retransmitOkTimer.Interval = Math.Min(_retransmitOkTimer.Interval*2, SipConstants.T2);
            _messageSender.SendResponse(_okResponse);
        }
       
        public bool HasAckReceived { get; set; }

        private void SetDialogProps()
        {
            _routeSet = _firstRequest.RecordRoutes.ToList();
            _remoteTag = _firstRequest.From.Tag;
            _remoteTarget = _firstRequest.Contacts.GetTopMost().SipUri;
            _remoteSequenceNr = _firstRequest.CSeq.Sequence;
            _callId = _firstRequest.CallId.Value;
            _remoteUri = _firstRequest.From.SipUri;
            _localUri = _firstRequest.To.SipUri;
            _localTag = _firstResponse.To.Tag;
        }

        ///// <summary>
        ///// Sends the OK response and handles retransmitions of OK.
        ///// </summary>
        ///// <param name="okResponse"></param>
        //internal void SendOk(SipResponse okResponse)
        //{
        //    Check.Require(okResponse, "okResponse");
        //    Check.IsTrue(okResponse.StatusLine.StatusCode / 100 == 2, "parameter 'okResponse' is not an ok response.");

        //    SetLastResponse(okResponse);

        //    _messageSender.SendResponse(okResponse);
        //}
    }
}