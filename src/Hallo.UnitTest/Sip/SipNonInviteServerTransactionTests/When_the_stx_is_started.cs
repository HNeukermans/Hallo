using FluentAssertions;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteServerTransactionTests
{
    public class When_the_stx_is_started : TxSpecificationBase
    {
        public When_the_stx_is_started()
            : base()
        {
            //Stx.Start();
        }

        [Test]
        public void Expect_the_Stx_to_be_in_trying_state()
        {
            Stx.State.Should().Be(SipNonInviteServerTransaction.TryingState);
        }

        [Test]
        public void Expect_TxTable_to_contain_the_Tx_id()
        {
            TxTable.ContainsKey(Stx.GetId()).Should().BeTrue();
        }
    }
}