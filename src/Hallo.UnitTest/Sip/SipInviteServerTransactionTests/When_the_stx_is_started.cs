using FluentAssertions;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteServer;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public class When_the_stx_is_started : TxSpecificationBase
    {
        public When_the_stx_is_started()
            : base()
        {

        }

        protected override void When()
        {
            ((SipAbstractServerTransaction) Stx).Initialize();
        }

        [Test]
        public void Expect_the_Stx_to_be_in_proceeding_state()
        {
            Stx.State.Should().Be(SipInviteServerTransaction.ProceedingState);
        }

        [Test]
        public void Expect_TxTable_to_contain_the_Tx_id()
        {
            TxTable.ContainsKey(Stx.GetId()).Should().BeTrue();
        }
    }
}