using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.UnitTest.Builders;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public class When_in_proceeding_state_a_resent_request_is_received : TxSpecificationBase
    {
        private SipResponse _provisionalResponse;

        public When_in_proceeding_state_a_resent_request_is_received()
            : base()
        {

        }

        protected override void GivenOverride()
        {
            //((SipAbstractServerTransaction) Stx).Initialize();
            _provisionalResponse = CreateProvisionalResponse();
            Stx.SendResponse(_provisionalResponse);
            Stx.State.Should().Be(SipInviteServerTransaction.ProceedingState); /*assertion*/
        }

        protected override void When()
        {
            Stx.ProcessRequest(new SipRequestEventBuilder().WithRequest(Request).Build());
        }

        [Test]
        public void Expect_the_LatestResponse_to_be_provisional_response()
        {
            Stx.LatestResponse.Should().Be(_provisionalResponse);
        }

        [Test]
        public void Expect_the_most_recent_response_to_be_resend()
        {
            Sender.Verify((r) => r.SendResponse(_provisionalResponse), Times.Exactly(2));
        }

    }
}