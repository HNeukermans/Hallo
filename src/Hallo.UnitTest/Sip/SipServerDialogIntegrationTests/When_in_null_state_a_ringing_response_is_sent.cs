using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Dialogs;
using Hallo.UnitTest.Builders;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipServerDialogTests
{
    [TestFixture]
    public class When_in_null_state_a_ringing_response_is_sent : SipDialogSpecificationBase
    {
       
        protected override void GivenOverride()
        {

        }

        protected override void When()
        {
            _inviteRequest = CreateInviteRequest();
            var c = new SipContextBuilder().WithRequest(_inviteRequest).Build();

            _contextSource.FireNewContextReceivedEvent(c);
        }

        public override void OnProcessRequestOverride()
        {
            if(_requestEvent.Request.RequestLine.Method == SipMethods.Invite)
            {
                _inviteTransaction = _provider.CreateServerTransaction(_requestEvent.Request);
                _dialog = _provider.CreateServerDialog(_inviteTransaction);
                var response = CreateRingingResponse();
                _inviteTransaction.SendResponse(response);
            }
        }
        
        [Test]
        public void Expect_the_dialog_not_to_be_null()
        {
            _dialog.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_dialog_to_transition_to_Early_state()
        {
            _dialog.State.Equals(DialogState.Early);
        }
    }
}