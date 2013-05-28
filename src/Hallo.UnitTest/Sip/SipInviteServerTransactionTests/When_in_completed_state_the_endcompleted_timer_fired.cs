using System;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sip.Stack;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Stubs;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public class When_in_completed_state_the_endcompleted_timer_fired : When_in_completed_state_base
    {
        private readonly AutoResetEvent _waitHandle = new AutoResetEvent(false);
        
        public When_in_completed_state_the_endcompleted_timer_fired()
            : base()
        {
            var tf = new TimerFactoryStubBuilder();
            tf.WithInviteStxRetransmitTimerInterceptor((a) => new TxTimerStub(a, 500, false, ()=> { }));
            tf.WithInviteStxEndCompletedInterceptor((a) => new TxTimerStub(a, 64 * SipConstants.T1, false, AfterEndCompleted));
            TimerFactory = tf.Build();
        }

        private void AfterEndCompleted()
        {
            _waitHandle.Set();
        }

        protected override void When()
        {
            _waitHandle.WaitOne(TimeSpan.FromSeconds(35));
        }

        /// <summary>
        ///1nd 2xx - 0.5 sec
        ///2rd 2xx - 1.5 sec
        ///3th 2xx - 3.5 sec
        ///4th 2xx - 7.5 sec
        ///5th 2xx - 11.5 sec
        ///6th 2xx - 15.5 sec
        ///7th 2xx - 19.5 sec
        ///8th 2xx - 23.5 sec
        ///9th 2xx - 27.5 sec
        ///10th 2xx - 31.5 sec
        /// </summary>
        [Test]
        public void Expect_the_final_response_to_be_resent_10_times()
        {
            Sender.Verify(s => s.SendResponse(_non200FinalResponse), Times.Exactly(11));
        }

        [Test]
        public void Expect_the_RetransitTimer_interval_to_be_T4()
        {
            Stx.ReTransmitNonx200FinalResponseTimer.Interval.Should().Be(SipConstants.T2);
        }

        [Test]
        public void Expect_the_ProcessTimeOut_to_be_fired()
        {
            Listener.Verify(s => s.ProcessTimeOut(It.IsAny<SipTimeOutEvent>()), Times.Exactly(1));
        }

        [Test]
        public void Expect_the_Tx_to_be_removed_from_the_TxTable()
        {
            TxTable.ContainsKey(Stx.GetId()).Should().BeFalse();
        }

    }
}