using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public class When_in_proceeding_state_a_nonx200final_response_is_send : TxSpecificationBase
    {
        private SipResponse _non200FinalResponse;

        public When_in_proceeding_state_a_nonx200final_response_is_send()
            : base()
        {

        }

        protected override void GivenOverride()
        {
            //((SipAbstractServerTransaction) Stx).Initialize();
        }

        protected override void When()
        {
            _non200FinalResponse = CreateFinalResponse(302, "Moved Temporarily");
            Stx.SendResponse(_non200FinalResponse);
        }

        [Test]
        public void Expect_the_stx_to_transition_to_completed_state()
        {
            Stx.State.Should().Be(SipInviteServerTransaction.CompletedState);
        }

        [Test]
        public void Expect_the_LatestResponse_to_be_the_nonx200_final_response()
        {
            Stx.LatestResponse.Should().Be(_non200FinalResponse);
        }

        [Test]
        public void Expect_the_most_recent_response_to_be_send()
        {
            Sender.Verify((r) => r.SendResponse(_non200FinalResponse), Times.Exactly(1));
        }

        [Test]
        public void Expect_the_the_transaction_to_be_in_the_TxTable()
        {
            TxTable.ContainsKey(Stx.GetId()).Should().BeTrue();
        }

    }
}