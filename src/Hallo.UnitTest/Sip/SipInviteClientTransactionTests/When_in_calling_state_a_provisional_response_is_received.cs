using FluentAssertions;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientTransactionTests
{
    [TestFixture]
    public class When_in_calling_state_a_provisional_response_is_received : SipInviteClientTransactionTests.TxSpecificationBase
    {
        protected override void GivenOverride()
        {
            Ctx.SendRequest();
        }

        protected override void When()
        {
            var r = base.CreateProvisionalResponseEvent();

            Ctx.ProcessResponse(r);
        }

        [Test]
        public void Expect_the_Ctx_to_transition_to_proceeding_state()
        {
            Ctx.State.Should().Be(SipInviteClientTransaction.ProceedingState);
        }

        [Test]
        public void Expect_ProcessResponse_to_be_invoked()
        {
            ListenerMock.Verify((l) => l.ProcessResponse(It.IsAny<SipResponseEvent>()), Times.Exactly(1));
        }

    }
}