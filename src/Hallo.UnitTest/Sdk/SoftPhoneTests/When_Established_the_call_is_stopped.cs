using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_Established_the_call_is_stopped : When_Established_Base
    {
        protected ManualResetEvent _waitForByeReceived = new ManualResetEvent(false);
        private SipRequest _receivedByeRequest;

        protected override void When()
        {
            _incomingCall.Stop();
            _waitForByeReceived.WaitOne(3000);
        }

        protected override void OnTestClientUaReceive(SipContext sipContext)
        {
            if (sipContext.Response != null)
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
            else if (sipContext.Request != null)
            {
                if (sipContext.Request.CSeq.Command == SipMethods.Bye)
                {
                    _receivedByeRequest = sipContext.Request;
                    _waitForByeReceived.Set();
                } 
            }
        }

        [Test]
        public void Expect_the_phone_to_transition_to_waitbyeok_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetWaitByeOk());
        }

        [Test]
        public void Expect_the_testclientua_to_have_received_a_bye_request()
        {
            _receivedByeRequest.Should().NotBeNull();
        }


        
    }
}