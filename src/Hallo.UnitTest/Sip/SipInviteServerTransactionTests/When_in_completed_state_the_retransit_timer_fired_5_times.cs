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
    public class When_in_completed_state_the_retransit_timer_fired_5_times : When_in_completed_state_base
    {
        private readonly AutoResetEvent _waitHandle = new AutoResetEvent(false);
        private int _counter = 5;
        private int _originalInterval;

        public When_in_completed_state_the_retransit_timer_fired_5_times()
            : base()
        {
            var tf = new TimerFactoryStubBuilder();
            _originalInterval = 500;
            tf.WithInviteStxRetransmitTimerInterceptor((a) => new TxTimerStub(a, _originalInterval, false, AfterRetransmitFired));
            TimerFactory = tf.Build();
        }

        private void AfterRetransmitFired()
        {
            _counter--;
            if (_counter == 0)
            {
                _waitHandle.Set();
            }
        }

        protected override void When()
        {
            _waitHandle.WaitOne(TimeSpan.FromSeconds(30));
        }

        [Test]
        public void Expect_the_final_response_to_be_resent_5_times()
        {
            Sender.Verify(s => s.SendResponse(_non200FinalResponse), Times.Exactly(6));
        }

        [Test]
        public void Expect_the_RetransitTimer_interval_to_be_T2()
        {
            Stx.ReTransmitNonx200FinalResponseTimer.Interval.Should().Be(SipConstants.T2);
        }

    }
}