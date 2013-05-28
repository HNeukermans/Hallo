using System;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Stubs;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public class When_in_completed_state_the_retransit_timer_fired_1_times : When_in_completed_state_base
    {
        private readonly AutoResetEvent _waitHandle = new AutoResetEvent(false);
        private int _originalInterval;

        public When_in_completed_state_the_retransit_timer_fired_1_times()
            : base()
        {
            var tf = new TimerFactoryStubBuilder();
            _originalInterval = 500;
            tf.WithInviteStxRetransmitTimerInterceptor((a) => new TxTimerStub(a, _originalInterval, false, AfterRetransmitFired));
            TimerFactory = tf.Build();
        }

        private void AfterRetransmitFired()
        {
            _waitHandle.Set();
        }

        protected override void When()
        {
            _waitHandle.WaitOne(TimeSpan.FromSeconds(5));
        }

        [Test]
        public void Expect_the_LatestResponse_not_to_be_null()
        {
            Stx.LatestResponse.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_final_response_to_be_resent()
        {
            Sender.Verify(s => s.SendResponse(_non200FinalResponse), Times.Exactly(2));
        }

        [Test]
        public void Expect_the_RetransitTimer_interval_to_be_doubled()
        {
            Stx.ReTransmitNonx200FinalResponseTimer.Interval.Should().Be(_originalInterval* 2);
        }

    }
}