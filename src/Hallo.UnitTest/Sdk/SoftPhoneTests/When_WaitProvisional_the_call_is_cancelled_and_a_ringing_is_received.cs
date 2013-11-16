using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Util;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitProvisional_the_call_is_cancelled_and_a_ringing_is_received : When_WaitProvisional_Base
    {
        protected ManualResetEvent _waitingforRingingProcessed = new ManualResetEvent(false);
        private SipRequest _receivedCancel;

        protected override void When()
        {
            _call.Stop();
            var ringing = CreateRingingResponse(_receivedInvite, SipUtil.CreateTag());
            _network.SendTo(SipFormatter.FormatMessage(ringing), _testClientUaEndPoint, _phoneUaEndPoint);
            _waitingforRingingProcessed.WaitOne();
        }

        protected override void OnTestClientUaReceive(SipContext sipContext)
        {
            if (sipContext.Request.RequestLine.Method == SipMethods.Invite)
            {
                _receivedInvite = sipContext.Request;
                _waitingforInviteReceived.Set();
            }
            if (sipContext.Request.RequestLine.Method == SipMethods.Cancel)
            {
                _receivedCancel = sipContext.Request;
            }
        }

        protected override void AfterPhoneProcessedResponse(IInternalSoftPhone softPhone, Hallo.Sip.Stack.SipResponseEvent responseEvent)
        {
            if (responseEvent.Response.StatusLine.StatusCode == 180)
            {
                _waitingforRingingProcessed.Set();
            }
        }

        [Test]
        public void Expect_the_phone_to_transition_to_WaitForCancelOk()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetWaitCancelOk());
        }

        [Test]
        public void Expect_the_CancelOnWaitFinal_to_be_False()
        {
            _phone.PendingInvite.CancelOnWaitFinal.Should().BeFalse();
        }

        [Test]
        public void Expect_a_testclient_to_have_received_a_cancel_request()
        {
            _receivedCancel.Should().NotBeNull();
        }
    }
}