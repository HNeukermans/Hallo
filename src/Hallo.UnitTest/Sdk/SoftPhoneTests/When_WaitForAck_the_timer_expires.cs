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
    internal class When_WaitForAck_the_timer_expires : When_WaitForAck_Base
    {
        private SipRequest _receivedByeRequest;
        private ManualResetEvent _waitForTimeOut;

        public When_WaitForAck_the_timer_expires()
        {
            _waitForTimeOut = new ManualResetEvent(false);
            _timerFactory = new TimerFactoryStubBuilder()
                .WithRingingTimerInterceptor(OnCreateRingingTimer)
                .WithInviteCtxTimeOutTimerInterceptor(OnCreateWaitForAckTimer).Build();
        }

        protected override void When()
        {
            /*wait for the timeout to happen*/
            _waitForTimeOut.WaitOne(TimeSpan.FromSeconds(3));
        }

        protected override void OnTestClientUaReceive(SipContext sipContext)
        {
            if (sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x180_Ringing)
            {
                if (_receivedRingingResponse == null)
                {
                    _receivedRingingResponse = sipContext.Response;
                }
            }
            if (sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x200_Ok)
            {
                _okResponse = sipContext.Response;
                _waitingforOkReceived.Set();
           }
           if (sipContext.Request != null && sipContext.Request.RequestLine.Method == SipMethods.Bye)
           {
               _receivedByeRequest = sipContext.Request;
           }
        }
        

        protected virtual ITimer OnCreateWaitForAckTimer(Action action)
        {
            Action afterCallBack = () => _waitForTimeOut.Set();
            _waitforAckTimer = new TxTimerStub(action, 200, false, afterCallBack);
            return _waitforAckTimer;
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
        [Ignore("This test should not belong to this behavior.")]
        public void Expect_to_have_received_a_bye_request()
        {
            _receivedByeRequest.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_phone_to_transition_to_idle_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetIdle());
        }
       
    }
}