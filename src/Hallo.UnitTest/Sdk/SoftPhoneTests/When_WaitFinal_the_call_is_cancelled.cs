using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitFinal_the_call_is_cancelled : When_WaitFinal_Base
    {
        protected override void When()
        {
            _call.Stop();
        }

        [Test]
        public void Expect_the_phone_to_transition_to_WaitforCancelOk_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetWaitCancelOk());
        }
        
    }
}