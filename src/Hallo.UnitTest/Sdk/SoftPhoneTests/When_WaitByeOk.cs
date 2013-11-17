using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip.Stack.Transactions.InviteServer;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitByeOk : When_WaitByeOk_Base
    {
        [Test]
        public void Expect_ua_testclient_to_have_received_a_bye()
        {
            _receivedBye.Should().NotBeNull();
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
        public void Expect_the_callstate_to_be_Completed()
        {
            _callState.Should().Be(CallState.Completed);
        }

        [Test]
        public void Expect_DialogTable_to_have_0_dialog()
        {
            _sipProvider1.DialogTable.Count.Should().Be(0);
        }

        [Test]
        public void Expect_ClientTransactionTable_to_have_1_Tx()
        {
            /*must contain the bye tx*/
            _sipProvider1.ClientTransactionTable.Count.Should().Be(1);
        }

    }
}