using FluentAssertions;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteClientTransactionTests
{
   [TestFixture]
    public class When_in_completed_state_a_provisional_response_is_received_ : TxSpecificationBase
    {
        protected override void GivenOverride()
        {
            Ctx.SendRequest();
            var r = CreateFinalResponseEvent();
            Ctx.ProcessResponse(r); /*force it to go into completed state*/
            Ctx.State.Should().Be(SipNonInviteClientTransaction.CompletedState); /*required assertion*/
        }

        protected override void When()
        {
            var r = CreateProvisionalResponseEvent();
            Ctx.ProcessResponse(r);
        }

        [Test]
        public void Expect_the_Ctx_to_stay_in_completed_state()
        {
            Ctx.State.Should().Be(SipNonInviteClientTransaction.CompletedState);
        }
    }
}