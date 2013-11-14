using System;
using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Util;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitFinal_a_x200_final_response_is_received : When_WaitFinal_Base
    {
        protected ManualResetEvent _waitingforOkProcessed = new ManualResetEvent(false);
        
        protected override void AfterPhoneProcessedResponse(IInternalSoftPhone softPhone, Hallo.Sip.Stack.SipResponseEvent responseEvent)
        {
            if (responseEvent.Response.StatusLine.StatusCode == 180)
            {
                _ringingProcessed.Set();
            }
            if (responseEvent.Response.StatusLine.StatusCode == 200)
            {
                _waitingforOkProcessed.Set();
            }
       } 

        protected override void When()
        {
            var provResponse = CreateOkResponse(_receivedInvite, _toTag);
            _network.SendTo(SipFormatter.FormatMessage(provResponse), _testClientUaEndPoint, _phoneUaEndPoint);
            _waitingforOkProcessed.WaitOne();
       } 

        [Test]
        public void Expect_the_phone_to_transition_to_Established_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetEstablished());
        }
    }
}