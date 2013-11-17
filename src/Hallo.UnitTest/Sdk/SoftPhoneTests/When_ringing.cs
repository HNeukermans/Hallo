using System;
using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.Sip.Util;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;
using Hallo.Component;
using System.Linq;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_Ringing : When_Ringing_Base
    {
        protected override void When()
        {            
            
        }
        
        [Test]
        public void Expect_at_least_a_ringing_response_is_sent()
        {
            _receivedRingingResponse.Should().NotBeNull();
        }

        [Test]
        public void Expect_ringing_timer_to_be_started()
        {
            _ringingTimer.IsStarted.Should().BeTrue();
        }

        [Test]
        public void Expect_PendingInvite_not_to_be_null()
        {
            _phone.PendingInvite.Should().NotBeNull();
        }

        [Test]
        public void Expect_InviteTransaction_not_to_be_null()
        {
            _phone.PendingInvite.InviteServerTransaction.Should().NotBeNull();
        }

        [Test]
        public void Expect_InviteTransaction_to_be_proceeding()
        {
            _phone.PendingInvite.InviteServerTransaction.State.Should().Be(SipInviteServerTransaction.ProceedingState);
        }

        [Test]
        public void Expect_IsIncomingCall_to_be_true()
        {
            _phone.PendingInvite.IsIncomingCall.Should().BeTrue();
        }

        [Test]
        public void Expect_Dailog_not_to_be_null()
        {
            _phone.PendingInvite.Dialog.Should().NotBeNull();
        }

        [Test]
        public void Expect_OriginalRequest_not_to_be_null()
        {
            _phone.PendingInvite.OriginalRequest.Should().NotBeNull();
        }

        [Test]
        public void Expect_RingingResponse_not_to_be_null()
        {
            _phone.PendingInvite.RingingResponse.Should().NotBeNull();
        }

        [Test]
        public void Expect_Provider_DialogTable_to_have_1_dialog()
        {
            _sipProvider1.DialogTable.Count.Should().Be(1);
        }

        [Test]
        public void Expect_dialog_to_be_in_early_state()
        {
            _phone.PendingInvite.Dialog.State.Should().Be(Hallo.Sip.Stack.Dialogs.DialogState.Early);
        }

        [Test]
        public void Expect_the_phone_call_state_to_be_Ringing()
        {
            _callState.Should().Be(CallState.Ringing);
        }


    }
}