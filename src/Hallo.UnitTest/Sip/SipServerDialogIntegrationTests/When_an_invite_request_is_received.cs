using System;
using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Sip.SipClientDialogTests;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipServerDialogTests
{
    //[TestFixture]
    //public class When_an_invite_request_is_received : SipDialogSpecificationBase
    //{
        
    //    protected override void GivenOverride()
    //    {
            
    //    }

    //    //protected override void When()
    //    //{
    //    //    _contextSource = new FakeSipContextSource();
    //    //    _sipStack = new SipStack();
    //    //    _provider = new SipProvider(_sipStack, _contextSource);
            
    //    //    /*a request was sent in the Given()*/
    //    //    _inviteRequest = CreateInviteRequest();
    //    //    var c = new SipContextBuilder().WithRequest(_inviteRequest).Build();

    //    //    _contextSource.FireNewContextReceivedEvent(c);
    //    //}


    //    [Test]
    //    public void Expect_the_dialog_not_to_be_null()
    //    {
    //        _provider.GetDialog(_inviteTransaction).Should().NotBeNull();
    //    }

    //    [Test]
    //    public void Expect_the_dialog_to_transition_to_Null_state()
    //    {
    //        _provider.GetDialog(_inviteTransaction).State.Equals(DialogState.Null);
    //    }
    //}
}