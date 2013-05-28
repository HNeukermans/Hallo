using System.Threading;
using FluentAssertions;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteClientTransactionTests
{
    public class When_in_proceeding_state : When_in_state_base
    {
        private AutoResetEvent _timeOutWaitHandle = new AutoResetEvent(false);
        private string _txId;

        public When_in_proceeding_state()
            : base()
        {

        }

        protected override void GivenOverride()
        {
        }

        protected override void When()
        {
            Ctx.SendRequest(); /*force it to go into state*/
            Ctx.ProcessResponse(CreateProvisionalResponseEvent());
            Ctx.State.Should().Be(SipNonInviteClientTransaction.ProceedingState); /*required assertion*/
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