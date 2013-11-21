using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitCancelOk_finalcancel_and_finalinvite_are_received : When_WaitCancelOk_Base
    {
        protected ManualResetEvent _waitforOkCancelProcessed = new ManualResetEvent(false);
        protected ManualResetEvent _waitforFinalInviteProcessed = new ManualResetEvent(false);

        protected override void When()
        {
            var response = _receivedInvite.CreateResponse(SipResponseCodes.x487_Request_Terminated);
            _network.SendTo(SipFormatter.FormatMessage(response), _testClientUaEndPoint, _phoneUaEndPoint);
            _waitforFinalInviteProcessed.WaitOne();
            var ok = _receivedCancel.CreateResponse(SipResponseCodes.x200_Ok);
            _network.SendTo(SipFormatter.FormatMessage(ok), _testClientUaEndPoint, _phoneUaEndPoint);
            _waitforOkCancelProcessed.WaitOne();
        }

        protected override void AfterPhoneProcessedResponse(IInternalSoftPhone softPhone, Hallo.Sip.Stack.SipResponseEvent responseEvent)
        {
            if (responseEvent.Response.StatusLine.StatusCode == 180)
            {
                _ringingProcessed.Set();
            }

            if (responseEvent.Response.StatusLine.StatusCode == 487 && responseEvent.Response.CSeq.Command == SipMethods.Invite)
            {
                _waitforFinalInviteProcessed.Set();
            }

            if (responseEvent.Response.StatusLine.StatusCode == 200 && responseEvent.Response.CSeq.Command == SipMethods.Cancel)
            {
                _waitforOkCancelProcessed.Set();
            }
        }

        [Test]
        public void Expect_the_InternalState_to_transition_to_Idle()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetIdle());
        }

        [Test]
        public void Expect_the_callstate_to_be_Cancelled()
        {
            _callState.Should().Be(CallState.Cancelled);
        }
    }
}