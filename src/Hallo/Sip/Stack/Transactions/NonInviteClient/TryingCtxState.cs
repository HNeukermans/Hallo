using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip.Stack.Transactions.NonInviteClient
{
    internal  class TryingCtxState : AbstractCtxState
    {
        internal override void Initialize(SipNonInviteClientTransaction transaction)
        {
            //start the timers
            transaction.ReTransmitTimer.Start();
            transaction.TimeOutTimer.Start();
        }

        internal override StateResult HandleResponse(SipNonInviteClientTransaction ctx, SipResponse response)
        {
            var statusCode = response.StatusLine.StatusCode;

            if (statusCode >= 100 && statusCode < 200)
            {
                ctx.ChangeState(SipNonInviteClientTransaction.ProceedingState);
            }
            else if (statusCode >= 200 && statusCode < 700)
            {
                ctx.ChangeState(SipNonInviteClientTransaction.CompletedState);
            }

            return new StateResult() { InformToUser = true };
        }

        internal override void Retransmit(SipNonInviteClientTransaction ctx)
        {
            ctx.ReTransmitTimer.Interval = Math.Min(ctx.ReTransmitTimer.Interval*2, SipConstants.T2);

            ctx.SendRequestInternal();
        }

        public override SipTransactionStateName Name
        {
            get { return SipTransactionStateName.Trying; }
        }
    }
}
