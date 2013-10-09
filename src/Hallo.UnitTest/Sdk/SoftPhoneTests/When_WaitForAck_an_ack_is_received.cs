using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sdk.SoftPhoneStates;
using Hallo.Sip;
using Hallo.Component;
using Hallo.Sdk;
using FluentAssertions;
using Hallo.UnitTest.Helpers;
using Hallo.Sip.Stack;
using NUnit.Framework;
using System.Threading;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitForAck_an_ack_is_received : SoftPhoneSpecificationBase
    {
        SipResponse _receivedRingingResponse;
        protected ManualResetEvent _waitForRinging = new ManualResetEvent(false);
        protected ManualResetEvent _waitForAck = new ManualResetEvent(false);
        protected ManualResetEvent _waitForAck_AckReceived = new ManualResetEvent(false);

       

        public When_WaitForAck_an_ack_is_received()
        {
            _timerFactory = new TimerFactoryStubBuilder()
                .WithInviteCtxTimeOutTimerInterceptor(OnWaitForAckTimer)
                .Build();
        }

        protected override void _calleePhone_InternalStateChanged(object sender, EventArgs e)
        {
            if (_calleePhone.InternalState == _stateProvider.GetRinging())
            {
                _waitForRinging.Set();
            }

            if (_calleePhone.InternalState == _stateProvider.GetWaitForAck())
            {
                _waitForAck.Set();
            }
        }

        protected override void _calleePhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {
            /*immediately accept*/
            e.Item.Accept();
        }

        protected override void AfterProcessRequest(ISoftPhoneState softPhoneState, SipRequestEvent requestEvent)
        {
            if (softPhoneState == _stateProvider.GetWaitForAck() && requestEvent.Request.RequestLine.Method == SipMethods.Ack)
            {
                _waitForAck_AckReceived.Set();
            }
        }
        
        protected override void GivenOverride()
        {
            _network.SendTo(SipFormatter.FormatMessage(_invite), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);
            _waitForRinging.WaitOne();            
            _waitForAck.WaitOne();                                
        }


        protected override void When()
        {
            var ack = CreateAckRequest(_invite, _receivedRingingResponse);
            _network.SendTo(SipFormatter.FormatMessage(ack), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);

            Thread.Sleep(8000);

            _waitForAck_AckReceived.WaitOne();

            _calleePhone.InternalState.Should().Be(_stateProvider.GetWaitForAck()); /*required assertion*/   
        }


        protected virtual ITimer OnWaitForAckTimer(Action action)
        {
            _waitforAckTimer = new TxTimerStub(action, int.MaxValue, false, null);
            return _waitforAckTimer;
        }

        protected override void OnReceive(SipContext sipContext)
        {
            if (sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x180_Ringing)
            {
                if (_receivedRingingResponse == null) {
                    _receivedRingingResponse = sipContext.Response;
                } 
            }
        }


        [Test]
        public void Expect_the_phone_to_transition_to_established_state()
        {
            _calleePhone.InternalState.Should().Be(_stateProvider.GetEstablished());
        }

        [Test]
        public void Expect_at_least_a_ringing_response_is_sent()
        {
            _receivedRingingResponse.Should().NotBeNull();
        }

        [Test]
        public void Expect_ringing_timer_to_be_stopped()
        {
            _ringingTimer.IsStarted.Should().BeFalse();
        }

        [Test]
        public void Expect_WaitForAck_timer_to_be_stopped()
        {
            _waitforAckTimer.IsStarted.Should().BeFalse();
        }

        private TxTimerStub _ringingTimer;
        private TxTimerStub _waitforAckTimer;
    }



}
