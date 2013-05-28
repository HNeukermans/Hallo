using System;
using Hallo.Component;
using Hallo.Sip.Stack;
using Hallo.UnitTest.Stubs;

namespace Hallo.UnitTest.Sip.SipNonInviteClientTransactionTests
{
    public abstract class When_in_state_base : TxSpecificationBase
    {
        public TxTimerStub RetransitTimer { get; set; }
        public TxTimerStub TimeOutTimer { get; set; }
        public TxTimerStub CompletedEndTimer { get; set; }

        protected When_in_state_base():base()
        {
            var tf = new TimerFactoryStub();
            tf.CreateNonInviteCtxRetransmitTimerInterceptor = CreateNonInviteCtxRetransmitTimer;
            tf.CreateNonInviteCtxEndCompletedTimerInterceptor = CreateNonInviteCtxCompletedEndTimer;
            tf.CreateNonInviteCtxTimeOutTimerInterceptor = CreateNonInviteCtxTimeOutTimer;
            TimerFactory = tf;
        }

        protected ITimer CreateNonInviteCtxRetransmitTimer(Action action)
        {
            RetransitTimer = new TxTimerStub(action, int.MaxValue, false, () => { });
            return RetransitTimer;
        }

        protected ITimer CreateNonInviteCtxCompletedEndTimer(Action action)
        {
            CompletedEndTimer = new TxTimerStub(action, int.MaxValue, false, () => { });
            return CompletedEndTimer;
        }

        protected ITimer CreateNonInviteCtxTimeOutTimer(Action action)
        {
            TimeOutTimer = new TxTimerStub(action, int.MaxValue, false, () => { });
            return TimeOutTimer;
        }
    }
}