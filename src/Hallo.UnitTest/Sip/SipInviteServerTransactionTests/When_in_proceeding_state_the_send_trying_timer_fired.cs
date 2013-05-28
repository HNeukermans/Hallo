using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Stubs;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public class When_in_proceeding_state_the_send_trying_timer_fired : When_in_state_base
    {
        private AutoResetEvent _waitHandle = new AutoResetEvent(false);
        
        public When_in_proceeding_state_the_send_trying_timer_fired()
            : base()
        {
            var tf = new TimerFactoryStubBuilder();
            tf.WithInviteStxSendTryingTimerInterceptor((a) => new TxTimerStub(a, 200, false, AfterSendTryingFired));
            TimerFactory = tf.Build();
        }
        
        private void AfterSendTryingFired()
        {
            _waitHandle.Set();
        }
        
        protected override void When()
        {
            Stx.Start();
            Stx.State.Should().Be(SipInviteServerTransaction.ProceedingState); /*required assertion*/
            _waitHandle.WaitOne(TimeSpan.FromSeconds(5));
        }
       
        [Test]
        public void Expect_the_LatestResponse_not_to_be_null()
        {
            Stx.LatestResponse.Should().NotBeNull();
        }

        [Test]
        public void Expect_a_trying_response_to_be_sent()
        {
            Sender.Verify(s=> s.SendResponse(It.Is<SipResponse>(r => r.StatusLine.StatusCode == 100)), Times.Exactly(1));
        }

        [Test]
        public void Expect_the_EndCompletedTimer_not_to_be_started()
        {
            Stx.EndCompletedTimer.As<TxTimerStub>().IsStarted.Should().BeFalse();
        }

        [Test]
        public void Expect_the_EndConfirmedTimer_not_to_be_started()
        {
            Stx.EndConfirmedTimer.As<TxTimerStub>().IsStarted.Should().BeFalse();
        }

        [Test]
        public void Expect_the_RetransitNonx200FinalResponseTimer_not_to_be_started()
        {
            Stx.ReTransmitNonx200FinalResponseTimer.As<TxTimerStub>().IsStarted.Should().BeFalse();
        }
        
    }
}
