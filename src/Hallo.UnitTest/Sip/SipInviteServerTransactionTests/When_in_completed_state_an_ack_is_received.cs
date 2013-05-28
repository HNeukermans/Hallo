using System.Threading;
using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.UnitTest.Builders;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public class When_in_completed_state_an_ack_is_received : When_in_completed_state_base
    {
        private readonly AutoResetEvent _waitHandle = new AutoResetEvent(false);

        public When_in_completed_state_an_ack_is_received()
            : base()
        {
            
        }


        protected override void When()
        {
            var ackRequest = new SipRequestBuilder()
                .WithRequestLine
                (
                    new SipRequestLineBuilder().WithMethod(SipMethods.Ack).Build()
                )
                .Build();

            Stx.ProcessRequest(new SipRequestEventBuilder().WithRequest(ackRequest).Build());
        }

        [Test]
        public void Expect_the_tx_to_be_transitioned_to_confirmed_state()
        {
            Stx.State.Should().Be(SipInviteServerTransaction.ConfirmedState);
        }




    }
}