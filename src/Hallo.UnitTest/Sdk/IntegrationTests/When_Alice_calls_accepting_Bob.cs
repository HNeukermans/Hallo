using FluentAssertions;
using Hallo.Sdk;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
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
}