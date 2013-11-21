using System;
using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
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
    internal class When_Ringing_the_call_is_accepted : When_Ringing_Base
    {
       
        protected override void When()
        {
            _incomingCall.Accept();
            
        }

        [Test]
        public void Expect_the_phone_to_transition_to_waitforack_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetWaitForAck());
        }

        [Test]
        public void Expect_InviteTransaction_to_be_terminated()
        {
            _phone.PendingInvite.InviteServerTransaction.State.Should().Be(SipInviteServerTransaction.TerminatedState);
        }
    }
}
