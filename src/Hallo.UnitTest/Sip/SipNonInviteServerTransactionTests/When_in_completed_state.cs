using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteServerTransactionTests
{
    public class When_in_completed_state : TxSpecificationBase
    {
        private SipResponse _response;

        public When_in_completed_state()
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

        }

        [Test]
        public void Expect_the_LatestResponse_to_be_the_final_response()
        {
            Stx.LatestResponse.Should().Be(_response);
        }

        [Test]
        public void Expect_the_EndCompletedTimer_to_be_started()
        {
            EndCompletedTimer.IsStarted.Should().BeTrue();
        }
    }
}