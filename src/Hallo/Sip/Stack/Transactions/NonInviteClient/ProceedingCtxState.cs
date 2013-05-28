using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip.Stack.Transactions.NonInviteClient
{
    internal class ProceedingCtxState : AbstractCtxState
    {
        
        internal override void Initialize(SipNonInviteClientTransaction transaction)
        {
            
        }


        internal override StateResult HandleResponse(SipNonInviteClientTransaction ctx, SipResponse response)
        {
            var statusCode = response.StatusLine.StatusCode;
            
            if (statusCode >= 100 && statusCode < 200)
            {
                return new StateResult(){ InformToUser = true };
            }
            if (statusCode >= 200 && statusCode < 700)
            {
                ctx.ChangeState(SipNonInviteClientTransaction.CompletedState);
                return new StateResult() { InformToUser = true };
            }

            return new StateResult();
        }

        internal override void Retransmit(SipNonInviteClientTransaction ctx)
        {
            ctx.ReTransmitTimer.Interval = SipConstants.T2;

            ctx.SendRequestInternal();
        }

        public override SipTransactionStateName Name
        {
            get { return SipTransactionStateName.Proceeding; }
        }
    }
}
