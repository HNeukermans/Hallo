using System;
using System.Linq;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerDialogTests
{
    public class When_in_early_state : SipInviteServerDialogSpecificationBase
    {
        private SipResponse _ringingResponse;

        public When_in_early_state()
        {
            var tf = new TimerFactoryStubBuilder()
               .WithInviteCtxRetransmitTimerInterceptor(CreateRetransmitOkTimer)
               .WithInviteCtxTimeOutTimerInterceptor(CreateTimeOutTimer).Build();
            TimerFactory = tf;
        }
        
        protected ITimer CreateRetransmitOkTimer(Action action)
        {
            RetransitOkTimer = new TxTimerStub(action, int.MaxValue, false, () => { });
            return RetransitOkTimer;
        }

        protected ITimer CreateTimeOutTimer(Action action)
        {
            TimeOutTimer = new TxTimerStub(action, int.MaxValue, false, () => { });
            return TimeOutTimer;
        }

        protected override void When()
        {
            /*force it to go into early state*/
            _ringingResponse = CreateRingingResponse();
            ServerDialog.SetLastResponse(_ringingResponse);
            ServerDialog.State.Should().Be(DialogState.Early); /*required assertion*/
        }

        [Test]
        public void Expect_the_CallId_to_be_same_as_the_request_CallId()
        {
            ServerDialog.CallId.Should().Be(ReceivedRequest.CallId.Value);
        }
       
        [Test]
        public void Expect_the_Remotetag_to_be_From_tag_of_the_request()
        {
            ServerDialog.RemoteTag.Should().Be(ReceivedRequest.From.Tag);
        }

        [Test]
        public void Expect_the_LocalTag_to_be_ToHeader_Tag_of_the_response()
        {
            ServerDialog.LocalTag.Should().Be(_toTag);
        }

        [Test]
        public void Expect_the_HasAckReceived_to_be_false()
        {
            ServerDialog.HasAckReceived.Should().BeFalse();
        }

        [Test]
        public void Expect_RetransitTimer_not_to_be_started()
        {
            RetransitOkTimer.IsStarted.Should().BeFalse();
        }

        [Test]
        public void Expect_TimeOutTimer_not_to_be_started()
        {
            TimeOutTimer.IsStarted.Should().BeFalse();
        }

    }
}