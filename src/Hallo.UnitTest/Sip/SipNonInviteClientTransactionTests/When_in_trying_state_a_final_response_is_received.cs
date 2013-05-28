using FluentAssertions;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using Hallo.UnitTest.Builders;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteClientTransactionTests
{
    [TestFixture("Transactions")]
    public class When_in_trying_state_a_final_response_is_received : TxSpecificationBase
    {
        protected override void GivenOverride()
        {
            Ctx.SendRequest();
        }

        protected override void When()
        {
            var r = CreateFinalResponseEvent();

            Ctx.ProcessResponse(r);
        }

        [Test]
        public void Expect_the_Ctx_to_transition_to_the_CompletedState()
        {
            Ctx.State.Should().Be(SipNonInviteClientTransaction.CompletedState);
        }

    }
}