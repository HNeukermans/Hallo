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
    internal class When_Ringing_the_call_is_accepted : SoftPhoneSpecificationBase
    {
        ManualResetEvent _waitForInRinging = new ManualResetEvent(false);
        ManualResetEvent _waitForOkAccepted = new ManualResetEvent(false);

        public When_Ringing_the_call_is_accepted()
        {
            _timerFactory = new TimerFactoryStubBuilder().WithRingingTimerInterceptor(OnCreateRingingTimer).Build();
        }

        protected override void GivenOverride()
        {
             _network.SendTo(SipFormatter.FormatMessage(_invite), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);
            // _waitForInRinging.WaitOne(TimeSpan.FromSeconds(3));
            _waitForInRinging.WaitOne();

            _calleePhone.InternalState.Should().Be(_stateProvider.GetRinging()); /*required assertion*/
        }
        
        protected override void _calleePhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {
            _incomingCall = e.Item; 
        }
               
        protected override void _calleePhone_StateChanged(object sender, VoipEventArgs<SoftPhoneState> e)
        {
            if (e.Item == SoftPhoneState.Ringing) _waitForInRinging.Set();
        }    

        protected override void When()
        {
            _incomingCall.Accept();
            //_waitForOkAccepted.WaitOne(TimeSpan.FromSeconds(3));
            _waitForOkAccepted.WaitOne();
        }

        protected virtual ITimer OnCreateRingingTimer(Action action)
        {
            _ringingTimer = new TxTimerStub(action, 200, true, null);
            return _ringingTimer;
        }

        protected override void OnReceive(SipContext sipContext)
        {
            if (sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x200_Ok)
            {
                _okResponse = sipContext.Response;
                _waitForOkAccepted.Set();
            }
        }        

        [Test]
        public void Expect_ok_response_to_have_received()
        {
            _okResponse.Should().NotBeNull();
        }

        [Test]
        public void Expect_DialogTable_to_have_1_dialog()
        {
            _sipProvider1.DialogTable.Count.Should().Be(1);
        }

        [Test]
        public void Expect_dialog_to_be_confirmed()
        {
            _calleePhone.PendingInvite.Dialog.State.Should().Be(Hallo.Sip.Stack.Dialogs.DialogState.Confirmed);
        }

        [Test]
        public void Expect_the_phone_to_transition_to_waitforack_state()
        {
            _calleePhone.InternalState.Should().Be(_stateProvider.GetWaitForAck());
        }

        private TxTimerStub _ringingTimer;
        private IPhoneCall _incomingCall;
        private SipResponse _okResponse;

    }
  
}
