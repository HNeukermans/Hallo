using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Dialogs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientDialogTests
{
    public class When_in_early_state_a_x200_final_response_is_received : SipInviteClientDialogSpecificationBase
    {
        private SipResponse _okResponse;

        protected override void GivenOverride()
        {
            /*force it to go into early state*/
            var ringingResponse = CreateRingingResponse();
            ClientDialog.SetLastResponse(ringingResponse);
            ClientDialog.State.Should().Be(DialogState.Early); /*required assertion*/
        }

        protected override void When()
        {
            _okResponse = CreateOkResponse();
            ClientDialog.SetLastResponse(_okResponse);
        }

        [Test]
        public void Expect_the_dialog_to_transition_to_confirmed_state()
        {
            ClientDialog.State.Should().Be(DialogState.Confirmed);
        }
    }
}