using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Util;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitFinal_a_non200_final_response_is_received : When_WaitFinal_Base
    {
        protected ManualResetEvent _waitingforResponseProcessed = new ManualResetEvent(false);
        
        protected override void AfterPhoneProcessedResponse(IInternalSoftPhone softPhone, Hallo.Sip.Stack.SipResponseEvent responseEvent)
        {
            if (responseEvent.Response.StatusLine.StatusCode == 180)
            {
                _ringingProcessed.Set();
            }
            if (responseEvent.Response.StatusLine.StatusCode == 486)
            {
                _waitingforResponseProcessed.Set();
            }
        }

        protected override void When()
        {
            //_toTag = SipUtil.CreateTag();
            var provResponse = CreateResponse(_receivedInvite, _toTag, SipResponseCodes.x486_Busy_Here);
            _network.SendTo(SipFormatter.FormatMessage(provResponse), _testClientUaEndPoint, _phoneUaEndPoint);
            _waitingforResponseProcessed.WaitOne();
        }

        [Test]
        public void Expect_the_phone_to_remain_in_WaitFinal_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetWaitFinal());
        }

        [Test]
        public void Expect_the_phonecall_state_to_have_changed_to_BusyHere()
        {
            _callState.Should().Be(CallState.BusyHere);
        }
    }
}