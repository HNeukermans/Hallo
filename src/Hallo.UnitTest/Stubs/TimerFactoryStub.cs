using System;
using Hallo.Sip.Stack;

namespace Hallo.UnitTest.Stubs
{
    ///// <summary>
    ///// TimerFactory stub. For every method it implements, it has a Func property that allow a custom routine to be followed.
    ///// <remarks>
    ///// This stub can be used in cases mocking doesn't fit. For example when you won't to proxy the timer callback.
    ///// </remarks>
    ///// </summary>
    ////public class TimerFactoryStub : ITimerFactory
    ////{
    ////    public TimerFactoryStub()
    ////    {
    ////    }

    ////    #region non invite client

    ////    public ITimer CreateNonInviteCtxRetransmitTimer(Action callBack)
    ////    {
    ////        return CreateNonInviteCtxRetransmitTimerInterceptor(callBack);
    ////    }

    ////    public ITimer CreateNonInviteCtxTimeOutTimer(Action callBack)
    ////    {
    ////        return CreateNonInviteCtxTimeOutTimerInterceptor(callBack);
    ////    }

    ////    public ITimer CreateNonInviteCtxEndCompletedTimer(Action callBack)
    ////    {
    ////        return CreateNonInviteCtxEndCompletedTimerInterceptor(callBack);
    ////    }

    ////    public Func<Action, ITimer> CreateNonInviteCtxRetransmitTimerInterceptor { get; set; }
    ////    public Func<Action, ITimer> CreateNonInviteCtxEndCompletedTimerInterceptor { get; set; }
    ////    public Func<Action, ITimer> CreateNonInviteCtxTimeOutTimerInterceptor { get; set; }

    ////    #endregion

    ////    #region non invite server

    ////    public Func<Action, ITimer> CreateNonInviteStxEndCompletedTimerInterceptor { get; set; }

    ////    public ITimer CreateNonInviteStxEndCompletedTimer(Action callBack)
    ////    {
    ////        return CreateNonInviteStxEndCompletedTimerInterceptor(callBack);
    ////    }

    ////    #endregion

    ////    #region invite client

    ////    public ITimer CreateInviteCtxEndCompletedTimer(Action onCompletedEnded)
    ////    {
    ////        return CreateInviteCtxEndCompletedTimerInterceptor(onCompletedEnded);
    ////    }

    ////    public ITimer CreateInviteCtxTimeOutTimer(Action onTimeOut)
    ////    {
    ////        return CreateInviteCtxTimeOutTimerInterceptor(onTimeOut);
    ////    }

    ////    public ITimer CreateInviteCtxRetransmitTimer(Action onReTransmit)
    ////    {
    ////        return CreateInviteCtxRetransmitTimerInterceptor(onReTransmit);
    ////    }

    ////    public Func<Action, ITimer> CreateInviteCtxRetransmitTimerInterceptor { get; set; }
    ////    public Func<Action, ITimer> CreateInviteCtxEndCompletedTimerInterceptor { get; set; }
    ////    public Func<Action, ITimer> CreateInviteCtxTimeOutTimerInterceptor { get; set; }
        
    ////    #endregion
        
    ////    public ITimer CreateSingleFireTimer(int miliseconds, Action onCompletedEnded)
    ////    {
    ////        throw new NotImplementedException();
    ////    }
        
    ////    #region invite server
        
    ////    public ITimer CreateInviteStxRetransmitTimer(Action callback)
    ////    {
    ////        return CreateInviteStxRetransmitTimerInterceptor(callback);
    ////    }

    ////    public ITimer CreateInviteStxEndCompletedTimer(Action callback)
    ////    {
    ////        return CreateInviteStxEndCompletedTimerInterceptor(callback);
    ////    }

    ////    public ITimer CreateInviteStxEndConfirmed(Action callback)
    ////    {
    ////        return CreateInviteStxEndConfirmedInterceptor(callback);
    ////    }

    ////    public ITimer CreateInviteStxSendTryingTimer(Action callback)
    ////    {
    ////        return CreateInviteStxSendTryingInterceptor(callback);
    ////    }

    ////    public Func<Action, ITimer> CreateInviteStxRetransmitTimerInterceptor { get; set; }
    ////    public Func<Action, ITimer> CreateInviteStxEndCompletedTimerInterceptor { get; set; }
    ////    public Func<Action, ITimer> CreateInviteStxEndConfirmedInterceptor { get; set; }
    ////    public Func<Action, ITimer> CreateInviteStxSendTryingInterceptor { get; set; }
        
    ////    #endregion


    ////}
}