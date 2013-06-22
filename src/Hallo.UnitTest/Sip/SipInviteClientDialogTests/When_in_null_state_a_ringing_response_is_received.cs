using FluentAssertions;
using Hallo.Sip.Stack.Dialogs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientDialogTests
{
    public class When_in_null_state_a_ringing_response_is_received : SipInviteClientDialogSpecificationBase
    {
        protected override void When()
        {
            var ringingResponse = CreateRingingResponse();
            ClientDialog.SetLastResponse(ringingResponse);
        }

        [Test]
        public void Expect_the_dialog_to_transition_to_early_state()
        {
            ClientDialog.State.Should().Be(DialogState.Early);
        }

    }
}