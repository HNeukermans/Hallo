using FluentAssertions;
using Hallo.Sdk;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitCancelOk_an_x487_invite_is_received : When_WaitCancelOk_Base
    {
        protected override void When()
        {
            /*send 481 to invite*/
        }

        [Test]
        public void Expect_the_InternalState_to_remain()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetWaitCancelOk());
        }

        [Test]
        public void Expect_the_callstate_to_be_Completed()
        {
            _callState.Should().Be(CallState.Completed);
        }
    }
}