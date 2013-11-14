using System;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_Established_Base : SoftPhoneSpecificationBase
    {
        protected TxTimerStub _waitforAckTimer;
        protected SipResponse _receivedRingingResponse;
        protected ManualResetEvent _waitForAckProcessed = new ManualResetEvent(false);

        public When_Established_Base()
        {
            _timerFactory = new TimerFactoryStubBuilder()
                .WithInviteCtxTimeOutTimerInterceptor(OnWaitForAckTimer)
                .Build();
        }

        protected override void _calleePhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {
            _incomingCall = e.Item;
            _incomingCall.CallStateChanged += (s, e2) => _callState = e2.Item;
        }

        protected override void AfterPhoneProcessedRequest(IInternalSoftPhone softPhone, SipRequestEvent requestEvent)
        {
            if (requestEvent.Request.CSeq.Command == SipMethods.Invite)
            {
                _waitingforInviteProcessed.Set();
            }
            if (requestEvent.Request.RequestLine.Method == SipMethods.Ack)
            {
                _waitForAckProcessed.Set();
            }
        }

        protected override void GivenOverride()
        {
            _network.SendTo(SipFormatter.FormatMessage(_invite), _testClientUaEndPoint, _phoneUaEndPoint);
            _waitingforInviteProcessed.WaitOne();
            _phone.InternalState.Should().Be(_stateProvider.GetRinging()); /*required assertion*/
            _incomingCall.Accept();
            _phone.InternalState.Should().Be(_stateProvider.GetWaitForAck()); /*required assertion*/
            _waitingforOkReceived.WaitOne();
            var ack = CreateAckRequest(_invite, _receivedRingingResponse);
            _network.SendTo(SipFormatter.FormatMessage(ack), _testClientUaEndPoint, _phoneUaEndPoint);
            _waitForAckProcessed.WaitOne();
            _phone.InternalState.Should().Be(_stateProvider.GetEstablished()); /*required assertion*/
        }

        protected override void When()
        {

        }

        protected virtual ITimer OnWaitForAckTimer(Action action)
        {
            _waitforAckTimer = new TxTimerStub(action, int.MaxValue, false, null);
            return _waitforAckTimer;
        }

        protected override void OnTestClientUaReceive(SipContext sipContext)
        {
            if (sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x180_Ringing)
            {
                _receivedRingingResponse = sipContext.Response;
            }

            if (sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x200_Ok)
            {
                _waitingforOkReceived.Set();
            }
        }

        

    }
}