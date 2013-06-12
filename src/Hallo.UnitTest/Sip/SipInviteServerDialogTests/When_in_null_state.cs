using System.Linq;
using FluentAssertions;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerDialogTests
{
    public class When_in_null_state : SipInviteServerDialogSpecificationBase
    {
        protected override void When()
        {
            base.When();
        }

        [Test]
        public void Expect_the_CallId_to_be_same_as_the_request_CallId()
        {
            ServerDialog.CallId.Should().Be(ReceivedRequest.CallId.Value);
        }

        [Test]
        public void Expect_the_RemoteSequenceNr_to_be_the_Cseq_of_the_request()
        {
            //the remote sequence number should be set to the value of the sequence number in the CSeq header field of the request
            ServerDialog.RemoteSequenceNr.Should().Be(ReceivedRequest.CSeq.Sequence);
        }

        [Test]
        public void Expect_the_RemoteTarget_to_be_contact_of_the_request()
        {
            //tthe remote target is set to the URI from the Contact header field of the request
            ServerDialog.RemoteTarget.Should().Be(ReceivedRequest.Contacts.First().SipUri);
        }

        [Test]
        public void Expect_the_RouteSet_to_be_RecordRoutes_of_the_request()
        {
            var oc = ObjectComparer.Create();
            oc.Compare(ServerDialog.RouteSet, ReceivedRequest.RecordRoutes.ToList());
            oc.Differences.Should().BeEmpty();
        }

        [Test]
        public void Expect_LocalSequenceNr_to_be_empty()
        {
            ServerDialog.LocalSequenceNr.Should().Be(-1);
        }

        [Test]
        public void Expect_the_localtag_to_be_empty()
        {
            ServerDialog.LocalTag.Should().BeNull();
        }

        [Test]
        public void Expect_the_Remotetag_to_be_From_tag_of_the_request()
        {
            ServerDialog.RemoteTag.Should().Be(ReceivedRequest.From.Tag);
        }

        [Test]
        public void Expect_the_RemoteUri_to_be_FromUri_of_the_request()
        {
            ServerDialog.RemoteUri.Should().Be(ReceivedRequest.From.SipUri);
        }

        [Test]
        public void Expect_the_LocalUri_to_be_ToUri_of_the_request()
        {
            ServerDialog.LocalUri.Should().Be(ReceivedRequest.To.SipUri);
        }

        [Test]
        public void Expect_the_LocalTag_to_be_null()
        {
            ServerDialog.LocalTag.Should().BeNull();
        }

        [Test]
        public void Expect_the_dialog_not_to_be_in_DialogTable()
        {
            DialogTable.ContainsKey(ServerDialog.GetId()).Should().BeFalse();
        }

        [Test]
        public void Expect_the_dialogtable_to_contain_zero_dialogs()
        {
            DialogTable.Count.Should().Be(0);
        }

        [Test]
        public void Expect_RetransitTimer_not_to_be_started()
        {
            RetransitOkTimer.IsStarted.Should().BeFalse();
        }

        [Test]
        public void Expect_TimeOutTimer_not_to_be_started()
        {
            EndWaitForAckTimer.IsStarted.Should().BeFalse();
        }
    }
}