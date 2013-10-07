using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip.Stack.Transactions;

namespace Hallo.Sip.Stack
{
    public class TimerFactory : ITimerFactory
    {
        #region private methods
        
        private static TxTimer CreatePeriodicTimer(Action callBack, int interval)
        {
            return new TxTimer(callBack, interval, true);
        }

        private static TxTimer CreateDelayedAction(Action callBack, int delay)
        {
            return new TxTimer(callBack, delay, false);
        }

        #endregion

        #region non-invite client
        
        public ITimer CreateNonInviteCtxRetransmitTimer(Action callBack)
        {
            return CreatePeriodicTimer(callBack, SipConstants.T1);
        }

        public ITimer CreateNonInviteCtxTimeOutTimer(Action callBack)
        {
            return CreateDelayedAction(callBack, 64 * SipConstants.T1);
        }
        
        public ITimer CreateNonInviteCtxEndCompletedTimer(Action callBack)
        {
            return CreateDelayedAction(callBack, SipConstants.T4);
        }

        #endregion

        #region invite client

        public ITimer CreateInviteCtxRetransmitTimer(Action callBack)
        {
            return CreatePeriodicTimer(callBack, SipConstants.T1);
        }

        public ITimer CreateInviteCtxTimeOutTimer(Action callBack)
        {
            return CreateDelayedAction(callBack, 64 * SipConstants.T1);
        }

        public ITimer CreateInviteCtxEndCompletedTimer(Action callBack)
        {
            return CreateDelayedAction(callBack, 64 * SipConstants.T1);
        }

        #endregion
        
        #region non-invite server

        public ITimer CreateNonInviteStxEndCompletedTimer(Action callBack)
        {
            return CreateDelayedAction(callBack, 64 * SipConstants.T1);
        }

        #endregion
        
        public ITimer CreateSingleFireTimer(int miliseconds, Action callBack)
        {
            return CreateDelayedAction(callBack, miliseconds);
        }

        #region invite server

        public ITimer CreateInviteStxRetransmitTimer(Action callBack)
        {
            return CreatePeriodicTimer(callBack, SipConstants.T1);
        }

        public ITimer CreateInviteStxEndCompletedTimer(Action callBack)
        {
            return CreateDelayedAction(callBack, 64 * SipConstants.T1);
        }

        public ITimer CreateInviteStxEndConfirmed(Action callBack)
        {
            return CreateDelayedAction(callBack, SipConstants.T4);
        }

        #endregion


        public ITimer CreateInviteStxSendTryingTimer(Action callBack)
        {
            return CreateDelayedAction(callBack, 200);
        }

        public ITimer CreateRingingTimer(Action callBack)
        {
            return CreatePeriodicTimer(callBack, 5000);
        }
    }
}
