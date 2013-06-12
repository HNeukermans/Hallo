using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Dialogs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientDialogTests
{
    public class When_in_null_state : SipInviteClientDialogSpecificationBase
    {
        protected override void When()
        {
            base.When();
        }

        [Test]
        public void Expect_the_CallId_to_be_null()
        {
            ClientDialog.CallId.Should().BeNull();
        }

        [Test]
        public void Expect_the_RemoteSequenceNr_to_be_minus_one()
        {
            ClientDialog.RemoteSequenceNr.Should().Be(-1);
        }


        [Test]
        public void Expect_the_RemoteTarget_to_be_null()
        {
            ClientDialog.RemoteTarget.Should().BeNull();
        }

        [Test]
        public void Expect_the_RouteSet_to_be_empty()
        {
            ClientDialog.RouteSet.Should().BeEmpty();
        }

        [Test]
        public void Expect_LocalSequenceNr_to_be_minus_one()
        {
            ClientDialog.LocalSequenceNr.Should().Be(-1);
        }

        [Test]
        public void Expect_the_localtag_to_be_null()
        {
            ClientDialog.LocalTag.Should().BeNull();
        }

        [Test]
        public void Expect_the_Remotetag_to_be_null()
        {
            ClientDialog.RemoteTag.Should().BeNull();
        }
        
        [Test]
        public void Expect_the_RemoteUri_to_be_null()
        {
            ClientDialog.RemoteUri.Should().BeNull();
        }

        [Test]
        public void Expect_the_LocalUri_to_be_null()
        {
            ClientDialog.LocalUri.Should().BeNull();
        }
       
        [Test]
        public void Expect_the_dialogtable_to_contain_zero_dialogs()
        {
            DialogTable.Count.Should().Be(0);
        }

        [Test]
        [ExpectedException(typeof(SipCoreException))]
        public void Expect_the_dialog_GetId_to_throw_an_exception()
        {
            ClientDialog.GetId();
        }


    }
}