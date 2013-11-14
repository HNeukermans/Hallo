using FluentAssertions;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteClientTransactionTests
{
    [TestFixture()]
    public class When_in_proceeding_state_a_provisional_response_is_received_ : TxSpecificationBase
    {
        protected override void GivenOverride()
        {
            Ctx.SendRequest();
            var r = CreateProvisionalResponseEvent();
            Ctx.ProcessResponse(r); /*force it to go into proceeding state*/
            Ctx.State.Should().Be(SipNonInviteClientTransaction.ProceedingState); /*required assertion*/
        }

        protected override void When()
        {
            var r = CreateProvisionalResponseEvent();
            Ctx.ProcessResponse(r);
        }

        [Test]
        public void Expect_the_Ctx_to_stay_in_proceeding_state()
        {
            Ctx.State.Should().Be(SipNonInviteClientTransaction.ProceedingState);
        }
    }
}