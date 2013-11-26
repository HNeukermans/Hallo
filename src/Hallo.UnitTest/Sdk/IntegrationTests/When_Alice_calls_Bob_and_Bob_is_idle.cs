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
}