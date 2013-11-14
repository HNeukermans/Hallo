using System;
using System.Linq;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerDialogTests
{
    public class When_in_early_state : SipInviteServerDialogSpecificationBase
    {
        private SipResponse _ringingResponse;

        public When_in_early_state()
        {
            
        }

        protected override void When()
        {
            /*force it to go into early state*/
            _ringingResponse = CreateRingingResponse();
            ServerDialog.SetLastResponse(_ringingResponse);
            ServerDialog.State.Should().Be(DialogState.Early); /*required assertion*/
        }

        [Test]
        public void Expect_the_CallId_to_be_same_as_the_request_CallId()
        {
            ServerDialog.CallId.Should().Be(ReceivedRequest.CallId.Value);
        }
       
        [Test]
        public void Expect_the_Remotetag_to_be_From_tag_of_the_request()
        {
            ServerDialog.RemoteTag.Should().Be(ReceivedRequest.From.Tag);
        }

        [Test]
        public void Expect_the_LocalTag_to_be_To_Tag_of_the_response()
        {
            ServerDialog.LocalTag.Should().Be(_toTag);
        }

        [Test]
        public void Expect_the_HasAckReceived_to_be_false()
        {
            ServerDialog.HasAckReceived.Should().BeFalse();
        }

        [Test]
        public void Expect_RetransitTimer_not_to_be_started()
        {
            RetransitOkTimer.IsStarted.Should().BeFalse();
        }

        [Test]
        [Ignore("EndWaitForAckTimer is move out of Dialog, and left to the responsibility of the Dialog user.")]
        public void Expect_TimeOutTimer_not_to_be_started()
        {
            EndWaitForAckTimer.IsStarted.Should().BeFalse();
        }

        [Test]
        public void Expect_the_dialog_to_be_in_the_DialogTable()
        {
            DialogTable.ContainsKey(ServerDialog.GetId()).Should().BeTrue();
        }

        [Test]
        public void Expect_the_dialog_to_contain_1_dialog()
        {
            DialogTable.Count.Should().Be(1);
        }

    }
}