using System;
using Hallo.Sip.Stack;
using Hallo.UnitTest.Sip.SipNonInviteClientTransactionTests;
using Hallo.UnitTest.Stubs;
using Hallo.Component;

namespace Hallo.UnitTest.Sip.SipInviteClientTransactionTests
{
    public abstract class When_in_state_base : TxSpecificationBase
    {
        public TxTimerStub RetransitTimer { get; set; }
        public TxTimerStub TimeOutTimer { get; set; }
        public TxTimerStub CompletedEndTimer { get; set; }

        protected When_in_state_base():base()
        {
            var tf = new TimerFactoryStub();
            tf.CreateInviteCtxRetransmitTimerInterceptor = CreateInviteCtxRetransmitTimer;
            tf.CreateInviteCtxEndCompletedTimerInterceptor = CreateInviteCtxCompletedEndTimer;
            tf.CreateInviteCtxTimeOutTimerInterceptor = CreateInviteCtxTimeOutTimer;
            TimerFactory = tf;
        }

        protected ITimer CreateInviteCtxRetransmitTimer(Action action)
        {
            RetransitTimer = new TxTimerStub(action, int.MaxValue, false, () => { });
            return RetransitTimer;
        }

        protected ITimer CreateInviteCtxCompletedEndTimer(Action action)
        {
            CompletedEndTimer = new TxTimerStub(action, int.MaxValue, false, () => { });
            return CompletedEndTimer;
        }

        protected ITimer CreateInviteCtxTimeOutTimer(Action action)
        {
            TimeOutTimer = new TxTimerStub(action, int.MaxValue, false, () => { });
            return TimeOutTimer;
        }
    }
}