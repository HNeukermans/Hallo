using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.UnitTest.Builders;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientDialogTests
{
    public class When_in_early_state_a_non_x200_final_response_is_received : SipInviteClientDialogSpecificationBase
    {
        private SipResponse _okResponse;

        protected override void GivenOverride()
        {
            /*force it to go into early state*/
            var ringingResponse = CreateRingingResponse();
            var c = new SipResponseEventBuilder().WithResponse(ringingResponse).WithClientTx(InviteCtx.Object).Build();
            ClientDialog.ProcessResponse(c);
            ClientDialog.State.Should().Be(DialogState.Early); /*required assertion*/
        }

        protected override void When()
        {
             _okResponse = CreateBusyHereResponse();
             var c = new SipContextBuilder().WithResponse(_okResponse).Build();
             ClientDialog.ProcessResponse(new SipResponseEvent(c));
        }

        [Test]
        public void Expect_the_dialog_to_transition_to_terminated_state()
        {
            ClientDialog.State.Should().Be(DialogState.Terminated);
        }
    }
}