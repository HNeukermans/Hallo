using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteServerTransactionTests
{
    public class When_in_proceeding_state_a_final_response_is_send : TxSpecificationBase
    {
        private SipResponse _response;
        private SipResponse _finalResponse;

        public When_in_proceeding_state_a_final_response_is_send()
            : base()
        {

        }

        protected override void GivenOverride()
        {
            _response = CreateProvisionalResponse();
            Stx.SendResponse(_response); /*force it to go into state*/
            Stx.State.Should().Be(SipNonInviteServerTransaction.ProceedingState); /*required assertion*/
        }

        protected override void When()
        {
            _finalResponse = CreateFinalResponse();
            Stx.SendResponse(_finalResponse);
        }

        [Test]
        public void Expect_the_stx_to_transition_to_completed_state()
        {
            Stx.State.Should().Be(SipNonInviteServerTransaction.CompletedState);
        }

        [Test]
        public void Expect_the_LatestResponse_to_be_the_final_response()
        {
            Stx.LatestResponse.Should().Be(_finalResponse);
        }

        [Test]
        public void Expect_the_most_recent_response_to_be_send()
        {
            Sender.Verify((r) => r.SendResponse(It.IsAny<SipResponse>()), Times.Exactly(2));
        }
    }
}
