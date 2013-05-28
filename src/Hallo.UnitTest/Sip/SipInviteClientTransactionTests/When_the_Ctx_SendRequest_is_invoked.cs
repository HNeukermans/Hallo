using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientTransactionTests
{
    [TestFixture]
    public class When_the_Ctx_SendRequest_is_invoked : TxSpecificationBase
    {
        protected override void GivenOverride()
        {
            
        }

        protected override void When()
        {
            Ctx.SendRequest();
        }

        [Test]
        public void Expect_the_Ctx_to_have_the_calling_state()
        {
            Ctx.State.Should().Be(SipInviteClientTransaction.CallingState);
        }

        [Test]
        public void Expect_the_TxTable_to_contain_the_Id()
        {
            TxTable.ContainsKey(Ctx.GetId()).Should().BeTrue();
        }

        [Test]
        public void Expect_the_Request_to_be_sent()
        {
            MessageSenderMock.Verify((ms) => ms.SendRequest(It.IsAny<SipRequest>()), Times.Once());
        }
    }

}