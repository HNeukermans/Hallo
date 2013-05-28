using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Component;
using Hallo.Sip.Stack;
using Hallo.UnitTest.Stubs;

namespace Hallo.UnitTest.Builders
{
    //public class TxTimerBuilder : ObjectBuilder<TxTimer>
    //{
    //    Action _callback;

    //    public TxTimerBuilder()
    //    {
    //        _interval = int.MaxValue;
    //        _isPeriodic = false;
    //    }

    //    public TxTimerBuilder WithCallback(Action value)
    //    {
    //        _callback = value;
    //        return this;
    //    }
        
    //    int _interval;

    //    public TxTimerBuilder WithInterval(int value)
    //    {
    //        _interval = value;
    //        return this;
    //    }


    //    bool _isPeriodic;

    //    public TxTimerBuilder WithisPeriodic(bool value)
    //    {
    //        _isPeriodic = value;
    //        return this;
    //    }

    //    public override TxTimer Build()
    //    {
    //        return new TxTimer(_callback, _interval, _isPeriodic);
    //    }
    //}
    
    /// <summary>
    /// builds a TcTimerStub. Defaults to a void timer.
    /// </summary>
    public class TxTimerStubBuilder : ObjectBuilder<TxTimerStub>
    {
        Action _callback;

        public TxTimerStubBuilder()
        {
            _interval = int.MaxValue;
            _isPeriodic = false;
            _afterCallBack = () => { };
        }

        public TxTimerStubBuilder(Action afterCallBack = null):this()
        {
            _afterCallBack = afterCallBack;
        }

        public TxTimerStubBuilder WithCallback(Action value)
        {
            _callback = value;
            return this;
        }

        int _interval;

        public TxTimerStubBuilder WithInterval(int value)
        {
            _interval = value;
            return this;
        }


        bool _isPeriodic;

        public TxTimerStubBuilder WithisPeriodic(bool value)
        {
            _isPeriodic = value;
            return this;
        }


        Action _afterCallBack;

        public TxTimerStubBuilder WithAfterCallBack(Action value)
        {
            _afterCallBack = value;
            return this;
        }

        public override TxTimerStub Build()
        {
            return new TxTimerStub(_callback, _interval, _isPeriodic, _afterCallBack);
        }
    }

    //public class TimerFactoryStubBuilder : ObjectBuilder<TimerFactoryStub>
    //{
    //    public TimerFactoryStubBuilder()
    //    {
    //        _nonInviteCtxRetransmitTimerInterceptor = CreateDoNothingTimer;
    //        _nonInviteCtxEndCompletedTimerInterceptor = CreateDoNothingTimer;
    //        _nonInviteCtxTimeOutTimerInterceptor = CreateDoNothingTimer;
    //        _nonInviteStxEndCompletedTimerInterceptor = CreateDoNothingTimer;
    //        _inviteStxEndCompletedInterceptor = CreateDoNothingTimer;
    //        _inviteStxRetransmitTimerInterceptor = CreateDoNothingTimer;
    //        _inviteStxSendTryingTimerInterceptor = CreateDoNothingTimer;
    //        _inviteStxEndConfirmedInterceptor = CreateDoNothingTimer;
    //    }

    //    private static ITimer CreateDoNothingTimer(Action callback)
    //    {
    //        return new TxTimerStubBuilder().WithCallback(callback).Build();
    //    }

    //    Func<Action,ITimer> _nonInviteCtxRetransmitTimerInterceptor;

    //    public TimerFactoryStubBuilder WithNonInviteCtxRetransmitTimerInterceptor(Func<Action,ITimer> value)
    //    {
    //        _nonInviteCtxRetransmitTimerInterceptor = value;
    //        return this;
    //    }

    //    Func<Action,ITimer> _nonInviteCtxEndCompletedTimerInterceptor;

    //    public TimerFactoryStubBuilder WithNonInviteCtxEndCompletedTimerInterceptor(Func<Action,ITimer> value)
    //    {
    //        _nonInviteCtxEndCompletedTimerInterceptor = value;
    //        return this;
    //    }

    //    Func<Action, ITimer> _inviteStxSendTryingTimerInterceptor;

    //    public TimerFactoryStubBuilder WithInviteStxSendTryingTimerInterceptor(Func<Action, ITimer> value)
    //    {
    //        _inviteStxSendTryingTimerInterceptor = value;
    //        return this;
    //    }

    //    Func<Action, ITimer> _inviteStxEndConfirmedInterceptor;

    //    public TimerFactoryStubBuilder WithInviteStxEndConfirmedTimerInterceptor(Func<Action, ITimer> value)
    //    {
    //        _inviteStxEndConfirmedInterceptor = value;
    //        return this;
    //    }

    //    Func<Action, ITimer> _inviteStxRetransmitTimerInterceptor;

    //    public TimerFactoryStubBuilder WithInviteStxRetransmitTimerInterceptor(Func<Action, ITimer> value)
    //    {
    //        _inviteStxRetransmitTimerInterceptor = value;
    //        return this;
    //    }

    //    Func<Action, ITimer> _inviteStxEndCompletedInterceptor;

    //    public TimerFactoryStubBuilder WithInviteStxEndCompletedInterceptor(Func<Action, ITimer> value)
    //    {
    //        _inviteStxEndCompletedInterceptor = value;
    //        return this;
    //    }

    //    Func<Action, ITimer> _nonInviteCtxTimeOutTimerInterceptor;

    //    public TimerFactoryStubBuilder WithNonInviteCtxTimeOutTimerInterceptor(Func<Action, ITimer> value)
    //    {
    //        _nonInviteCtxTimeOutTimerInterceptor = value;
    //        return this;
    //    }
        
    //    Func<Action,ITimer> _nonInviteStxEndCompletedTimerInterceptor;

    //    public TimerFactoryStubBuilder WithNonInviteStxEndCompletedTimerInterceptor(Func<Action,ITimer> value)
    //    {
    //        _nonInviteStxEndCompletedTimerInterceptor = value;
    //        return this;
    //    }
        
    //    public override TimerFactoryStub Build()
    //    {
    //        var tf = new TimerFactoryStub();
    //        tf.CreateNonInviteCtxRetransmitTimerInterceptor = _nonInviteCtxRetransmitTimerInterceptor;
    //        tf.CreateNonInviteCtxEndCompletedTimerInterceptor = _nonInviteCtxEndCompletedTimerInterceptor;
    //        tf.CreateNonInviteCtxTimeOutTimerInterceptor = _nonInviteCtxTimeOutTimerInterceptor;

    //        tf.CreateNonInviteStxEndCompletedTimerInterceptor = _nonInviteStxEndCompletedTimerInterceptor;

    //        tf.CreateInviteStxSendTryingInterceptor = _inviteStxSendTryingTimerInterceptor;
    //        tf.CreateInviteStxEndCompletedTimerInterceptor = _inviteStxEndCompletedInterceptor;
    //        tf.CreateInviteStxRetransmitTimerInterceptor = _inviteStxRetransmitTimerInterceptor;
    //        tf.CreateInviteStxEndConfirmedInterceptor = _inviteStxEndConfirmedInterceptor;

    //        tf.CreateInviteCtxTimeOutTimerInterceptor = _inviteCtxTimeOutTimerInterceptor;
    //        tf.CreateInviteCtxRetransmitTimerInterceptor = _inviteCtxRetransmitTimerInterceptor;
    //        tf.CreateInviteCtxEndCompletedTimerInterceptor = _inviteCtxEndCompletedTimerInterceptor;
    //        return tf;
    //    }

    //    Func<Action, ITimer> _inviteCtxTimeOutTimerInterceptor;

    //    public TimerFactoryStubBuilder WithInviteCtxTimeOutTimerInterceptor(Func<Action, ITimer> value)
    //    {
    //        _inviteCtxTimeOutTimerInterceptor = value;
    //        return this;
    //    }

    //    Func<Action, ITimer> _inviteCtxRetransmitTimerInterceptor;

    //    public TimerFactoryStubBuilder WithInviteCtxRetransmitTimerInterceptor(Func<Action, ITimer> value)
    //    {
    //        _inviteCtxRetransmitTimerInterceptor = value;
    //        return this;
    //    }

    //    Func<Action, ITimer> _inviteCtxEndCompletedTimerInterceptor;

    //    public TimerFactoryStubBuilder WithInviteCtxEndCompletedTimerInterceptor(Func<Action, ITimer> value)
    //    {
    //        _inviteCtxEndCompletedTimerInterceptor = value;
    //        return this;
    //    }
    //}
}
