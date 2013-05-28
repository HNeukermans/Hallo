using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public class When_in_proceeding_state_a_provisional_response_is_send : TxSpecificationBase
    {
        private SipResponse _provisionalResponse;

        public When_in_proceeding_state_a_provisional_response_is_send()
            : base()
        {

        }

        protected override void GivenOverride()
        {
            Stx.Start();
        }

        protected override void When()
        {
            _provisionalResponse = CreateProvisionalResponse();
            Stx.SendResponse(_provisionalResponse);
        }

        [Test]
        public void Expect_the_stx_to_remain_in_proceeding_state()
        {
            Stx.State.Should().Be(SipInviteServerTransaction.ProceedingState);
        }

        [Test]
        public void Expect_the_LatestResponse_to_be_the_provisional_response()
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