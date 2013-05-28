using Hallo.Sip;
using Hallo.Sip.Stack.Dialogs;
using Hallo.UnitTest.Builders;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipServerDialogTests
{
    //[TestFixture]
    //public class When_a_dialog_is_created_from_a_received_invite_request : SipDialogSpecificationBase
    //{
    //    protected override void When()
    //    {
    //        _inviteRequest = CreateInviteRequest();
    //        var c = new SipContextBuilder().WithRequest(_inviteRequest).Build();

    //        _contextSource.FireNewContextReceivedEvent(c);
    //    }

    //    public override void OnProcessRequestOverride()
    //    {
    //        if (_requestEvent.Request.RequestLine.Method == SipMethods.Invite)
    //        {
    //            _inviteTransaction = _provider.CreateServerTransaction(_requestEvent.Request);
    //            _dialog = _provider.CreateServerDialog(_inviteTransaction);
    //        }
    //    }

    //    [Test]
    //    public void Expect_the_LocalTag_to_be_()
    //    {
    //        _dialog.LocalTag = 
    //    }

    //    [Test]
    //    public void Expect_the_dialog_to_transition_to_Null_state()
    //    {
    //        _provider.GetDialog(_inviteTransaction).State.Equals(DialogState.Null);
    //    }


    //}
}