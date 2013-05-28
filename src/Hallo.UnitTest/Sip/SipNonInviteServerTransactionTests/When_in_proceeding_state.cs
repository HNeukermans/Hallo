using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteServerTransactionTests
{
    public class When_in_proceeding_state : TxSpecificationBase
    {
        private SipResponse _response;

        public When_in_proceeding_state()
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
           
        }

        [Test]
        public void Expect_the_LatestResponse_to_be_the_provisional_response()
        {
            Stx.LatestResponse.Should().Be(_response);
        }

        [Test]
        public void Expect_the_EndCompletedTimer_not_to_be_started()
        {
            EndCompletedTimer.IsStarted.Should().BeFalse();
        }
    }
}