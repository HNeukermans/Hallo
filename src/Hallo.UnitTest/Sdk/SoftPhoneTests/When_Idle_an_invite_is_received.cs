using System;
using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Util;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;
using Hallo.Component;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_Idle_a_invite_is_received : SoftPhoneSpecificationBase
    {

        protected override void _calleePhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {
            _incomingCall = e.Item;
            _firedIncomingCall = true;
        }

        protected override void _calleePhone_StateChanged(object sender, VoipEventArgs<SoftPhoneState> e)
        {
            _firedStateChanged = true;
            _wait.Set();
        }

        protected override void OnTestClientUaReceive(SipContext sipContext)
        {
            /*continue test execution*/
            //_wait.Set(); move to statechanged, as this is the last event in code.
        }

        protected override void When()
        {
            _network.SendTo(SipFormatter.FormatMessage(_invite), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);
            _wait.WaitOne(TimeSpan.FromSeconds(3));
            //_wait.WaitOne();
        }

        [Test]
        [Ignore("State has moved from softphone to phonecall and phoneline")]
        public void Expect_the_phone_to_transition_to_Ringing_state()
        {
            _phone.CurrentState.Should().Be(SoftPhoneState.Ringing);
        }

        [Test]
        public void Expect_the_provider_to_have_1_servertx()
        {
            _sipProvider1.ServerTransactionTable.Count.Should().Be(1);
        }

        [Test]
        public void Expect_the_IncomingCall_event_to_be_raised()
        {
            _firedIncomingCall.Should().BeTrue();
        }

        [Test]
        public void Expect_the_IncomingCall_not_to_be_null()
        {
            _incomingCall.Should().NotBeNull();
        }

        protected SipRequest CreateInviteRequest(SipUri from, SipUri to)
        {
            var r = new SipRequestBuilder()
                .WithRequestLine(
                    new SipRequestLineBuilder().WithMethod(SipMethods.Invite).Build())
                .WithCSeq(
                    new SipCSeqHeaderBuilder().WithCommand(SipMethods.Invite).WithSequence(1).Build())
                .WithFrom(
                    new SipFromHeaderBuilder().WithSipUri(from).WithTag(SipUtil.CreateTag()).Build())
                .WithTo(
                    new SipToHeaderBuilder().WithSipUri(to).WithTag(null).Build())
                .WithCallId(
                    new SipCallIdHeaderBuilder().WithValue(SipUtil.CreateCallId()).Build())
                .WithContacts(
                    new SipContactHeaderListBuilder()
                        .Add(new SipContactHeaderBuilder().WithSipUri(from).Build())
                        .Build())
                .Build();

            return r;
        }



        protected override void _calleePhone_InternalStateChanged(object sender, EventArgs e)
        {
            
        }
    }
   
}