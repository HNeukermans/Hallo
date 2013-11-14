using System;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sip.Stack.Dialogs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerDialogTests
{
    public class When_in_confirmed_state_the_endwaitforack_timer_has_fired : SipInviteServerDialogSpecificationBase
    {
        private readonly AutoResetEvent _waitHandle = new AutoResetEvent(false);

        public When_in_confirmed_state_the_endwaitforack_timer_has_fired()
        {
            var tfb = new TimerFactoryStubBuilder();
            tfb.WithInviteCtxTimeOutTimerInterceptor((a) => new TxTimerStub(a, SipConstants.T1, false, AfterEndWaitForAck));
            TimerFactory = tfb.Build();
        }

        private void AfterEndWaitForAck()
        {
            _waitHandle.Set();
        }

        protected override void GivenOverride()
        {
            /*force it to go into confirmed state*/
            ServerDialog.SetLastResponse(CreateRingingResponse());
            ServerDialog.SetLastResponse(CreateOkResponse());
            ServerDialog.State.Should().Be(DialogState.Confirmed); /*required assertion*/
        }

        protected override void When()
        {
            _waitHandle.WaitOne(TimeSpan.FromSeconds(3));
        }

        [Test]
        [Ignore("EndWaitForAckTimer is move out of Dialog, and left to the responsibility of the Dialog user.")]
        public void Expect_the_dialog_to_transition_to_Terminated_state()
        {
            ServerDialog.State.Should().Be(DialogState.Terminated);
        }
    }
}