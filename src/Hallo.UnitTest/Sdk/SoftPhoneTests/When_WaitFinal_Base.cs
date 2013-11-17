using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Util;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitFinal_Base : SoftPhoneSpecificationBase
    {
        protected SipRequest _receivedInvite;
        protected CallState? _callState;
        protected string _toTag;
        protected ManualResetEvent _ringingProcessed = new ManualResetEvent(false);
        protected ManualResetEvent _waitingForAckReceived = new ManualResetEvent(false);
        protected IPhoneCall _call;
        protected SipRequest _receivedAck;

        protected override void GivenOverride()
        {
            _call = _phone.CreateCall();
            _call.CallStateChanged += call_CallStateChanged;
            _call.Start(_testClientUaUri.FormatToString());

            _waitingforInviteReceived.WaitOne();
            _phone.InternalState.Should().Be(_stateProvider.GetWaitProvisional()); /*required assertion*/

            _toTag = SipUtil.CreateTag();
            var provResponse = CreateRingingResponse(_receivedInvite, _toTag);
            _network.SendTo(SipFormatter.FormatMessage(provResponse), _testClientUaEndPoint, _phoneUaEndPoint);
            _ringingProcessed.WaitOne();

            _phone.InternalState.Should().Be(_stateProvider.GetWaitFinal()); /*required assertion*/
        }

        void call_CallStateChanged(object sender, VoipEventArgs<CallState> e)
        {
            _callState = e.Item;
        }

        protected override void When()
        {

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

        protected override void AfterPhoneProcessedResponse(IInternalSoftPhone softPhone, Hallo.Sip.Stack.SipResponseEvent responseEvent)
        {
            if (responseEvent.Response.StatusLine.StatusCode == 180)
            {
                _ringingProcessed.Set();
            }
        }

    }
}