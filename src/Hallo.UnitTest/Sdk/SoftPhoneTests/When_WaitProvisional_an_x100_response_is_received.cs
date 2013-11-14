using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Util;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitProvisional_an_x100_response_is_received : When_WaitProvisional_Base
    {
        protected ManualResetEvent _ringingProcessed = new ManualResetEvent(false);
        
        protected override void When()
        {
            _toTag = SipUtil.CreateTag();
            var provResponse = CreateRingingResponse(_receivedInvite, _toTag);
            _network.SendTo(SipFormatter.FormatMessage(provResponse), _testClientUaEndPoint, _phoneUaEndPoint);
            _ringingProcessed.WaitOne();
        }

        protected override void AfterPhoneProcessedResponse(IInternalSoftPhone softPhone, Hallo.Sip.Stack.SipResponseEvent responseEvent)
        {
            if (responseEvent.Response.StatusLine.StatusCode == 180)
            {
                _ringingProcessed.Set();
            }
        }
        
        [Test]
        public void Expect_the_phone_to_transition_to_WaitFinal_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetWaitFinal());
        }
    }
}