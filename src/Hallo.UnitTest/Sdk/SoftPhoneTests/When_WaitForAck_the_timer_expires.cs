using System;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitForAck_the_timer_expires : SoftPhoneSpecificationBase
    {
        SipResponse _receivedRingingResponse;
        protected ManualResetEvent _waitForRinging = new ManualResetEvent(false);
        protected ManualResetEvent _waitForAck = new ManualResetEvent(false);

        public When_WaitForAck_the_timer_expires()
        {
            _waitForTimeOut = new ManualResetEvent(false);
            _timerFactory = new TimerFactoryStubBuilder()
                .WithRingingTimerInterceptor(OnCreateRingingTimer)
                .WithInviteCtxTimeOutTimerInterceptor(OnWaitForAckTimer).Build();
        }

        protected override void _calleePhone_InternalStateChanged(object sender, EventArgs e)
        {
            if (_phone.InternalState == _stateProvider.GetRinging())
            {
                _waitForRinging.Set();
            }

            if (_phone.InternalState == _stateProvider.GetWaitForAck())
            {
                _waitForAck.Set();
            }
        }

        protected override void _calleePhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {
            /*immediately accept*/
            _phoneCall = e.Item;
            _phoneCall.CallErrorOccured += (s, ev) => _callError = ev.Item;
            _phoneCall.Accept(); ;
        }

        protected override void GivenOverride()
        {
            _network.SendTo(SipFormatter.FormatMessage(_invite), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);
            _waitForRinging.WaitOne();
           
        }

        protected virtual ITimer OnCreateRingingTimer(Action action)
        {
            _ringingTimer = new TxTimerStub(action, 200, true, null);
            return _ringingTimer;
        }

        protected virtual ITimer OnWaitForAckTimer(Action action)
        {
            _waitforAckTimer = new TxTimerStub(action, 200, false, AfterCallBack);
            return _waitforAckTimer;
        }

        private void AfterCallBack()
        {
            _waitForTimeOut.Set();
        }

        protected override void OnReceive(SipContext sipContext)
        {
            if (sipContext.Response != null && sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x180_Ringing)
            {
                if (_receivedRingingResponse == null)
                {
                    _receivedRingingResponse = sipContext.Response;
                }
            }
            if (sipContext.Request != null && sipContext.Request.RequestLine.Method == SipMethods.Bye)
            {
                _receivedByeRequest = sipContext.Request;
            }
        }

        protected override void When()
        {
            _waitForTimeOut.WaitOne(TimeSpan.FromSeconds(3));
        }

        [Test]
        public void Expect_the_call_error_to_have_fired()
        {
            _callError.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_call_error_to_be_WaitForAckTimeOut()
        {
            _callError.Should().Be(CallError.WaitForAckTimeOut);
        }

        [Test]
        public void Expect_to_have_received_a_bye_request_()
        {
            _receivedByeRequest.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_phone_to_transition_to_idle_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetIdle());
        }
      
        private TxTimerStub _ringingTimer;
        private TxTimerStub _waitforAckTimer;
        private IPhoneCall _phoneCall;
        private ManualResetEvent _waitForTimeOut;
        private CallError? _callError;
        private SipRequest _receivedByeRequest
            ;
    }
}