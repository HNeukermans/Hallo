using FluentAssertions;
using Hallo.Sip.Stack.Transactions.InviteClient;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientTransactionTests
{
    public class When_in_completed_state : When_in_state_base
    {
        public When_in_completed_state()
            : base()
        {

        }

        protected override void GivenOverride()
        {
        }

        protected override void When()
        {
            Ctx.SendRequest(); /*force it to go into calling state*/
            Ctx.ProcessResponse(CreateFinalResponseEvent(500, "Internal Server error"));
            Ctx.State.Should().Be(SipInviteClientTransaction.CompletedState); /*required assertion*/
        }

        [Test]
        public void Expect_the_Retransmit_timer_to_be_disposed()
        {
            RetransitTimer.IsDisposed.Should().BeTrue();
        }

        [Test]
        public void Expect_the_TimeOut_timer_to_be_disposed()
        {
            TimeOutTimer.IsDisposed.Should().BeTrue();
        }

        [Test]
        public void Expect_the_CompletedEnd_timer_to_be_started()
        {
            CompletedEndTimer.IsStarted.Should().BeTrue();
        }

    }
}