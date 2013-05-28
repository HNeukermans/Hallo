using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteServerTransactionTests
{
    public class When_in_completed_state_a_final_response_is_send : TxSpecificationBase
    {
        private SipResponse _response;
        private SipResponse _mostRecentResponse;

        public When_in_completed_state_a_final_response_is_send()
            : base()
        {

        }

        protected override void GivenOverride()
        {
            _response = CreateFinalResponse();
            Stx.SendResponse(_response); /*force it to go into state*/
            Stx.State.Should().Be(SipNonInviteServerTransaction.CompletedState); /*required assertion*/
        }

        protected override void When()
        {
            _mostRecentResponse = CreateFinalResponse(202, "accepted");
            Stx.SendResponse(_mostRecentResponse);
        }
        
        [Test]
        public void Expect_the_most_recent_response_to_be_discarded()
        {
            Stx.LatestResponse.StatusLine.StatusCode.Should().Be(_response.StatusLine.StatusCode);
        }

        [Test]
        public void Expect_the_most_recent_response_not_to_be_send()
        {
            Sender.Verify((r) => r.SendResponse(It.IsAny<SipResponse>()), Times.Once());
        }
    }
}