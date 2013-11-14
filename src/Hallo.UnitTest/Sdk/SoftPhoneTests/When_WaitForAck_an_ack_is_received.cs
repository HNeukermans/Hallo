using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sdk.SoftPhoneStates;
using Hallo.Sip;
using Hallo.Component;
using Hallo.Sdk;
using FluentAssertions;
using Hallo.UnitTest.Helpers;
using Hallo.Sip.Stack;
using Moq;
using NUnit.Framework;
using System.Threading;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitForAck_an_ack_is_received : When_WaitForAck_Base
    {
        protected ManualResetEvent _waitForAckProcessed = new ManualResetEvent(false);
       
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
        
        protected override void When()
        {
            var ack = CreateAckRequest(_invite, _okResponse);
            _network.SendTo(SipFormatter.FormatMessage(ack), _testClientUaEndPoint, _phoneUaEndPoint);
            _waitForAckProcessed.WaitOne();
        }

        [Test]
        public void Expect_the_phone_to_transition_to_established_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetEstablished());
        }
    }
}
