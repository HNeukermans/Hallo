using System;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitForAck_Base : SoftPhoneSpecificationBase
    {
        protected SipResponse _okResponse;
        protected SipResponse _receivedRingingResponse;
        protected ManualResetEvent _waitForRinging = new ManualResetEvent(false);
        protected ManualResetEvent _waitForAck = new ManualResetEvent(false);
        protected TxTimerStub _ringingTimer;
        protected TxTimerStub _waitforAckTimer;
        protected CallErrorObject _callError;

        public When_WaitForAck_Base()
        {
            _timerFactory = new TimerFactoryStubBuilder()
                .WithRingingTimerInterceptor(OnCreateRingingTimer)
                .WithInviteCtxTimeOutTimerInterceptor(OnCreateWaitForAckTimer).Build();
        }

        protected override void AfterPhoneProcessedRequest(IInternalSoftPhone softPhone, SipRequestEvent requestEvent)
        {
            if (requestEvent.Request.CSeq.Command == SipMethods.Invite)
            {
                _waitingforInviteProcessed.Set();
            }
        }

        protected override void _calleePhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {
            _incomingCall = e.Item;
            _incomingCall.CallStateChanged += (s, e1) => _callState = e1.Item;
            _incomingCall.CallErrorOccured += (s, e2) => _callError = e2.Item;
            
        }

        protected override void GivenOverride()
        {
            _network.SendTo(SipFormatter.FormatMessage(_invite), _testClientUaEndPoint, _phoneUaEndPoint);
            _waitingforInviteProcessed.WaitOne();
            _phone.InternalState.Should().Be(_stateProvider.GetRinging()); /*required assertion*/
            _incomingCall.Accept();
            _waitingforOkReceived.WaitOne();
            _phone.InternalState.Should().Be(_stateProvider.GetWaitForAck()); /*required assertion*/
        }

        protected virtual ITimer OnCreateRingingTimer(Action action)
        {
            _ringingTimer = new TxTimerStub(action, 200, true, null);
            return _ringingTimer;
        }

        protected virtual ITimer OnCreateWaitForAckTimer(Action action)
        {
            _waitforAckTimer = new TxTimerStub(action, int.MaxValue, false, null);
            return _waitforAckTimer;
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
        }

        protected override void When()
        {

        }
      
    }
}