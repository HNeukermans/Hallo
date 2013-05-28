using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace Hannes.Net.Test.Console
{
    public class NonInviteClientTransaction
    {
        private readonly SipRequest _request;
        private NonInviteClientTransactionState _state;
        protected IDisposable _timeOutSubscription;

        public const int T1 = 500;
        public const int T2 = 4000;

        public NonInviteClientTransaction(SipRequest request)
        {
            _request = request;
            //Check.Require()
            //Check.Require(request.Method != SipMethods.Invite)
            
            _state = new ProceedingState(this);
        }

        protected void GoToState(NonInviteClientTransactionState from, NonInviteClientTransactionState to)
        {
            from.Stop();
            _state = to;
            to.Start();
        }

        public void Start()
        {
            var to = Observable.Timer(TimeSpan.FromMilliseconds(64 * T1), Program.MyScheduler.Instance);
            _timeOutSubscription = to.Subscribe(d => Terminate());
         
            _state.Start();
        }

        private void Terminate()
        {
            _state.Stop();
            _timeOutSubscription.Dispose();
        }

        public void ProcessResponse(SipResponse response)
        {
            _state.ProcessResponse(response);
        }

        #region friend classes

        public abstract class NonInviteClientTransactionState
        {
           

            protected readonly NonInviteClientTransaction _transaction;

            protected NonInviteClientTransactionState(NonInviteClientTransaction transaction)
            {
                _transaction = transaction;
            }

            public abstract void Start();
            public abstract void Stop();
            public abstract void ProcessResponse(SipResponse response);

           
        }

        public class ProceedingState : NonInviteClientTransactionState
        {
            private IDisposable _rxSubscription;
            private IDisposable _timeOutSubscription;

            public ProceedingState(NonInviteClientTransaction transaction) : base(transaction)
            {

            }

            public override void Start()
            {
                var rxo = Observable.Interval(TimeSpan.FromMilliseconds(T1), Program.MyScheduler.Instance).Where(i => i % 2 == 0).Where(i=> i%2==0);
                _rxSubscription = rxo.Subscribe(d => ReTransmit());
            }

            private void ReTransmit()
            {
                throw new NotImplementedException();
            }

            public override void Stop()
            {
                _rxSubscription.Dispose();
            }

            public override void ProcessResponse(SipResponse response)
            {
                _transaction.GoToState(this, new ProceedingState(_transaction));
            }
        }

        public class TryingState : NonInviteClientTransactionState
        {
            public TryingState(NonInviteClientTransaction transaction) : base(transaction)
            {
            }

            public override void Start()
            {
                throw new NotImplementedException();
            }

            public override void Stop()
            {
                throw new NotImplementedException();
            }

            public override void ProcessResponse(SipResponse response)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

    }

    public class SipRequest
    {
    }
}
