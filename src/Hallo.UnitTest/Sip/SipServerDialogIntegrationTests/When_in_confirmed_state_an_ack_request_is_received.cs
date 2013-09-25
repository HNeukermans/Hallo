using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.UnitTest.Builders;
using Hallo.Util;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipServerDialogTests
{
    [TestFixture]
    public class When_in_confirmed_state_an_ack_request_is_received : SipDialogSpecificationBase
    {
        protected override void GivenOverride()
        {
            _inviteRequest = CreateInviteRequest();
            var c = new SipContextBuilder().WithRequest(_inviteRequest).Build();
            _contextSource.FireNewContextReceivedEvent(c);
        }

        protected override void When()
        {
            var c = new SipContextBuilder().WithRequest(_inviteRequest).Build();
            _contextSource.FireNewContextReceivedEvent(c);

            GoFromEarlyToConfirmedState();
        }

        public override void OnProcessRequestOverride()
        {
            if (_requestEvent.Request.RequestLine.Method == SipMethods.Invite)
            {
                GoToEarlyState();
            }
        }

        [Test]
        public void Expect_the_dialog_not_to_be_null()
        {
            Assert.Fail();
           // _provider.GetDialog(_inviteTransaction).Should().NotBeNull();
        }

        [Test]
        public void Expect_the_dialog_to_transition_to_Confirmed_state()
        {
            Assert.Fail();
            //_provider.GetDialog(_inviteTransaction).State.Equals(DialogState.Confirmed);
        }
    }
}