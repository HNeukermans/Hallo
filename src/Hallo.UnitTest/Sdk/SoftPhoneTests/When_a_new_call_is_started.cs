using System;
using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_a_new_call_is_started : SoftPhoneSpecificationBase
    {
        SipUri _toUri;
        private SipRequest _receivedInvite;
        

        protected override void OnTestClientUaReceive(SipContext sipContext)
        {
            if (sipContext.Request.RequestLine.Method == SipMethods.Invite)
            {
                _receivedInvite = sipContext.Request;
                _waitingforInviteReceived.Set();
            }
        }

        protected override void When()
        {
            var call = _phone.CreateCall();
            _toUri = TestConstants.EndPoint1Uri;
            call.Start(_toUri.FormatToString());
            _waitingforInviteReceived.WaitOne();
        }

        [Test]
        public void Expect_the_phone_internalstate_to_transition_to_WaitProvisional_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetWaitProvisional());
        }

        [Test]
        [Ignore("State has moved from softphone to phonecall and phoneline")]
        public void Expect_the_phone_to_transition_to_Waiting_state()
        {
            _phone.CurrentState.Should().Be(SoftPhoneState.Waiting);
        }

        [Test]
        public void Expect_an_invite_request_is_received()
        {
            _receivedInvite.Should().NotBeNull();
        }

        [Test]
        public void Expect_an_invite_to_ToUri_be_sent()
        {
            _receivedInvite.To.SipUri.Equals(_toUri).Should().BeTrue();
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