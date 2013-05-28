using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using Hallo.UnitTest.Builders;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteServerTransactionTests
{
    public class When_in_completed_state_a_resent_request_is_received : TxSpecificationBase
    {
        private SipResponse _response;

        public When_in_completed_state_a_resent_request_is_received()
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
            Stx.ProcessRequest(new SipRequestEventBuilder().Build());
        }

        [Test]
        public void Expect_the_stx_to_respond_with_the_final_response()
        {
            Sender.Verify((r) => r.SendResponse(It.Is<SipResponse>(response => response == _response)));
        }
    }
}