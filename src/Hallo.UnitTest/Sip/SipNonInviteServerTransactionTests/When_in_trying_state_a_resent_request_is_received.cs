using Hallo.Sip;
using Hallo.UnitTest.Builders;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteServerTransactionTests
{
    [TestFixture]
    public class When_in_trying_state_a_resent_request_is_received : TxSpecificationBase
    {
        public When_in_trying_state_a_resent_request_is_received()
            : base()
        {

        }

        protected override void GivenOverride()
        {
        }

        protected override void When()
        {
            Stx.ProcessRequest(new SipRequestEventBuilder().Build());
        }

        [Test]
        public void Expect_the_stx_to_transition_to_completed_state()
        {
            Sender.Verify((r) => r.SendResponse(It.IsAny<SipResponse>()), Times.Never());
        }

    }
}