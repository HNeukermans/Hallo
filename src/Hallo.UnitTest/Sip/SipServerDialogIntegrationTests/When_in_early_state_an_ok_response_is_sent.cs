using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.UnitTest.Builders;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipServerDialogTests
{
    [TestFixture]
    public class When_in_early_state_an_ok_response_is_sent : SipDialogSpecificationBase
    {

        protected override void GivenOverride()
        {

        }

        protected override void When()
        {
            _inviteRequest = CreateInviteRequest();
            var c = new SipContextBuilder().WithRequest(_inviteRequest).Build();
            _contextSource.FireNewContextReceivedEvent(c);
            
            /*send an ok*/
            var okResponse = _inviteTransaction.Request.CreateResponse(SipResponseCodes.x200_Ok);
            _inviteTransaction.SendResponse(okResponse);
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
            //_provider.GetDialog(_inviteTransaction).Should().NotBeNull();
        }

        [Test]
        public void Expect_the_dialog_to_transition_to_Confirmed_state()
        {
            Assert.Fail();
           //_provider.GetDialog(_inviteTransaction).State.Equals(DialogState.Confirmed);
        }
    }
}