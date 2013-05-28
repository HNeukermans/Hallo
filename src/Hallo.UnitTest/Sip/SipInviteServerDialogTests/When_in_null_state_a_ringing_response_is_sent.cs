using FluentAssertions;
using Hallo.Sip.Stack.Dialogs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerDialogTests
{
    public class When_in_null_state_a_ringing_response_is_sent : SipInviteServerDialogSpecificationBase
    {
        protected override void When()
        {
            var ringingResponse = CreateRingingResponse();
            ServerDialog.SetLastResponse(ringingResponse);
        }

        [Test]
        public void Expect_the_dialog_to_transition_to_Early_state()
        {
            ServerDialog.State.Should().Be(DialogState.Early);
        }

        
    }
}