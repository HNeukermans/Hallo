using System;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Util;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitProvisional_the_call_is_cancelled : When_WaitProvisional_Base
    {
        
        protected override void OnTestClientUaReceive(SipContext sipContext)
        {
            if (sipContext.Request.RequestLine.Method == SipMethods.Invite)
            {
                _waitingforInviteReceived.Set();
            }
        }

        protected override void When()
        {
            _call.Stop();
        }
        
        [Test]
        public void Expect_the_phone_to_transition_to_WaitCancelOk_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetWaitCancelOk());
        }

        [Test]
        public void Expect_the_call_error_to_be_null()
        {
            _callError.Should().BeNull();
        }
    }
}