using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Util;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitFinal_an_x100_response_is_received : When_WaitFinal_Base
    {
        protected ManualResetEvent _callForwardProcessed = new ManualResetEvent(false);
        
        protected override void When()
        {
            SendRingingToPhone();
            _callForwardProcessed.WaitOne();
        }

        private void SendRingingToPhone()
        {
            var provResponse = CreateResponse(_receivedInvite, _toTag, SipResponseCodes.x181_Call_Forwarded);
            _network.SendTo(SipFormatter.FormatMessage(provResponse), _testClientUaEndPoint, _phoneUaEndPoint);
        }
        
        protected override void AfterPhoneProcessedResponse(IInternalSoftPhone softPhone, Hallo.Sip.Stack.SipResponseEvent responseEvent)
        {
            if (responseEvent.Response.StatusLine.StatusCode == 180)
            {
                _ringingProcessed.Set();
            }
            if (responseEvent.Response.StatusLine.StatusCode == 181)
            {
                _callForwardProcessed.Set();
            }
        }

        [Test]
        public void Expect_the_phone_to_remain_in_waitfinal_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetWaitFinal());
        }
    }
}