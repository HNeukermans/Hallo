using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitProvisional_Base : SoftPhoneSpecificationBase
    {
        protected CallError? _callError;
        protected IPhoneCall _call;
        protected CallState? _callState;
        protected SipRequest _receivedInvite;
        protected string _toTag;
        protected ManualResetEvent _waitingforResponseProcessed = new ManualResetEvent(false);
        protected ManualResetEvent _waitingForAckReceived = new ManualResetEvent(false);
        protected SipRequest _receivedAck;

        protected override void GivenOverride()
        {
            _call = _phone.CreateCall();
            _call.CallErrorOccured += (s, e) => _callError = e.Item;
            _call.CallStateChanged += (s, e) => _callState = e.Item;
            _call.Start(_testClientUaUri.FormatToString());
            _waitingforInviteReceived.WaitOne();

            _phone.InternalState.Should().Be(_stateProvider.GetWaitProvisional()); /*required assertion*/
        }

        protected override void OnTestClientUaReceive(SipContext sipContext)
        {
            if (sipContext.Request.RequestLine.Method == SipMethods.Invite)
            {
                _receivedInvite = sipContext.Request;
                _waitingforInviteReceived.Set();
            }
            else if (sipContext.Request.RequestLine.Method == SipMethods.Ack)
            {
                _receivedAck = sipContext.Request;
                _waitingForAckReceived.Set();
            }
        }
    }
}