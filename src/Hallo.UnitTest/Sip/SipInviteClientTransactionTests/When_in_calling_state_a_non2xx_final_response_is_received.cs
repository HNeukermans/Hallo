using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientTransactionTests
{
    [TestFixture("Transactions")]
    public class When_in_calling_state_a_non2xx_final_response_is_received : SipInviteClientTransactionTests.TxSpecificationBase
    {
        protected override void GivenOverride()
        {
            Ctx.SendRequest();
        }

        protected override void When()
        {
            var r = base.CreateFinalResponseEvent(301, "Moved Temporarily");

            Ctx.ProcessResponse(r);
        }

        [Test]
        public void Expect_the_Ctx_to_transition_to_completed_state()
        {
            Ctx.State.Should().Be(SipInviteClientTransaction.CompletedState);
        }

        [Test]
        public void Expect_ProcessResponse_to_be_invoked()
        {
            ListenerMock.Verify((l) => l.ProcessResponse(It.IsAny<SipResponseEvent>()), Times.Exactly(1));
        }

        [Test]
        public void Expect_the_ack_request_to_be_sent()
        {
            MessageSenderMock.Verify((ms) => ms.SendRequest(It.Is<SipRequest>(r => r.RequestLine.Method == SipMethods.Ack)), Times.Once());
        }

    }
}