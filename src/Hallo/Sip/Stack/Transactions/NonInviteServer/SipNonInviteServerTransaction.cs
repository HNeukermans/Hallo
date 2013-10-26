using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using NLog;
using Hallo.Util;

namespace Hallo.Sip.Stack.Transactions.NonInviteServer
{
    public partial class SipNonInviteServerTransaction : SipAbstractServerTransaction
    {
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
       
        internal static readonly TryingStxState TryingState = new TryingStxState();
        internal static readonly ProceedingStxState ProceedingState = new ProceedingStxState();
        internal static readonly CompletedStxState CompletedState = new CompletedStxState();
        
        private AbstractStxState _state;

        internal ITimer EndCompletedTimer { get; set; }

        internal SipNonInviteServerTransaction(
            SipServerTransactionTable table, 
            SipRequest request, 
            ISipListener listener, 
            ISipMessageSender messageSender,
            ITimerFactory timerFactory)
            : base(table, request, listener, messageSender, timerFactory)
        {
            Check.IsTrue(request.RequestLine.Method != SipMethods.Invite, "Request method can not be 'INVITE'.");
            Check.IsTrue(request.RequestLine.Method != SipMethods.Ack, "Request method can not be 'ACK'.");

            EndCompletedTimer = _timerFactory.CreateNonInviteStxEndCompletedTimer(OnCompletedEnded);
        }

        internal override void Initialize()
        {
            if (!_table.TryAdd(this.GetId(), this))
                throw new Exception(
                    string.Format("Could not add client transaction. The id already exists. Id:'{0}'. .", this.GetId()));
            
            ChangeState(TryingState);
        }
        
        private void OnCompletedEnded()
        {
            Dispose();
        }
 
        public override void SendResponse(SipResponse response)
        {
            lock(_lock)
            {
                /*thread-safety is assured*/
                if(_state == null) Initialize();

                _state.HandleSendingResponse(this, response);
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
            if (result.InformToUser)
            {
                _listener.ProcessRequest(requestEvent);
            }
        }

        //internal void SendResponseInternal(SipResponse response = null)
        //{
        //    if(response != null) LatestResponse = response;
        //    _provider.SendResponse(LatestResponse);
        //}

        public override void Dispose()
        {
            lock (_lock)
            {
                if (_isDisposed) return;
                _isDisposed = true;
                EndCompletedTimer.Dispose();

                SipAbstractServerTransaction tx;
                if(!_table.TryRemove(this.GetId(), out tx))
                {
                    if(_logger.IsWarnEnabled) _logger.Warn("The server transaction with id '{0}' could not be removed from the table.");
                }
            }

            if (_stateObserver != null)
            {
                _stateObserver.OnNext(CreateStateInfo(SipTransactionStateName.Terminated));
                _stateObserver.OnCompleted();
            }
        }

        internal void ChangeState(AbstractStxState newstate)
        {
            /*attention: no locking. Is already called thread safely via call to SendResponse*/
            _state = newstate;
            newstate.Initialize(this);

            if (_stateObserver != null) _stateObserver.OnNext(CreateStateInfo(newstate.Name));
        }

        internal AbstractStxState State
        {
            get { return _state; }
        }

        public override SipTransactionType Type
        {
            get { return SipTransactionType.NonInviteServer; }
        }

    }
}
