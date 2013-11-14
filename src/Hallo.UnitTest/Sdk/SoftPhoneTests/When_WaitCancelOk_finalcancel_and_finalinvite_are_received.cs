using FluentAssertions;
using Hallo.Sdk;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitCancelOk_finalcancel_and_finalinvite_are_received : When_WaitCancelOk_Base
    {
        protected override void When()
        {
            /*send 481 to invite*/
        }

        [Test]
        public void Expect_the_InternalState_to_transition_to_Idle()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetIdle());
        }

        [Test]
        public void Expect_the_callstate_to_be_Completed()
        {
            _callState.Should().Be(CallState.Completed);
        }
    }
}