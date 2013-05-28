using FluentAssertions;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientTransactionTests
{
    [TestFixture("Transactions")]
    public class When_in_proceeding_state_a_provisional_response_is_received : SipInviteClientTransactionTests.TxSpecificationBase
    {
        protected override void GivenOverride()
        {
            Ctx.SendRequest(); /*force it to go into calling state*/
            Ctx.ProcessResponse(CreateProvisionalResponseEvent());
            Ctx.State.Should().Be(SipInviteClientTransaction.ProceedingState); /*required assertion*/
        }

        protected override void When()
        {
            var r = base.CreateProvisionalResponseEvent();

            Ctx.ProcessResponse(r);
        }

        [Test]
        public void Expect_the_Ctx_to_remain_in_proceeding_state()
        {
            Ctx.State.Should().Be(SipInviteClientTransaction.ProceedingState);
        }

        [Test]
        public void Expect_ProcessResponse_to_be_invoked()
        {
            ListenerMock.Verify((l) => l.ProcessResponse(It.IsAny<SipResponseEvent>()), Times.Exactly(2));
        }

    }
}