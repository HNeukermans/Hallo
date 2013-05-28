using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientTransactionTests
{
    
    public class When_in_calling_state_a_2xx_final_response_is_received : TxSpecificationBase
    {
        public When_in_calling_state_a_2xx_final_response_is_received():base()
        {
        }

        protected override void GivenOverride()
        {
            Ctx.SendRequest();
        }

        protected override void When()
        {
            var r = base.CreateFinalResponseEvent();

            Ctx.ProcessResponse(r);
        }

        [Test]
        public void Expect_the_Ctx_to_be_removed()
        {
            TxTable.Count.Should().Be(0);
        }

        [Test]
        public void Expect_ProcessResponse_to_be_invoked()
        {
            ListenerMock.Verify((l) => l.ProcessResponse(It.IsAny<SipResponseEvent>()), Times.Exactly(1));
        }

        [Test]
        public void Expect_the_ack_request_not_to_be_sent()
        {
            MessageSenderMock.Verify((ms) => ms.SendRequest(It.Is<SipRequest>(r => r.RequestLine.Method == SipMethods.Ack)), Times.Never());
        }

    }
}