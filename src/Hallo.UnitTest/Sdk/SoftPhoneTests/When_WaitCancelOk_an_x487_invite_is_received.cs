using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.InviteClient;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitCancelOk_an_x487_invite_is_received : When_WaitCancelOk_Base
    {
        protected ManualResetEvent _waitforProcessed = new ManualResetEvent(false);

        protected override void When()
        {
            var response = _receivedInvite.CreateResponse(SipResponseCodes.x487_Request_Terminated);
            _network.SendTo(SipFormatter.FormatMessage(response), _testClientUaEndPoint, _phoneUaEndPoint);
            _waitforProcessed.WaitOne();
        }

        protected override void AfterPhoneProcessedResponse(IInternalSoftPhone softPhone, Hallo.Sip.Stack.SipResponseEvent responseEvent)
        {
            if (responseEvent.Response.StatusLine.StatusCode == 180)
            {
                _ringingProcessed.Set();
            }

            if (responseEvent.Response.StatusLine.StatusCode == 487 && responseEvent.Response.CSeq.Command == SipMethods.Invite)
            {
                _waitforProcessed.Set();
            }
        }

        [Test]
        public void Expect_the_InternalState_to_remain_in_WaitCancelOk()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetWaitCancelOk());
        }

        [Test]
        public void Expect_DialogTable_to_have_0_dialog()
        {
            _sipProvider1.DialogTable.Count.Should().Be(0);
        }

        [Test]
        public void Expect_InviteClientTransaction_to_be_in_completed_state()
        {
            _phone.PendingInvite.InviteClientTransaction.State.Should().Be(SipInviteClientTransaction.CompletedState);
        }

        [Test]
        public void Expect_dialog_to_be_Terminated()
        {
            _phone.PendingInvite.Dialog.State.Should().Be(Hallo.Sip.Stack.Dialogs.DialogState.Terminated);
        }

        [Test]
        public void Expect_the_callstate_to_be_Completed()
        {
            _callState.Should().Be(CallState.Completed);
        }
    }
}