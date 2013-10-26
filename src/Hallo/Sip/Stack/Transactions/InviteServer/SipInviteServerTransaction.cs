using System;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using Hallo.Util;

namespace Hallo.Sip.Stack.Transactions.InviteServer
{
    public partial class SipInviteServerTransaction : SipAbstractServerTransaction
    {
        internal static readonly ConfirmedStxState ConfirmedState = new ConfirmedStxState();
        internal static readonly ProceedingStxState ProceedingState = new ProceedingStxState();
        internal static readonly CompletedStxState CompletedState = new CompletedStxState();
        internal static readonly TerminatedStxState TerminatedState = new TerminatedStxState();

        private AbstractStxState _state;
        private SipInviteServerDialog _dialog;
        
        internal SipInviteServerTransaction(
            SipServerTransactionTable table,
            ISipMessageSender messageSender,
            ISipListener listener,
            SipRequest request,
            ITimerFactory timerFactory)
            : base(table, request, listener, messageSender, timerFactory)
        {
            Check.IsTrue(request.RequestLine.Method == SipMethods.Invite, "Method other then 'INVITE' is not allowed");
            
            ReTransmitNonx200FinalResponseTimer = _timerFactory.CreateInviteStxRetransmitTimer(OnReTransmitNonx200FinalResponse);
            EndCompletedTimer = _timerFactory.CreateInviteStxEndCompletedTimer(OnCompletedEnded);
            EndConfirmedTimer = _timerFactory.CreateInviteStxEndConfirmed(OnConfirmedEnded);
            SendTryingTimer = _timerFactory.CreateInviteStxSendTryingTimer(OnSendTryingTimer);
        }

        private void OnSendTryingTimer()
        {
            if(LatestResponse == null)
            {
                LatestResponse = Request.CreateResponse(SipResponseCodes.x100_Trying);
                SendResponseInternal();
            }
        }

        internal override void Initialize()
        {
            if (!_table.TryAdd(this.GetId(), this))
                throw new Exception(
                    string.Format("Could not add server transaction. The id already exists. Id:'{0}'. .", this.GetId()));
            
            ChangeState(ProceedingState);
        }

        private void OnReTransmitNonx200FinalResponse()
        {
            lock (_lock)
            {
                if(State == CompletedState)
                {
                    var completedState = State as CompletedStxState;
                    completedState.RetransmitNonx200FinalResponse(this);
                }
            }
        }

        public override void Dispose()
        {
            lock (_lock)
            {
                if (_isDisposed) return;
                _isDisposed = true;
                ReTransmitNonx200FinalResponseTimer.Dispose();
                EndConfirmedTimer.Dispose();
                EndCompletedTimer.Dispose();
                SendTryingTimer.Dispose();

                SipAbstractServerTransaction tx;
                _table.TryRemove(this.GetId(), out tx);

                 _state = TerminatedState;
            }

            if (_stateObserver != null)
            {
                _stateObserver.OnNext(CreateStateInfo(_state.Name));
                _stateObserver.OnCompleted();
            }
        }

        internal AbstractStxState State
        {
            get { return _state; }
        }

        internal ITimer ReTransmitNonx200FinalResponseTimer { get; set; }
        internal ITimer EndConfirmedTimer { get; set; }
        internal ITimer EndCompletedTimer { get; set; }
        internal ITimer SendTryingTimer { get; set; }
        
        private void OnCompletedEnded()
        {
            _listener.ProcessTimeOut(new SipServerTxTimeOutEvent(){ Request = Request, ServerTransaction = this});
            Dispose();
        }

        private void OnConfirmedEnded()
        {
            Dispose();
        }

        public override void SendResponse(SipResponse response)
        {
            
            StateResult result;
            if(LatestResponse == null)
            {
                Initialize();
            }
            lock (_lock)
            {
                /*thread-safety is assured*/
                result = _state.HandleSendingResponse(this, response);
            }
            
            if(result.Dispose)
            {
                Dispose();
            }

            if (GetDialog() != null)
            {
                GetDialog().SetLastResponse(response);
            }
        }
        
        public override void ProcessRequest(SipRequestEvent requestEvent)
        {
            requestEvent.ServerTransaction = this;
            StateResult result;
            lock (_lock)
            {
                result = _state.ProcessRequest(this, requestEvent);
            }
            if(result.InformToUser)
            {
                _listener.ProcessRequest(requestEvent);
            }
        }

        internal void ChangeState(AbstractStxState newstate)
        {
            /*attention: no locking. Is already called thread safely via call to SendResponse */
            _state = newstate;
            newstate.Initialize(this);

            if (_stateObserver != null) _stateObserver.OnNext(CreateStateInfo(newstate.Name));
        }
        
        public override SipTransactionType Type
        {
            get { return  SipTransactionType.InviteServer; }
        }

        public void SetDialog(SipInviteServerDialog dialog)
        {
            _dialog = dialog;
        }

        internal SipInviteServerDialog GetDialog()
        {
            return _dialog;
        }

    }
}
