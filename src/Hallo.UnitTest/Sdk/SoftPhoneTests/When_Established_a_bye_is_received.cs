using System;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.UnitTest.Helpers;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_Established_a_bye_is_received : When_Established_Base
    {
        protected ManualResetEvent _waitForByeReceived = new ManualResetEvent(false);
        private SipResponse _receivedOkByeResponse;
        
        protected override void When()
        {
            var bye = CreateByeRequest(_invite, _receivedRingingResponse);
            _network.SendTo(SipFormatter.FormatMessage(bye), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);

            _waitForByeReceived.WaitOne();
        }
        
        protected override void OnTestClientUaReceive(SipContext sipContext)
        {
            if (sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x180_Ringing)
            {
                _receivedRingingResponse = sipContext.Response;
            }
            if (sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x200_Ok)
            {
                _waitingforOkReceived.Set();
            }
            if (sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x200_Ok && sipContext.Response.CSeq.Command == SipMethods.Bye)
            {
               _receivedOkByeResponse = sipContext.Response;
                _waitForByeReceived.Set();
            }
        }
        
        [Test]
        public void Expect_the_phone_to_transition_to_idle_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetIdle());
        }

        [Test]
        public void Expect_the_dialog_to_be_removed_from_the_table()
        {
            _sipProvider1.DialogTable.Count.Should().Be(0);
        }

        [Test]
        public void Expect_the_phone_to_have_replied_to_bye_with_ok_response()
        {
            _receivedOkByeResponse.Should().NotBeNull();
        }
       

    }

}