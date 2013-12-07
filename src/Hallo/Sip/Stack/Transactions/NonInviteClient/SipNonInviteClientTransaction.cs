using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using Hallo.Util;

namespace Hallo.Sip.Stack.Transactions.NonInviteClient
{
    public class SipNonInviteClientTransaction : SipAbstractClientTransaction
    {
        internal static readonly TryingCtxState TryingState = new TryingCtxState();
        internal static readonly ProceedingCtxState ProceedingState = new ProceedingCtxState();
        internal static readonly CompletedCtxState CompletedState = new CompletedCtxState();

        internal 
            
            SipNonInviteClientTransaction(
            SipClientTransactionTable table,
            ISipMessageSender messageSender,
            ISipListener listener,
            SipRequest request,
            ITimerFactory timerFactory) 
            : base(table, request, messageSender, listener, timerFactory)
        {
            Check.IsTrue(request.RequestLine.Method != SipMethods.Invite, "Method 'INVITE' is not allowed");
            Check.IsTrue(request.RequestLine.Method != SipMethods.Ack, "Method 'ACK' is not allowed");

            _logger = NLog.LogManager.GetCurrentClassLogger();

            ReTransmitTimer = _timerFactory.CreateNonInviteCtxRetransmitTimer(OnReTransmit);
            TimeOutTimer = _timerFactory.CreateNonInviteCtxTimeOutTimer(OnTimeOut);
            EndCompletedTimer = _timerFactory.CreateNonInviteCtxEndCompletedTimer(OnCompletedEnded);
        }

        private void OnReTransmit()
        {
            lock (_lock)
            {
                /* ignore callbacks when the timer is disposed. These can potentially happen when
                * timercallbacks, queued by the threadpool, are invoked (with a certain delay !!)*/

                if(!ReTransmitTimer.IsDisposed) State.Retransmit(this);
            }
        }

        private void OnTimeOut()
        {
            /* ignore timeouts in the completed state. These can potentially happen when
             * timercallbacks, queued by the threadpool, are invoked (with a certain delay !!)*/
            if (State == CompletedState) return;

            Dispose();

            _listener.ProcessTimeOut(new SipClientTxTimeOutEvent() { Request = Request, ClientTransaction = this });
        }

        private void OnCompletedEnded()
        {
            Dispose();
        }

        public override void Dispose()
        {
            lock (_lock)
            {
                if (_isDisposed) return;
                _isDisposed = true;
                ReTransmitTimer.Dispose();
                TimeOutTimer.Dispose();
                EndCompletedTimer.Dispose();

                SipAbstractClientTransaction tx;
                _table.TryRemove(this.GetId(), out tx);
            }

            if (_stateObserver != null)
            {
                _stateObserver.OnNext(CreateStateInfo(SipTransactionStateName.Terminated));
                _stateObserver.OnCompleted();
            }
        }

        internal AbstractCtxState State { get; private set; }
        internal ITimer ReTransmitTimer { get; set; }
        internal ITimer TimeOutTimer { get; set; }
        internal ITimer EndCompletedTimer { get; set; }

        public override void SendRequest()
        {
            if (!_table.TryAdd(this.GetId(), this))
                throw new Exception(
                    string.Format("Could not add client transaction. The id already exists. Id:'{0}'. .", this.GetId()));

            ChangeState(TryingState);

            SendRequestInternal();
        }

        public override void ProcessResponse(SipResponseEvent responseEvent)
        {
            StateResult result;
            lock (_lock)
            {
                result = State.HandleResponse(this, responseEvent.Response);
            }

            responseEvent.ClientTransaction = this;

            if (result.InformToUser)
            {
                if (GetDialog() != null)
                {
                    if (_logger.IsDebugEnabled) _logger.Debug("Tx is holding a dialog. Invoking ProcessResponse on Dialog.");

                    GetDialog().ProcessResponse(responseEvent);
                }
                else
                {
                    if (_logger.IsDebugEnabled) _logger.Debug("Passing response to listener: '{0}'", _listener.GetType().Name);

                    _listener.ProcessResponse(responseEvent);
                }
            }
        }
       
        internal void ChangeState(AbstractCtxState newstate)
        {
            /*attention: no locking. Is already called thread safely via call to HandleResponseContext*/
            State = newstate;
            newstate.Initialize(this);

            if(_stateObserver != null) _stateObserver.OnNext(CreateStateInfo(newstate.Name));
        }
        
        public override SipTransactionType Type
        {
            get { return SipTransactionType.NonInviteClient; }
        }
        
    }
}
