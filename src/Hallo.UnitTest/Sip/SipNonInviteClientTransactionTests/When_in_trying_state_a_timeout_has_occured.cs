using System;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;
using Hallo.UnitTest.Builders;

namespace Hallo.UnitTest.Sip.SipNonInviteClientTransactionTests
{
    [TestFixture]
    public class When_in_trying_state_a_timeout_has_occured : TxSpecificationBase
    {
        private readonly AutoResetEvent _timeOutWaitHandle;
        private string _txId;
        public TxTimerStub TimeOutTimer { get; set; }

        public When_in_trying_state_a_timeout_has_occured()
        {
            _timeOutWaitHandle = new AutoResetEvent(false);

            var tf = new TimerFactoryStub();
            tf.CreateNonInviteCtxRetransmitTimerInterceptor = (a) => new TxTimerBuilder().WithCallback(a).Build();
            tf.CreateNonInviteCtxEndCompletedTimerInterceptor = (a) => new TxTimerBuilder().WithCallback(a).Build();
            tf.CreateNonInviteCtxTimeOutTimerInterceptor = (a) => new TxTimerStub(a, 1000, false, AfterCallBack);
            TimerFactory = tf;
        }

        private ITimer CreateNonInviteCtxTimeOutTimer(Action action)
        {
            TimeOutTimer = new TxTimerStub(action, int.MaxValue, false, () => { });
            return TimeOutTimer;
        }

        private void AfterCallBack()
        {
            _timeOutWaitHandle.Set();
        }

        protected override void GivenOverride()
        {
            _txId = Ctx.GetId();
            /*go to trying state*/
            Ctx.SendRequest();
        }

        protected override void When()
        {
            /*wait for the timeout to happen*/
            _timeOutWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
        }

        [Test]
        public void Expect_the_TransactionTable_to_be_empty()
        {
            TxTable.Keys.Contains(_txId).Should().BeFalse();
        }
    }
}