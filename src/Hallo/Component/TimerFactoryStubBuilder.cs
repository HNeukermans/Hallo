using Hallo.Sip.Stack;
using System;
namespace Hallo.Component
{
    public class TimerFactoryStubBuilder 
    {
        public TimerFactoryStubBuilder()
        {
            _nonInviteCtxRetransmitTimerInterceptor = CreateDoNothingTimer;
            _nonInviteCtxEndCompletedTimerInterceptor = CreateDoNothingTimer;
            _nonInviteCtxTimeOutTimerInterceptor = CreateDoNothingTimer;
            _nonInviteStxEndCompletedTimerInterceptor = CreateDoNothingTimer;
            _inviteStxEndCompletedInterceptor = CreateDoNothingTimer;
            _inviteStxRetransmitTimerInterceptor = CreateDoNothingTimer;
            _inviteStxSendTryingTimerInterceptor = CreateDoNothingTimer;
            _inviteStxEndConfirmedInterceptor = CreateDoNothingTimer;
            _inviteCtxRetransmitTimerInterceptor = CreateDoNothingTimer;
            _inviteCtxEndCompletedTimerInterceptor = CreateDoNothingTimer;
            _inviteCtxTimeOutTimerInterceptor = CreateDoNothingTimer;
        }

        private static ITimer CreateDoNothingTimer(Action callback)
        {
            return new TxTimerStubBuilder().WithCallback(callback).Build();
        }

        Func<Action, ITimer> _nonInviteCtxRetransmitTimerInterceptor;

        public TimerFactoryStubBuilder WithNonInviteCtxRetransmitTimerInterceptor(Func<Action, ITimer> value)
        {
            _nonInviteCtxRetransmitTimerInterceptor = value;
            return this;
        }

        Func<Action, ITimer> _nonInviteCtxEndCompletedTimerInterceptor;

        public TimerFactoryStubBuilder WithNonInviteCtxEndCompletedTimerInterceptor(Func<Action, ITimer> value)
        {
            _nonInviteCtxEndCompletedTimerInterceptor = value;
            return this;
        }

        Func<Action, ITimer> _inviteStxSendTryingTimerInterceptor;

        public TimerFactoryStubBuilder WithInviteStxSendTryingTimerInterceptor(Func<Action, ITimer> value)
        {
            _inviteStxSendTryingTimerInterceptor = value;
            return this;
        }

        Func<Action, ITimer> _inviteStxEndConfirmedInterceptor;

        public TimerFactoryStubBuilder WithInviteStxEndConfirmedTimerInterceptor(Func<Action, ITimer> value)
        {
            _inviteStxEndConfirmedInterceptor = value;
            return this;
        }

        Func<Action, ITimer> _inviteStxRetransmitTimerInterceptor;

        public TimerFactoryStubBuilder WithInviteStxRetransmitTimerInterceptor(Func<Action, ITimer> value)
        {
            _inviteStxRetransmitTimerInterceptor = value;
            return this;
        }

        Func<Action, ITimer> _inviteStxEndCompletedInterceptor;

        public TimerFactoryStubBuilder WithInviteStxEndCompletedInterceptor(Func<Action, ITimer> value)
        {
            _inviteStxEndCompletedInterceptor = value;
            return this;
        }

        Func<Action, ITimer> _nonInviteCtxTimeOutTimerInterceptor;

        public TimerFactoryStubBuilder WithNonInviteCtxTimeOutTimerInterceptor(Func<Action, ITimer> value)
        {
            _nonInviteCtxTimeOutTimerInterceptor = value;
            return this;
        }

        Func<Action, ITimer> _nonInviteStxEndCompletedTimerInterceptor;

        public TimerFactoryStubBuilder WithNonInviteStxEndCompletedTimerInterceptor(Func<Action, ITimer> value)
        {
            _nonInviteStxEndCompletedTimerInterceptor = value;
            return this;
        }

        public TimerFactoryStub Build()
        {
            var tf = new TimerFactoryStub();
            tf.CreateNonInviteCtxRetransmitTimerInterceptor = _nonInviteCtxRetransmitTimerInterceptor;
            tf.CreateNonInviteCtxEndCompletedTimerInterceptor = _nonInviteCtxEndCompletedTimerInterceptor;
            tf.CreateNonInviteCtxTimeOutTimerInterceptor = _nonInviteCtxTimeOutTimerInterceptor;

            tf.CreateNonInviteStxEndCompletedTimerInterceptor = _nonInviteStxEndCompletedTimerInterceptor;

            tf.CreateInviteStxSendTryingInterceptor = _inviteStxSendTryingTimerInterceptor;
            tf.CreateInviteStxEndCompletedTimerInterceptor = _inviteStxEndCompletedInterceptor;
            tf.CreateInviteStxRetransmitTimerInterceptor = _inviteStxRetransmitTimerInterceptor;
            tf.CreateInviteStxEndConfirmedInterceptor = _inviteStxEndConfirmedInterceptor;

            tf.CreateInviteCtxTimeOutTimerInterceptor = _inviteCtxTimeOutTimerInterceptor;
            tf.CreateInviteCtxRetransmitTimerInterceptor = _inviteCtxRetransmitTimerInterceptor;
            tf.CreateInviteCtxEndCompletedTimerInterceptor = _inviteCtxEndCompletedTimerInterceptor;
            return tf;
        }

        Func<Action, ITimer> _inviteCtxTimeOutTimerInterceptor;

        public TimerFactoryStubBuilder WithInviteCtxTimeOutTimerInterceptor(Func<Action, ITimer> value)
        {
            _inviteCtxTimeOutTimerInterceptor = value;
            return this;
        }

        Func<Action, ITimer> _inviteCtxRetransmitTimerInterceptor;

        public TimerFactoryStubBuilder WithInviteCtxRetransmitTimerInterceptor(Func<Action, ITimer> value)
        {
            _inviteCtxRetransmitTimerInterceptor = value;
            return this;
        }

        Func<Action, ITimer> _inviteCtxEndCompletedTimerInterceptor;

        public TimerFactoryStubBuilder WithInviteCtxEndCompletedTimerInterceptor(Func<Action, ITimer> value)
        {
            _inviteCtxEndCompletedTimerInterceptor = value;
            return this;
        }
    }
}