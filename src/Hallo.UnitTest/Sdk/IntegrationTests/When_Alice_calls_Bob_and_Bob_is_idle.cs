using FluentAssertions;
using Hallo.Sdk;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_Alice_calls_Idle_Bob : IntegrationTestBase
    {
        protected override void When()
        {
            CallBob();
            SleepSeconds(1);
        }

        [Test]
        public void Expect_alice_to_be_in_ringback_state()
        {
            _callStateAlice.Should().Be(CallState.Ringback);
        }
    }

    internal class When_Alice_calls_accepting_Bob : IntegrationTestBase
    {
        protected override void When()
        {
            CallBob();
            SleepSeconds(1);
        }

        protected override void OnBobCallStateChanged()
        {
            if (_callStateBob == CallState.Ringing) _incomingCallBob.Accept();
        }
    

        [Test]
        public void Expect_alice_to_be_in_incall_state()
        {
            _callStateAlice.Should().Be(CallState.InCall);
        }

    }

    internal class When_Alice_calls_Idle_Bob_and_hangsup : IntegrationTestBase
    {
        protected override void When()
        {
            CallBob();
            SleepMilliSeconds(1000);
            _outgoingCallBob.Stop();
            SleepMilliSeconds(1000);
        }
        
        [Test]
        public void Expect_alice_to_be_in_cancelled_state()
        {
            _callStateAlice.Should().Be(CallState.Cancelled);
        }

    }
}