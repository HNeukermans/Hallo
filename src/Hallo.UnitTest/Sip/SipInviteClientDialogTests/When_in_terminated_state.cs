using FluentAssertions;
using Hallo.Sip.Stack.Dialogs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientDialogTests
{
    public class When_in_terminated_state : SipInviteClientDialogSpecificationBase
    {
        protected override void When()
        {
            /*force it to go into state*/
            ReceivedResponse = CreateOkResponse();
            ClientDialog.SetLastResponse(CreateRingingResponse());
            ClientDialog.SetLastResponse(CreateBusyHereResponse());
            ClientDialog.State.Should().Be(DialogState.Terminated); /*required assertion*/
        }

        [Test]
        public void Expect_the_dialog_to_be_removed_from_table()
        {
            DialogTable.ContainsKey(ClientDialog.GetId()).Should().BeFalse();
        }

        [Test]
        public void Expect_the_dialogtable_to_zero_dialog()
        {
            DialogTable.Count.Should().Be(0);
        }
    }
}