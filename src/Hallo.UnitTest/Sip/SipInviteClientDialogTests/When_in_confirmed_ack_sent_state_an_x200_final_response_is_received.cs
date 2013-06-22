using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions;
using Hallo.UnitTest.Builders;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientDialogTests
{
    public class When_in_confirmed_ack_sent_state_an_x200_final_response_is_received : SipInviteClientDialogSpecificationBase
    {
        private int _beforeSentSeqNr;
        private Mock<ISipClientTransaction> _txMock;
        private SipResponse _okResponse;
        private SipRequest _ackRequest;

        protected override void GivenOverride()
        {
            /*force it to go into confirmed state*/
            _okResponse = CreateOkResponse();
            var rer = new SipResponseEventBuilder().WithResponse(CreateRingingResponse()).Build();
            var reo = new SipResponseEventBuilder().WithResponse(_okResponse).Build();
            ClientDialog.ProcessResponse(rer);
            ClientDialog.ProcessResponse(reo);
            ClientDialog.State.Should().Be(DialogState.Confirmed); /*required assertion*/

            _ackRequest = ClientDialog.CreateAck();
            ClientDialog.SendAck(_ackRequest);  /*sent an ack*/
            _beforeSentSeqNr = ClientDialog.LocalSequenceNr;
        }

        protected override void When()
        {
            var cb = new SipContextBuilder().WithResponse(_okResponse).Build();
            var rEvent = new SipResponseEvent(cb);
            ClientDialog.ProcessResponse(rEvent);
        }

        [Test]
        public void Expect_the_listener_not_to_be_notified_of_the_resent_ok()
        {
            /*expect only notified for ringing + first ok*/
            Listener.Verify((l) => l.ProcessResponse(It.IsAny<SipResponseEvent>()), Times.Exactly(2));
        }

        [Test]
        public void Expect_the_ack_to_be_resent()
        {
            /*expect send for first ack, ack resent*/
            Sender.Verify((s) => s.SendRequest(It.Is<SipRequest>((r)=> r.RequestLine.Method == SipMethods.Ack)), Times.Exactly(2));
        }
    }
}