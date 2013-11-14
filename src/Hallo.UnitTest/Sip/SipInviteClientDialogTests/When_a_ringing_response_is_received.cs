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
    public class When_a_ringing_response_is_received : SipDialogSpecificationBase
    {
        protected override void BeforeCreateInviteTransaction()
        {
            _provider.AddSipListener(new StubSipListener(onProcessResponse: OnProcessResponse));
        }

        private void OnProcessResponse(SipResponseEvent sipResponseEvent)
        {
            _onProcessResponseCount++;
            _sipResponseEvent = sipResponseEvent;
        }

        protected override void When()
        {
            /*create ringing response*/
            var c = new SipContext();
            _response = CreateRingingResponse();
            c.Response = _response;
            _contextSource.FireNewContextReceivedEvent(c);
        }

        [Test]
        public void Expect_OnProcessResponse_to_be_invoked()
        {
            _onProcessResponseCount.Should().Be(1);
            _sipResponseEvent.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_ClientTransaction_not_to_be_null()
        {
            _sipResponseEvent.ClientTransaction.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_ClientTransaction_to_be_of_type_SipInviteClientTransaction()
        {
            _sipResponseEvent.ClientTransaction.Should().BeOfType<SipInviteClientTransaction>();
        }

        [Test]
        public void Expect_the_dialog_not_to_be_null()
        {
            _sipResponseEvent.ClientTransaction.As<SipInviteClientTransaction>().GetDialog().Should().NotBeNull();
        }
        
        [Test]
        public void Expect_the_dialog_to_be_in_Early_state()
        {
            _sipResponseEvent.ClientTransaction.As<SipInviteClientTransaction>().GetDialog().State.Equals(DialogState.Early);
        }
    }
}