using System.Threading;
using FluentAssertions;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientTransactionTests
{
    public class When_in_calling_state : When_in_state_base
    {
        public When_in_calling_state()
            : base()
        {
            
        }

        protected override void GivenOverride()
        {
        }

        protected override void When()
        {
            Ctx.SendRequest(); /*force it to go into calling state*/
            Ctx.State.Should().Be(SipInviteClientTransaction.CallingState); /*required assertion*/
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