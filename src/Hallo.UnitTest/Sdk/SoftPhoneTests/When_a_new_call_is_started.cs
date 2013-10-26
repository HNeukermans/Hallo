using System;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_a_new_call_is_started : SoftPhoneSpecificationBase
    {
        protected override void OnReceive(SipContext sipContext)
        {
            /*continue test execution*/
            //_wait.Set(); move to statechanged, as this is the last event in code.
        }

        protected override void When()
        {
            var call = _phone.CreateCall();
            call.Start(TestConstants.EndPoint2Uri.FormatToString());
        }

        [Test]
        public void Expect_the_phone_internalstate_to_transition_to_WaitProvisional_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetWaitProvisional());
        }

        [Test]
        public void Expect_the_phone_to_transition_to_Waiting_state()
        {
            _phone.CurrentState.Should().Be(SoftPhoneState.Waiting);
        }

        protected override void _calleePhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {

        }

        protected override void _calleePhone_StateChanged(object sender, VoipEventArgs<SoftPhoneState> e)
        {

        }

        protected override void _calleePhone_InternalStateChanged(object sender, EventArgs e)
        {

        }
    }
}