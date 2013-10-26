using System;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.UnitTest.Helpers;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_Established_a_bye_is_received : SoftPhoneSpecificationBase
    {
        SipResponse _receivedRingingResponse;
        SipResponse _receivedOkByeResponse;
        protected ManualResetEvent _waitForRinging = new ManualResetEvent(false);
        protected ManualResetEvent _waitForAck = new ManualResetEvent(false);
        protected ManualResetEvent _waitForAckReceived = new ManualResetEvent(false);
        protected ManualResetEvent _waitForByeReceived = new ManualResetEvent(false);

        public When_Established_a_bye_is_received()
        {
            _timerFactory = new TimerFactoryStubBuilder()
                .WithInviteCtxTimeOutTimerInterceptor(OnWaitForAckTimer)
                .Build();

            var mock = new Mock<SoftPhone>();
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
            /*immediately accept = go to waitfor ack state*/
            e.Item.Accept();
        }

        protected override void AfterProcessRequest(IInternalSoftPhone softPhone, SipRequestEvent requestEvent)
        {
            if (requestEvent.Request.RequestLine.Method == SipMethods.Ack)
            {
                _waitForAckReceived.Set();
            }
            if (requestEvent.Request.RequestLine.Method == SipMethods.Bye)
            {
                _waitForByeReceived.Set();
            }
        }

        protected override void GivenOverride()
        {
            _network.SendTo(SipFormatter.FormatMessage(_invite), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);
            _waitForRinging.WaitOne();
            _waitForAck.WaitOne();

            _receivedRingingResponse.Should().NotBeNull(); /*required assertion*/

            var ack = CreateAckRequest(_invite, _receivedRingingResponse);
            _network.SendTo(SipFormatter.FormatMessage(ack), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);

            _waitForAckReceived.WaitOne();
        }

        protected override void When()
        {
            var bye = CreateByeRequest(_invite, _receivedRingingResponse);
            _network.SendTo(SipFormatter.FormatMessage(bye), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);

            _waitForByeReceived.WaitOne();
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
                if (_receivedRingingResponse == null)
                {
                    _receivedRingingResponse = sipContext.Response;
                }
            }
            if (sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x200_Ok && sipContext.Response.CSeq.Command == SipMethods.Bye)
            {
                if (_receivedOkByeResponse == null)
                {
                    _receivedOkByeResponse = sipContext.Response;
                }
            }
        }

        [Test]
        public void Expect_the_phone_to_transition_to_idle_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetIdle());
        }

        [Test]
        public void Expect_the_dialog_to_be_removed_from_the_table()
        {
            _sipProvider1.DialogTable.Count.Should().Be(0);
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