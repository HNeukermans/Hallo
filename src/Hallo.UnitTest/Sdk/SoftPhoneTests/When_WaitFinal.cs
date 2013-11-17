using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.Sip.Util;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitFinal : When_WaitFinal_Base
    {
        protected override void GivenOverride()
        {
            var call = _phone.CreateCall();
            call.CallStateChanged += (s, e) => _callState = e.Item;
            call.CallStateChanged += call_CallStateChanged;
            call.Start(_testClientUaUri.FormatToString());

            _waitingforInviteReceived.WaitOne();
            _phone.InternalState.Should().Be(_stateProvider.GetWaitProvisional()); /*required assertion*/

            _toTag = SipUtil.CreateTag();
            var provResponse = CreateRingingResponse(_receivedInvite, _toTag);
            _network.SendTo(SipFormatter.FormatMessage(provResponse), _testClientUaEndPoint, _phoneUaEndPoint);
            _ringingProcessed.WaitOne();

            _phone.InternalState.Should().Be(_stateProvider.GetWaitFinal()); /*required assertion*/
        }

        void call_CallStateChanged(object sender, VoipEventArgs<CallState> e)
        {
            _callState = e.Item;
        }

        protected override void When()
        {
            
        }

        protected override void OnTestClientUaReceive(SipContext sipContext)
        {
            if (sipContext.Request.RequestLine.Method == SipMethods.Invite)
            {
                _receivedInvite = sipContext.Request;
                _waitingforInviteReceived.Set();
            }
        }

        protected override void AfterPhoneProcessedResponse(IInternalSoftPhone softPhone, Hallo.Sip.Stack.SipResponseEvent responseEvent)
        {
            if (responseEvent.Response.StatusLine.StatusCode == 180)
            {
                _ringingProcessed.Set();
            }
        }
        
        [Test]
        public void Expect_the_callstate_to_be_ringback()
        {
            _callState.Should().Be(CallState.Ringback);
        }

        [Test]
        public void Expect_DialogTable_to_have_1_dialog()
        {
            _sipProvider1.DialogTable.Count.Should().Be(1);
        }

        [Test]
        public void Expect_dialog_to_be_early()
        {
            _phone.PendingInvite.Dialog.State.Should().Be(Hallo.Sip.Stack.Dialogs.DialogState.Early);
        }

        [Test]
        public void Expect_InviteClientTransaction_not_to_be_null()
        {
            _phone.PendingInvite.InviteClientTransaction.Should().NotBeNull();
        }

        [Test]
        public void Expect_InviteClientTransaction_to_be_in_calling_state()
        {
            _phone.PendingInvite.InviteClientTransaction.State.Should().Be(SipInviteClientTransaction.ProceedingState);
        }


        [Test]
        public void Expect_PendingInvite_not_to_be_null()
        {
            _phone.PendingInvite.Should().NotBeNull();
        }

        [Test]
        public void Expect_PendingCall_not_to_be_null()
        {
            _phone.PendingCall.Should().NotBeNull();
        }

    }
}