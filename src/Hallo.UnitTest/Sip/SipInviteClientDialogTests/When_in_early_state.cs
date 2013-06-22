using System.Linq;
using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Dialogs;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientDialogTests
{
    public class When_in_early_state : SipInviteClientDialogSpecificationBase
    {
        protected override void When()
        {
            /*force it to go into early state*/
            ReceivedResponse = CreateRingingResponse();
            ClientDialog.SetLastResponse(ReceivedResponse);
            ClientDialog.State.Should().Be(DialogState.Early); /*required assertion*/
        }


        [Test]
        public void Expect_the_CallId_not_to_be_null()
        {
            ClientDialog.CallId.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_CallId_to_be_the_callid_of_the_request()
        {
            ClientDialog.CallId.Should().Be(InvitingRequest.CallId.Value);
        }

        [Test]
        public void Expect_the_RemoteSequenceNr_to_be_minus_one()
        {
            //the remote sequence number should be set to the value of the sequence number in the CSeq header field of the request
            ClientDialog.RemoteSequenceNr.Should().Be(-1);
        }

        [Test]
        public void Expect_the_RemoteTarget_to_be_contact_of_the_response()
        {
            //tthe remote target is set to the URI from the Contact header field of the request
            ClientDialog.RemoteTarget.Should().Be(ReceivedResponse.Contacts.First().SipUri);
        }

        [Test]
        public void Expect_the_RouteSet_to_be_RecordRoutes_of_the_response_in_reverse_order()
        {
            var oc = ObjectComparer.Create();
            oc.Compare(ClientDialog.RouteSet, ReceivedResponse.RecordRoutes.ToList());
            oc.Differences.Should().BeEmpty();
        }

        [Test]
        public void Expect_LocalSequenceNr_to_be_cseq_sequence_of_the_request()
        {
            ClientDialog.LocalSequenceNr.Should().Be(InvitingRequest.CSeq.Sequence);
        }

        [Test]
        public void Expect_the_localtag_to_be_tag_of_the_fromheader()
        {
            ClientDialog.LocalTag.Should().Be(InvitingRequest.From.Tag);
        }

        [Test]
        public void Expect_the_localtag_not_to_be_null()
        {
            ClientDialog.LocalTag.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_Remotetag_not_to_be_null()
        {
            ClientDialog.RemoteTag.Should().NotBeNull();
        }
       
        [Test]
        public void Expect_the_RemoteUri_to_be_uri_of_the_toheader_of_the_request()
        {
            ClientDialog.RemoteUri.Should().Be(InvitingRequest.To.SipUri);
        }

        [Test]
        public void Expect_the_RemoteUri_to_be_not_null()
        {
            ClientDialog.RemoteUri.Should().NotBeNull();
        }
        
        [Test]
        public void Expect_the_LocalUri_to_be_not_null()
        {
            ClientDialog.LocalUri.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_LocalUri_to_be_uri_of_the_fromheader_of_the_request()
        {
            ClientDialog.LocalUri.Should().Be(InvitingRequest.From.SipUri);
        }

        [Test]
        public void Expect_the_RemoteTag_to_be_tag_of_the_toheader()
        {
            ClientDialog.RemoteTag.Should().Be(ReceivedResponse.To.Tag);
        }

        [Test]
        public void Expect_the_RemoteTag_not_to_be_null()
        {
            ClientDialog.RemoteTag.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_Id_of_the_dialog_to_be_CallId_FromTag_ToTag_concatenated()
        {
            string expectedId = InvitingRequest.CallId.Value;
            expectedId += ":" + InvitingRequest.From.Tag;
            expectedId += ":" + ReceivedResponse.To.Tag;
            ClientDialog.GetId().Should().Be(expectedId);
        }

        [Test]
        public void Expect_the_dialog_to_be_in_DialogTable()
        {
            DialogTable.ContainsKey(ClientDialog.GetId()).Should().BeTrue();
        }

        [Test]
        public void Expect_the_dialogtable_to_contain_one_dialog()
        {
            DialogTable.Count.Should().Be(1);
        }


    }
}