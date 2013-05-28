using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip.Stack.Transactions.NonInviteServer
{
    public partial class SipNonInviteServerTransaction
    {
        internal class TryingStxState : AbstractStxState
        {
            internal override void Initialize(SipNonInviteServerTransaction transaction)
            {

            }

            internal override StateResult ProcessRequest(SipNonInviteServerTransaction ctx, SipRequestEvent request)
            {
                /*17.2.2.: retransmitted requests in the trying state should be discarded.*/
                return new StateResult();
            }

            internal override void HandleSendingResponse(SipNonInviteServerTransaction ctx, SipResponse response)
            {
                var statusCode = response.StatusLine.StatusCode;

                if (statusCode >= 100 && statusCode < 200)
                {
                    ctx.SendResponseInternal(response);
                    ctx.ChangeState(SipNonInviteServerTransaction.ProceedingState);
                }
                else if (statusCode >= 200 && statusCode < 700)
                {
                    ctx.SendResponseInternal(response);
                    ctx.ChangeState(SipNonInviteServerTransaction.CompletedState);
                }
            }

            public override SipTransactionStateName Name
            {
                get { return SipTransactionStateName.Trying; }
            }
        }
    }
}
