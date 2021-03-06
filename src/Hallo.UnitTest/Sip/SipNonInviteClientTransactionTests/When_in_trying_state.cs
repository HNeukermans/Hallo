using System.Threading;
using FluentAssertions;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteClientTransactionTests
{
    public class When_in_trying_state : When_in_state_base
    {
        private AutoResetEvent _timeOutWaitHandle = new AutoResetEvent(false);
        private string _txId;

        public When_in_trying_state()
            : base()
        {
            
        }

        protected override void GivenOverride()
        {
        }

        protected override void When()
        {
            Ctx.SendRequest(); /*force it to go into trying state*/
            Ctx.State.Should().Be(SipNonInviteClientTransaction.TryingState); /*required assertion*/
        }

        [Test]
        public void Expect_the_Retransmit_timer_to_be_started()
        {
            RetransitTimer.IsStarted.Should().BeTrue();
        }

        [Test]
        public void Expect_the_TimeOut_timer_to_be_started()
        {
            TimeOutTimer.IsStarted.Should().BeTrue();
        }

        [Test]
        public void Expect_the_CompletedEnd_timer_not_to_be_started()
        {
            CompletedEndTimer.IsStarted.Should().BeFalse();
        }

    }
}