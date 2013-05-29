using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Dialogs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerDialogTests
{
    public class When_in_early_state_a_non_x200_final_response_is_send : SipInviteServerDialogSpecificationBase
    {
        private SipResponse _busyHereResponse;

        protected override void GivenOverride()
        {
            /*force it to go into early state*/
            var ringingResponse = CreateRingingResponse();
            ServerDialog.SetLastResponse(ringingResponse);
            ServerDialog.State.Should().Be(DialogState.Early); /*required assertion*/
        }

        protected override void When()
        {
            _busyHereResponse = CreateBusyHereResponse();
            ServerDialog.SetLastResponse(_busyHereResponse);
        }

        [Test]
        public void Expect_the_dialog_to_transition_to_Terminated_state()
        {
            ServerDialog.State.Should().Be(DialogState.Terminated);
        }
    }
}