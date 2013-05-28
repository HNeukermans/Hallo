using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteServerTransactionTests
{
    public class When_in_trying_state_a_provisional_response_is_send : TxSpecificationBase
    {
        private SipResponse _provisionalResponse;

        public When_in_trying_state_a_provisional_response_is_send()
            : base()
        {

        }

        protected override void GivenOverride()
        {
        }

        protected override void When()
        {
            _provisionalResponse = CreateProvisionalResponse();
            Stx.SendResponse(_provisionalResponse);
        }

        [Test]
        public void Expect_the_stx_to_transition_to_proceeding_state()
        {
            Stx.State.Should().Be(SipNonInviteServerTransaction.ProceedingState);
        }

        [Test]
        public void Expect_the_LatestResponse_to_be_the_final_response()
        {
            Stx.LatestResponse.Should().Be(_provisionalResponse);
        }


        [Test]
        public void Expect_the_most_recent_response_to_be_send()
        {
            Sender.Verify((r) => r.SendResponse(It.IsAny<SipResponse>()), Times.Exactly(1));
        }
    }
}