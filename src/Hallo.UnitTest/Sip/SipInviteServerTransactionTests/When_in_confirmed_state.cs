using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public class When_in_confirmed_state : When_in_state_base
    {
        private readonly AutoResetEvent _waitHandle = new AutoResetEvent(false);
       

        public When_in_confirmed_state()
            : base()
        {
            var tf = new TimerFactoryStubBuilder();
            //tf.WithInviteStxEndConfirmedTimerInterceptor((a) => new TxTimerStub(a, 200, false, AfterCallBack));
            TimerFactory = tf.Build();
        }

        protected override void GivenOverride()
        {
           // ((SipAbstractServerTransaction) Stx).Initialize();
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

        //protected override void When()
        //{
        //    _waitHandle.WaitOne(TimeSpan.FromSeconds(5));
        //}

        [Test]
        public void Expect_the_LatestResponse_to_be_non_200Final()
        {
            Stx.LatestResponse.StatusLine.StatusCode.Should().BeInRange(300, 700);
        }

        [Test]
        public void Expect_the_SendTrying_to_be_disposed()
        {
            Stx.SendTryingTimer.As<TxTimerStub>().IsDisposed.Should().BeTrue();
        }

        [Test]
        public void Expect_the_EndCompletedTimer_to_be_disposed()
        {
            Stx.EndCompletedTimer.As<TxTimerStub>().IsDisposed.Should().BeTrue();
        }

        [Test]
        public void Expect_the_EndConfirmedTimer_to_be_started()
        {
            Stx.EndConfirmedTimer.As<TxTimerStub>().IsStarted.Should().BeTrue();
        }

        [Test]
        public void Expect_retransmit_timer_to_be_disposed()
        {
            Stx.ReTransmitNonx200FinalResponseTimer.As<TxTimerStub>().IsDisposed.Should().BeTrue();
        }


    }
}