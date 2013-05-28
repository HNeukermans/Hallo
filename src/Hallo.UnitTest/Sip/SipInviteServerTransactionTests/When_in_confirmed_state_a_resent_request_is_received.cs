using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.UnitTest.Builders;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public class When_in_confirmed_state_a_resent_request_is_received : When_in_state_base
    {
        private readonly AutoResetEvent _waitHandle = new AutoResetEvent(false);
        private SipResponse _non200FinalResponse;

        public When_in_confirmed_state_a_resent_request_is_received()
            : base()
        {
            var tf = new TimerFactoryStubBuilder();
            TimerFactory = tf.Build();
        }

        protected override void GivenOverride()
        {
            Stx.Start();
            _non200FinalResponse = CreateFinalResponse(302, "Moved Temporarily");
            Stx.SendResponse(_non200FinalResponse);
            var ackRequest = new SipRequestBuilder().WithRequestLine(
                new SipRequestLineBuilder().WithMethod(SipMethods.Ack).Build()).Build();
            Stx.ProcessRequest(new SipRequestEventBuilder().WithRequest(ackRequest).Build());
            Stx.State.Should().Be(SipInviteServerTransaction.ConfirmedState); /*required assertion*/
        }


        protected override void When()
        {
            var ackRequest = new SipRequestBuilder().WithRequestLine(
                new SipRequestLineBuilder().WithMethod(SipMethods.Ack).Build()).Build();
            Stx.ProcessRequest(new SipRequestEventBuilder().WithRequest(ackRequest).Build());
        }

        /// <summary>
        /// Expect_the_ack_to_be_absorbed == do nothing
        /// </summary>
        [Test]
        public void Expect_the_ack_to_be_absorbed()
        {
            Sender.Verify(s => s.SendResponse(_non200FinalResponse), Times.Once());
        }


    }
}