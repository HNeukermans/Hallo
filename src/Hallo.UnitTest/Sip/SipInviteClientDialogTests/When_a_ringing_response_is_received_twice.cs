using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipClientDialogTests
{
    [TestFixture]
    public class When_a_ringing_response_is_received_twice : SipDialogSpecificationBase
    {
        protected override void BeforeCreateInviteTransaction()
        {
            _provider.AddSipListener(new StubSipListener(onProcessResponse: OnProcessResponse));
        }

        private void OnProcessResponse(SipResponseEvent sipResponseEvent)
        {
            _onProcessResponseCount++;
            _sipResponseEvent = sipResponseEvent;
        }

        protected override void When()
        {
            /*create ringing response*/
            var c = new SipContext();
            _response = CreateRingingResponse();
            c.Response = _response;
            /*fire twice*/
            _contextSource.FireNewContextReceivedEvent(c);
            _contextSource.FireNewContextReceivedEvent(c);
        }
        
        [Test]
        public void Expect_OnProcessResponse_to_be_invoked_twice()
        {
            _onProcessResponseCount.Should().Be(2);
        }
        
        [Test]
        public void Expect_the_dialog_to_be_in_Early_state()
        {
            _sipResponseEvent.ClientTransaction.As<SipInviteClientTransaction>().GetDialog().State.Equals(DialogState.Early);
        }
    }

}