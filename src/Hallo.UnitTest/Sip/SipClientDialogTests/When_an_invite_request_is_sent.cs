using FluentAssertions;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions.InviteClient;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipClientDialogTests
{
    [TestFixture]
    public class When_an_invite_request_is_sent : SipDialogSpecificationBase
    {
        protected override void When()
        {
            /*a request was sent in the Given()*/
        }


        [Test]
        public void Expect_the_dialog_not_to_be_null()
        {
            _inviteTransaction.GetDialog().Should().NotBeNull();
        }
        
        [Test]
        public void Expect_the_dialog_to_be_in_Null_state()
        {
            _inviteTransaction.GetDialog().State.Equals(DialogState.Null);
        }
    }
}