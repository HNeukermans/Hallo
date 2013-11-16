using System;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
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
        public void Expect_the_phone_to_remain_in_WaitProvisional()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetWaitProvisional());
        }

        [Test]
        public void Expect_the_CancelOnWaitFinal_to_be_true()
        {
            _phone.PendingInvite.CancelOnWaitFinal.Should().BeTrue();
        }

        [Test]
        public void Expect_the_call_error_to_be_null()
        {
            _callError.Should().BeNull();
        }
    }
}