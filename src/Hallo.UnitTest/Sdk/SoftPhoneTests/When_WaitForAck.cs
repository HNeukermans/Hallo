using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip;
using Hallo.Component;
using Hallo.Sdk;
using FluentAssertions;
using Hallo.UnitTest.Helpers;
using Hallo.Sip.Stack;
using NUnit.Framework;
using System.Threading;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitForAck_an_ack_is_received : SoftPhoneSpecificationBase
    {
        SipResponse _receivedRingingResponse;
        protected ManualResetEvent _waitForRinging = new ManualResetEvent(false);
        protected ManualResetEvent _waitForAccepted = new ManualResetEvent(false);

        public When_WaitForAck_an_ack_is_received()
        {
            _timerFactory = new TimerFactoryStubBuilder().WithRingingTimerInterceptor(OnCreateRingingTimer).Build();
        }


        protected override void _calleePhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {
            /*immediately accept*/
            e.Item.Accept();
            _waitForAccepted.Set();
        }

        protected override void _calleePhone_StateChanged(object sender, VoipEventArgs<SoftPhoneState> e)
        {
        }

        protected override void GivenOverride()
        {
            _network.SendTo(SipFormatter.FormatMessage(_invite), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);
            _waitForRinging.WaitOne();
            _waitForAccepted.WaitOne();
            //_waitForRinging.WaitOne(TimeSpan.FromSeconds(3));
            //_waitForAccepted.WaitOne(TimeSpan.FromSeconds(3));

            var ack = CreateAckRequest(_invite, _receivedRingingResponse);
            _network.SendTo(SipFormatter.FormatMessage(_invite), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);

            _calleePhone.InternalState.Should().Be(_stateProvider.GetWaitForAck()); /*required assertion*/            
        }

        protected virtual ITimer OnCreateRingingTimer(Action action)
        {
            _ringingTimer = new TxTimerStub(action, 200, true, null);
            return _ringingTimer;
        }

        protected override void OnReceive(SipContext sipContext)
        {
            if (sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x180_Ringing)
            {
                if (_receivedRingingResponse == null) {
                    _waitForRinging.Set();
                    _receivedRingingResponse = sipContext.Response;
                } 
            }
        }

        protected override void When()
        {

        }

        [Test]
        public void Expect_at_least_a_ringing_response_is_sent()
        {
            _receivedRingingResponse.Should().NotBeNull();
        }



        [Test]
        public void Expect_ringing_timer_to_be_started()
        {
            _ringingTimer.IsStarted.Should().BeTrue();
        }

        [Test]
        public void Expect_PendingInvite_not_to_be_null()
        {
            _calleePhone.PendingInvite.Should().NotBeNull();
        }

        [Test]
        public void Expect_InviteTransaction_not_to_be_null()
        {
            _calleePhone.PendingInvite.InviteTransaction.Should().NotBeNull();
        }

        [Test]
        public void Expect_IsIncomingCall_to_be_true()
        {
            _calleePhone.PendingInvite.IsIncomingCall.Should().BeTrue();
        }

        [Test]
        public void Expect_Dailog_not_to_be_null()
        {
            _calleePhone.PendingInvite.Dialog.Should().NotBeNull();
        }

        [Test]
        public void Expect_OriginalRequest_not_to_be_null()
        {
            _calleePhone.PendingInvite.OriginalRequest.Should().NotBeNull();
        }

        [Test]
        public void Expect_RingingResponse_not_to_be_null()
        {
            _calleePhone.PendingInvite.RingingResponse.Should().NotBeNull();
        }

        [Test]
        public void Expect_Provider_DialogTable_to_have_1_dialog()
        {
            _sipProvider1.DialogTable.Count.Should().Be(1);
        }

        [Test]
        public void Expect_dialog_to_be_early()
        {
            _calleePhone.PendingInvite.Dialog.State.Should().Be(Hallo.Sip.Stack.Dialogs.DialogState.Early);
        }


        private TxTimerStub _ringingTimer;

    }

}
