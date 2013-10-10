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
using Moq;
using NUnit.Framework;
using System.Threading;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitForAck_an_ack_is_received : SoftPhoneSpecificationBase
    {
        SipResponse _receivedRingingResponse;
        protected ManualResetEvent _waitForRinging = new ManualResetEvent(false);
        protected ManualResetEvent _waitForAck = new ManualResetEvent(false);
        protected ManualResetEvent _waitForAckReceived = new ManualResetEvent(false);

       

        public When_WaitForAck_an_ack_is_received()
        {
            _timerFactory = new TimerFactoryStubBuilder()
                .WithInviteCtxTimeOutTimerInterceptor(OnWaitForAckTimer)
                .Build();

            var mock = new Mock<SoftPhone>();
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

        protected override void AfterProcessRequest(IInternalSoftPhone softPhone, SipRequestEvent requestEvent)
        {
            if (requestEvent.Request.RequestLine.Method == SipMethods.Ack)
            {
                _waitForAckReceived.Set();
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
            _calleePhone.InternalState.Should().Be(_stateProvider.GetWaitForAck()); /*required assertion*/   

            var ack = CreateAckRequest(_invite, _receivedRingingResponse);
            _network.SendTo(SipFormatter.FormatMessage(ack), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);

            _waitForAckReceived.WaitOne();
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

        //TODO: move to when_established
        [Test]
        public void Expect_WaitForAck_timer_to_be_stopped()
        {
            _waitforAckTimer.IsStarted.Should().BeFalse();
        }

        private TxTimerStub _waitforAckTimer;
    }



}
