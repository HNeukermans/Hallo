using FluentAssertions;
using Hallo.Component;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public class When_in_completed_state : When_in_completed_state_base
    {


        public When_in_completed_state()
            : base()
        {
            var tf = new TimerFactoryStubBuilder().Build();
            TimerFactory = tf;
        }

        protected override void GivenOverride()
        {
            ((SipAbstractServerTransaction) Stx).Initialize();
            _non200FinalResponse = CreateFinalResponse(302, "Moved Temporarily");
            Stx.SendResponse(_non200FinalResponse);
            Stx.State.Should().Be(SipInviteServerTransaction.CompletedState); /*required assertion*/
        }

        [Test]
        public void Expect_the_LatestResponse_to_be_non_200Final()
        {
            Stx.LatestResponse.StatusLine.StatusCode.Should().BeInRange(300,700);
        }

        [Test]
        public void Expect_the_SendTrying_to_be_disposed()
        {
            Stx.SendTryingTimer.As<TxTimerStub>().IsDisposed.Should().BeTrue();
        }

        [Test]
        public void Expect_the_EndCompletedTimer_to_be_started()
        {
            Stx.EndCompletedTimer.As<TxTimerStub>().IsStarted.Should().BeTrue();
        }

        [Test]
        public void Expect_the_EndConfirmedTimer_not_to_be_started()
        {
            Stx.EndConfirmedTimer.As<TxTimerStub>().IsStarted.Should().BeFalse();
        }

        [Test]
        public void Expect_retransmit_timer_to_be_started()
        {
            Stx.ReTransmitNonx200FinalResponseTimer.As<TxTimerStub>().IsStarted.Should().BeTrue();
        }


    }
}