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
    public class When_in_early_an_ok_response_is_received : SipDialogSpecificationBase
    {
        private SipInviteClientTransaction _transaction;

        protected override void BeforeCreateInviteTransaction()
        {
            _provider.AddSipListener(new StubSipListener(onProcessResponse: OnProcessResponse));
        }

        private void OnProcessResponse(SipResponseEvent sipResponseEvent)
        {
            _onProcessResponseCount++;
            _sipResponseEvent = sipResponseEvent;
            _transaction = sipResponseEvent.ClientTransaction.As<SipInviteClientTransaction>();
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
            /*fire ok response*/
            var c = new SipContext();
            _response = CreateOkResponse();
            c.Response = _response;
            _contextSource.FireNewContextReceivedEvent(c);
        }

        [Test]
        public void Expect_the_transaction_to_be_in_terminated_state()
        {
            _transaction.State.Equals(SipInviteClientTransaction.TerminatedState);
        }

        [Test]
        public void Expect_the_dialog_to_be_in_confirmed_state()
        {
            _transaction.GetDialog().State.Equals(DialogState.Confirmed);
        }

        [Test]
        public void Expect_OnProcessResponse_to_be_invoked_2()
        {
            _onProcessResponseCount.Should().Be(2);
        }

        [Test]
        public void Expect_the_Event_dialog_not_to_be_null()
        {
            /*since ok does not match dialog*/
            _sipResponseEvent.Dialog.Should().NotBeNull();
        }

    }
}