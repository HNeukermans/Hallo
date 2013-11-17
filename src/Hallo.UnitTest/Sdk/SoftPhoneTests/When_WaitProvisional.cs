using System;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitProvisional : When_WaitProvisional_Base
    {
        [Test]
        public void Expect_the_callstate_to_be_setup()
        {
            _callState.Should().Be(CallState.Setup);
        }
        
        [Test]
        public void Expect_PendingInvite_not_to_be_null()
        {
            _phone.PendingInvite.Should().NotBeNull();
        }

        [Test]
        public void Expect_PendingCall_not_to_be_null()
        {
            _phone.PendingCall.Should().NotBeNull();
        }

        [Test]
        public void Expect_InviteClientTransaction_not_to_be_null()
        {
            _phone.PendingInvite.InviteClientTransaction.Should().NotBeNull();
        }

        [Test]
        public void Expect_InviteClientTransaction_to_be_in_calling_state()
        {
            _phone.PendingInvite.InviteClientTransaction.State.Should().Be(SipInviteClientTransaction.CallingState);
        }

        [Test]
        public void Expect_IsIncomingCall_to_be_false()
        {
            _phone.PendingInvite.IsIncomingCall.Should().BeFalse();
        }

        [Test]
        public void Expect_ClientDialog_not_to_be_null()
        {
            _phone.PendingInvite.Dialog.Should().NotBeNull();
        }

        [Test]
        public void Expect_OriginalRequest_not_to_be_null()
        {
            _phone.PendingInvite.OriginalRequest.Should().NotBeNull();
        }

        [Test]
        public void Expect_RingingResponse_to_be_null()
        {
            _phone.PendingInvite.RingingResponse.Should().BeNull();
        }

        [Test]
        public void Expect_Provider_DialogTable_to_have_0_dialog()
        {
            //in this state the dialog does not exist in the table yet.
            _sipProvider1.DialogTable.Count.Should().Be(0);
        }

        [Test]
        public void Expect_InviteClientTransaction_to_have_the_dialog()
        {
            _phone.PendingInvite.InviteClientTransaction.GetDialog().Should().NotBeNull();
        }

        [Test]
        public void Expect_ClientDialog_to_be_in_Null_state()
        {
            _phone.PendingInvite.Dialog.State.Should().Be(Hallo.Sip.Stack.Dialogs.DialogState.Null);
        }
        
    }
}