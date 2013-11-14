using System;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_Ringing_Base : SoftPhoneSpecificationBase
    {
        protected SipResponse _receivedRingingResponse;
        protected TxTimerStub _ringingTimer;

        public When_Ringing_Base()
        {
            _timerFactory = new TimerFactoryStubBuilder().WithRingingTimerInterceptor(OnCreateRingingTimer).Build();
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
                _waitingforInviteReceived.Set();
            }
        }

        protected override void GivenOverride()
        {
            _network.SendTo(SipFormatter.FormatMessage(_invite), _testClientUaEndPoint, _phoneUaEndPoint);
            _waitingforInviteReceived.WaitOne(TimeSpan.FromSeconds(3));
            _phone.InternalState.Should().Be(_stateProvider.GetRinging()); /*required assertion*/
        }

        protected virtual ITimer OnCreateRingingTimer(Action action)
        {
            _ringingTimer = new TxTimerStub(action, 200, true, null);
            return _ringingTimer;
        }

        protected override void OnTestClientUaReceive(SipContext sipContext)
        {
            if (sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x180_Ringing)
            {
                if (_receivedRingingResponse == null) _receivedRingingResponse = sipContext.Response;
            }
        }

        protected override void When()
        {

        }

    }
}