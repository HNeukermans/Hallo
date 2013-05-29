using System;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerDialogTests
{
    public class When_in_confirmed_state_the_retransmitok_timer_fired_1_times : SipInviteServerDialogSpecificationBase
    {
        private readonly AutoResetEvent _waitHandle = new AutoResetEvent(false);
        private SipResponse _okResponse;
        private int _originalInterval = 500;

        public When_in_confirmed_state_the_retransmitok_timer_fired_1_times()
        {
            var tfb = new TimerFactoryStubBuilder();
            tfb.WithInviteCtxRetransmitTimerInterceptor(OnCreateRetransmitOkTimer);
            TimerFactory = tfb.Build();
        }

        protected virtual ITimer OnCreateRetransmitOkTimer(Action action)
        {
            RetransitOkTimer = new TxTimerStub(action, _originalInterval, true, AfterRetransmitFired);
            return RetransitOkTimer;
        }

        protected override void GivenOverride()
        {
            /*force it to go into confirmed state*/
            ServerDialog.SetLastResponse(CreateRingingResponse());
            _okResponse = CreateOkResponse();
            ServerDialog.SetLastResponse(_okResponse);
            ServerDialog.State.Should().Be(DialogState.Confirmed); /*required assertion*/
        }

        private void AfterRetransmitFired()
        {
            _waitHandle.Set();
        }

        protected override void When()
        {
            _waitHandle.WaitOne(TimeSpan.FromSeconds(5));
        }
        
        [Test]
        public void Expect_the_ok_response_to_be_sent()
        {
            Sender.Verify((s) => s.SendResponse(_okResponse), Times.Once());
        }

        [Test]
        public void Expect_the_RetransitTimer_interval_to_be_doubled()
        {
            RetransitOkTimer.Interval.Should().Be(_originalInterval * 2);
        }
    }
}