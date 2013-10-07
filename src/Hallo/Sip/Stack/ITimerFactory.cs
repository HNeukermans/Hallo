using System;

namespace Hallo.Sip.Stack
{
    public interface ITimerFactory
    {
        // <summary>
        /// Creates the retransmit request timer for the non-invite ctx
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        ITimer CreateNonInviteCtxRetransmitTimer(Action callBack);

        /// <summary>
        /// Creates the timeout timer for the non-invite ctx
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        ITimer CreateNonInviteCtxTimeOutTimer(Action callBack);

        /// <summary>
        /// Creates the 'Completed' state timer for the non-invite ctx
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        ITimer CreateNonInviteCtxEndCompletedTimer(Action callBack);

        /*non-invite server Tx*/
        ITimer CreateNonInviteStxEndCompletedTimer(Action callBack);

        ITimer CreateSingleFireTimer(int miliseconds, Action onCompletedEnded);

        /// <summary>
        /// Creates the retransmit request timer for the invite ctx
        /// </summary>
        ITimer CreateInviteCtxRetransmitTimer(Action onReTransmit);

        /// <summary>
        /// Creates the timeout timer for the invite ctx
        /// </summary>
        ITimer CreateInviteCtxTimeOutTimer(Action onTimeOut);

        /// <summary>
        /// Creates the 'Completed' state timer for the invite ctx
        /// </summary>
        ITimer CreateInviteCtxEndCompletedTimer(Action onCompletedEnded);

        /// <summary>
        /// Creates the retransmit timer for the 'Completed' state timer for the invite stx
        /// </summary>
        ITimer CreateInviteStxRetransmitTimer(Action onReTransmit);

        /// <summary>
        /// Creates the 'Completed' state timer for the invite stx
        /// </summary>
        ITimer CreateInviteStxEndCompletedTimer(Action onCompletedEnded);

        /// <summary>
        /// Creates the 'Confirmed' state timer for the invite stx
        /// </summary>
        ITimer CreateInviteStxEndConfirmed(Action onConfirmedEnded);


        ITimer CreateInviteStxSendTryingTimer(Action onSendTryingTimer);

        ITimer CreateRingingTimer(Action callBack);
    }
}