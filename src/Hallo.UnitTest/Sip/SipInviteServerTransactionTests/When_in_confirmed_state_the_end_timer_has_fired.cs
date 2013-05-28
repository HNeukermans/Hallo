using System;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public class When_in_confirmed_state_the_end_timer_has_fired : When_in_state_base
    {
        private readonly AutoResetEvent _waitHandle = new AutoResetEvent(false);
        
        public When_in_confirmed_state_the_end_timer_has_fired()
            : base()
        {
            var tf = new TimerFactoryStubBuilder();
            tf.WithInviteStxEndConfirmedTimerInterceptor((a) => new TxTimerStub(a, 200, false, AfterCallBack));
            TimerFactory = tf.Build();
        }

        protected override void GivenOverride()
        {
            Stx.Start();
            var non200FinalResponse = CreateFinalResponse(302, "Moved Temporarily");
            Stx.SendResponse(non200FinalResponse);
            var ackRequest = new SipRequestBuilder().WithRequestLine(
                new SipRequestLineBuilder().WithMethod(SipMethods.Ack).Build()).Build();
            Stx.ProcessRequest(new SipRequestEventBuilder().WithRequest(ackRequest).Build());
            Stx.State.Should().Be(SipInviteServerTransaction.ConfirmedState); /*required assertion*/
        }

        private void AfterCallBack()
        {
            _waitHandle.Set();
        }

        protected override void When()
        {
            _waitHandle.WaitOne(TimeSpan.FromSeconds(5));
        }
        
        [Test]
        public void Expect_the_Tx_to_be_removed_from_the_TxTable()
        {
            TxTable.ContainsKey(Stx.GetId()).Should().BeFalse();
        }


    }
}