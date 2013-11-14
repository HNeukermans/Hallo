using System;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sip.Stack;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Stubs;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientTransactionTests
{
    [TestFixture]
    public class When_in_calling_state_the_timeout_timer_has_fired : TxSpecificationBase
    {
        private AutoResetEvent _timeOutWaitHandle = new AutoResetEvent(false);
        private string _txId;
        public TxTimerStub TimeOutTimer { get; set; }

        public When_in_calling_state_the_timeout_timer_has_fired()
        {
            var tf = new TimerFactoryStubBuilder()
                .WithInviteCtxTimeOutTimerInterceptor(OnCreateTimeOutTimerStub).Build();
            TimerFactory = tf;
        }

        private ITimer OnCreateTimeOutTimerStub(Action a)
        {
            TimeOutTimer = new TxTimerStub(a, 2000, false, AftertimeOut);
            return TimeOutTimer;
        }
        
        private void AftertimeOut()
        {
            _timeOutWaitHandle.Set();
        }

        protected override void GivenOverride()
        {
            _txId = Ctx.GetId();
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

        [Test]
        public void Expect_the_TimeOutTimer_to_be_Disposed()
        {
            TimeOutTimer.IsDisposed.Should().BeTrue();
        }

        [Test]
        public void Expect_ProcessTimeOut_to_be_invoked()
        {
            ListenerMock.Verify((l) => l.ProcessTimeOut(It.IsAny<SipTimeOutEvent>()), Times.Exactly(1));
        }

    }

    
}