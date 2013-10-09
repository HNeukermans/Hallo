using System;
using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Util;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;
using Hallo.Component;
using System.Linq;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{      
    internal class When_ringing : SoftPhoneSpecificationBase
    {
        SipResponse _receivedRingingResponse;

        public When_ringing() 
        {
            _timerFactory = new TimerFactoryStubBuilder().WithRingingTimerInterceptor(OnCreateRingingTimer).Build();
        }
        
        protected override void _calleePhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {
            
        }

        protected override void _calleePhone_StateChanged(object sender, VoipEventArgs<SoftPhoneState> e)
        {
            if (e.Item == SoftPhoneState.Ringing) _wait.Set();
        }

        
        protected override void GivenOverride()
        {
            _network.SendTo(SipFormatter.FormatMessage(_invite), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);
            _wait.WaitOne(TimeSpan.FromSeconds(3));
            //_wait.WaitOne();

            _calleePhone.InternalState.Should().Be(_stateProvider.GetRinging()); /*required assertion*/
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
                if (_receivedRingingResponse == null) _receivedRingingResponse = sipContext.Response;                
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


        protected override void _calleePhone_InternalStateChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }


    
}