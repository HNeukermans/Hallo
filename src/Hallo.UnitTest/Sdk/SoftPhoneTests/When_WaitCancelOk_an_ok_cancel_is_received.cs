using FluentAssertions;
using Hallo.Sdk;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitCancelOk_an_ok_cancel_is_received : When_WaitCancelOk_Base
    {
        protected override void When()
        {
            /*send ok to cancel*/
        }

        [Test]
        public void Expect_the_callstate_to_be_Ringing()
        {
            _callState.Should().Be(CallState.Ringing);
        }

        [Test]
        public void Expect_the_InternalState_to_remain()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetWaitCancelOk());
        }
    }
}