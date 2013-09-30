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

namespace Hallo.UnitTest.Sdk
{
    public class When_the_phone_is_in_Idle_state : Specification
    {
        private FakeNetwork _network;
        private SipRequest _invite;
        //TODO:test invite no from tag
        //TODO:test invite no contact header

        ManualResetEvent _wait = new ManualResetEvent(false);
        private decimal _counter;
        private ISoftPhone _calleePhone;
        private bool _firedStateChanged;
        private bool _firedIncomingCall;
        private IPhoneCall _incomingCall;
        private SipProvider _sipProvider1;
        
        protected override void Given()
        {
            //create invite.
            _invite = CreateInviteRequest(TestConstants.EndPoint1Uri, TestConstants.EndPoint2Uri);

            //create phone
            var cs1 = new FakeSipContextSource(TestConstants.IpEndPoint2);

            _network = new FakeNetwork();
            _sipProvider1 = new SipProvider(new SipStack(), cs1);
            _calleePhone = new SoftPhone(_sipProvider1, new SipMessageFactory(), new SipHeaderFactory(), new SipAddressFactory(), new SoftPhoneStateProvider());
            cs1.AddToNetwork(_network);
            _network.AddReceiver(TestConstants.IpEndPoint1, OnReceive);
            _calleePhone.IncomingCall += (s, e) =>
            {
                _incomingCall = e.Item;
                _firedIncomingCall = true;
            };
            _calleePhone.StateChanged += _calleePhone_StateChanged;
            _calleePhone.Start();
        }

        void _calleePhone_StateChanged(object sender, VoipEventArgs<SoftPhoneState> e)
        {
            _firedStateChanged = true;
            _wait.Set();
        }

        private void OnReceive(SipContext sipContext)
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
        public void Expect_the_phone_to_be_in_Idle_state()
        {
            _calleePhone.CurrentState.Should().Be(SoftPhoneState.Ringing);
        }
               
        [Test]
        public void Expect_the_provider_to_have_0_tx()
        {
            _sipProvider1.ServerTransactionTable.Count.Should().Be(0);
            _sipProvider1.ClientTransactionTable.Count.Should().Be(0);
        }
               
    }
   
}