using FluentAssertions;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteServerTransactionTests
{
    public class When_the_stx_is_instantiated : TxSpecificationBase
    {
        public When_the_stx_is_instantiated()
            : base()
        {
            //Stx.Start();
        }

        protected override void Given()
        {
            Stx = new SipNonInviteServerTransaction(TxTable, Request, Listener.Object, Sender.Object, TimerFactory);
        }

        [Test]
        public void Expect_the_Stx_Statz_to_be_null()
        {
            Stx.State.Should().BeNull();
        }

        [Test]
        public void Expect_TxTable_to_be_empty()
        {
            TxTable.ContainsKey(Stx.GetId()).Should().BeFalse();
        }
    }
}