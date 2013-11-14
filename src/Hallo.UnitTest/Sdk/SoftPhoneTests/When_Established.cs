using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.UnitTest.Helpers;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_Established : When_Established_Base
    {
        
        [Test]
        public void Expect_at_least_a_ringing_response_is_sent()
        {
            _receivedRingingResponse.Should().NotBeNull();
        }
       
        [Test]
        public void Expect_PendingInvite_not_to_be_null()
        {
            _phone.PendingInvite.Should().NotBeNull();
        }

        [Test]
        public void Expect_InviteTransaction_to_be_terminated()
        {
            _phone.PendingInvite.InviteServerTransaction.State.Should().Be(SipInviteServerTransaction.TerminatedState);
        }

        [Test]
        public void Expect_IsIncomingCall_to_be_true()
        {
            _phone.PendingInvite.IsIncomingCall.Should().BeTrue();
        }

        [Test]
        public void Expect_Dailog_not_to_be_null()
        {
            _phone.PendingInvite.ServerDialog.Should().NotBeNull();
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
        public void Expect_dialog_to_be_confirmed()
        {
            _phone.PendingInvite.ServerDialog.State.Should().Be(Hallo.Sip.Stack.Dialogs.DialogState.Confirmed);
        }
        
        [Test]
        public void Expect_the_callstate_to_be_InCall()
        {
            _callState.Should().Be(CallState.InCall);
        }

        [Test]
        public void Expect_DialogTable_to_have_1_dialog()
        {
            _sipProvider1.DialogTable.Count.Should().Be(1);
        }
        
        [Test]
        public void Expect_WaitForAck_timer_to_be_stopped()
        {
            _waitforAckTimer.IsStarted.Should().BeFalse();
        }

    }
}