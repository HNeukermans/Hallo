using System;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitByeOk_Base : SoftPhoneSpecificationBase
    {
        protected SipResponse _receivedRingingResponse;
        protected ManualResetEvent _waitForAckProcessed = new ManualResetEvent(false);
        protected ManualResetEvent _waitForByeReceived = new ManualResetEvent(false);
        protected SipRequest _receivedBye;

        public When_WaitByeOk_Base()
        {
           
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

         protected override void AfterPhoneProcessedResponse(IInternalSoftPhone softPhone, Hallo.Sip.Stack.SipResponseEvent responseEvent)
         {
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
            _incomingCall.Stop();
            _phone.InternalState.Should().Be(_stateProvider.GetWaitByeOk()); /*required assertion*/
            _waitForByeReceived.WaitOne();
        }

        protected override void When()
        {

        }
        
        protected override void OnTestClientUaReceive(SipContext sipContext)
        {
            if (sipContext.Response != null && sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x180_Ringing)
            {
                _receivedRingingResponse = sipContext.Response;
            }

            if (sipContext.Response != null && sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x200_Ok)
            {
                _waitingforOkReceived.Set();
            }

            if (sipContext.Request != null && sipContext.Request.RequestLine.Method == SipMethods.Bye)
            {
                _receivedBye = sipContext.Request;
                _waitForByeReceived.Set();
            }
            
        }
    }
}