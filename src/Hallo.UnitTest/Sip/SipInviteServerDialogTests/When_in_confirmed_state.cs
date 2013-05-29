using FluentAssertions;
using Hallo.Sip.Stack.Dialogs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerDialogTests
{
    public class When_in_confirmed_state : SipInviteServerDialogSpecificationBase
    {
        public When_in_confirmed_state()
        {

        }

        protected override void When()
        {
            /*force it to go into confirmed state*/
            ServerDialog.SetLastResponse(CreateRingingResponse());
            ServerDialog.SetLastResponse(CreateOkResponse());
            ServerDialog.State.Should().Be(DialogState.Confirmed); /*required assertion*/
        }
        
        [Test]
        public void Expect_RetransitTimer_to_be_started()
        {
            RetransitOkTimer.IsStarted.Should().BeTrue();
        }

        [Test]
        public void Expect_TimeOutTimer_to_be_started()
        {
            EndWaitForAckTimer.IsStarted.Should().BeTrue();
        }
        
    }
}