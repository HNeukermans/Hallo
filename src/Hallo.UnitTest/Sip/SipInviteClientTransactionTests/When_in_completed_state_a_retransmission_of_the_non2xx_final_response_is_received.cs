using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientTransactionTests
{
    [TestFixture]
    public class When_in_completed_state_a_retransmission_of_the_non2xx_final_response_is_received : SipInviteClientTransactionTests.TxSpecificationBase
    {
        private SipResponseEvent response;

        public When_in_completed_state_a_retransmission_of_the_non2xx_final_response_is_received()
            : base()
        {
        }

        protected override void GivenOverride()
        {
            Ctx.SendRequest(); /*force it to go into calling state*/
            response = CreateFinalResponseEvent(500, "Internal Server error");
            Ctx.ProcessResponse(response);
            Ctx.State.Should().Be(SipInviteClientTransaction.CompletedState); /*required assertion*/
        }

        protected override void When()
        {
            Ctx.ProcessResponse(response);
        }

        [Test]
        public void Expect_ProcessResponse_not_to_be_invoked()
        {
            //its invoked 1 time to go into completed state.
            ListenerMock.Verify((l) => l.ProcessResponse(It.IsAny<SipResponseEvent>()), Times.Exactly(1));
        }
        
        [Test]
        public void Expect_the_ack_request_to_be_resent()
        {
            MessageSenderMock.Verify((ms) => ms.SendRequest(It.Is<SipRequest>(r => r.RequestLine.Method == SipMethods.Ack)), Times.Exactly(2));
        }



    }
}