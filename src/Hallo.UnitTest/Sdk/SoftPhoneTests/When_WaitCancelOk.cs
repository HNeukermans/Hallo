using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitCancelOk : When_WaitCancelOk_Base
    {
        [Test]
        public void Expect_the_callstate_to_be_Ringing()
        {
            _callState.Should().Be(CallState.Ringing);
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
        public void Expect_DialogTable_to_have_1_dialog()
        {
            _sipProvider1.DialogTable.Count.Should().Be(1);
        }

        [Test]
        public void Expect_dialog_to_be_Early()
        {
            _phone.PendingInvite.ClientDialog.State.Should().Be(Hallo.Sip.Stack.Dialogs.DialogState.Early);
        }

        [Test]
        public void Expect_InviteClientTransaction_not_to_be_null()
        {
            _phone.PendingInvite.InviteClientTransaction.Should().NotBeNull();
        }

        [Test]
        public void Expect_CancelTransaction_not_to_be_null()
        {
            _phone.PendingInvite.CancelTransaction.Should().NotBeNull();
        }
        
        [Test]
        public void Expect_InviteClientTransactionTable_to_have_2_tx()
        {
            /*invite + cancel*/
            _sipProvider1.ClientTransactionTable.Count.Should().Be(2);
        }

        [Test]
        public void Expect_InviteClientTransaction_to_be_in_calling_state()
        {
            _phone.PendingInvite.InviteClientTransaction.State.Should().Be(SipInviteClientTransaction.ProceedingState);
        }

        [Test]
        public void Expect_CancelTransaction_to_be_in_Trying_state()
        {
            _phone.PendingInvite.CancelTransaction.State.Should().Be(SipNonInviteClientTransaction.TryingState);
        }
        
        
    }
}