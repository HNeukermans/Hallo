using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipClientDialogTests
{
    [TestFixture]
    public class When_an_ok_response_is_received_3_times : SipDialogSpecificationBase
    {
        protected override void BeforeCreateInviteTransaction()
        {
            _provider.AddSipListener(new StubSipListener(onProcessResponse: OnProcessResponse));
        }

        private void OnProcessResponse(SipResponseEvent sipResponseEvent)
        {
            _onProcessResponseCount++;
            _sipResponseEvent = sipResponseEvent;

            if (sipResponseEvent.Dialog != null && sipResponseEvent.Response.StatusLine.StatusCode == 200)
            {
                var ack = sipResponseEvent.Dialog.As<SipInviteClientDialog>().CreateAck();
                sipResponseEvent.Dialog.As<SipInviteClientDialog>().SendAck(ack);
            }

        }

        protected override void GivenOverride()
        {
            /*fire ringing response to forec the dialog to go into early state*/
            var c = new SipContext();
            _response = CreateRingingResponse();
            c.Response = _response;
            _contextSource.FireNewContextReceivedEvent(c);
        }

        protected override void When()
        {
            /*create ringing response*/
            var c = new SipContext();
            _response = CreateOkResponse();
            c.Response = _response;
            /*fire twice*/
            _contextSource.FireNewContextReceivedEvent(c);
            _contextSource.FireNewContextReceivedEvent(c);
            _contextSource.FireNewContextReceivedEvent(c);
        }

        [Test]
        public void Expect_OnProcessResponse_to_be_invoked_2()
        {
            _onProcessResponseCount.Should().Be(2);
        }

        [Test]
        public void Expect_the_dialog_to_be_in_Confirmed_state()
        {
            _sipResponseEvent.ClientTransaction.As<SipInviteClientTransaction>().GetDialog().State.Equals(DialogState.Confirmed);
        }
    }
}