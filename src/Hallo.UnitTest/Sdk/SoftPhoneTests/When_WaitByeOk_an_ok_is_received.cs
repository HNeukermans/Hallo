using System;
using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitByeOk_an_ok_is_received : When_WaitByeOk_Base
    {
        protected ManualResetEvent _waitForOkByeProcessed = new ManualResetEvent(false);

        protected override void When()
        {
            var ok =_receivedBye.CreateResponse(SipResponseCodes.x200_Ok);
            _network.SendTo(SipFormatter.FormatMessage(ok), _testClientUaEndPoint, _phoneUaEndPoint);
            _waitForOkByeProcessed.WaitOne(TimeSpan.FromSeconds(3));
        }
        
        protected override void AfterPhoneProcessedResponse(IInternalSoftPhone softPhone, SipResponseEvent responseEvent)
        {
            if (responseEvent.Response.StatusLine.StatusCode == 200 && responseEvent.Response.CSeq.Command == SipMethods.Bye)
            {
                _waitForOkByeProcessed.Set();
            }
        }

        [Test]
        public void Expect_the_phone_to_transition_to_idle()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetIdle());
        }
    }
}