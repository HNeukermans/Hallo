using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip.Stack.Transactions.InviteClient
{
    public partial class SipInviteClientTransaction
    {
        internal class ProceedingCtxState : AbstractCtxState
        {
            internal override void Initialize(SipInviteClientTransaction ctx)
            {
                ctx.TimeOutTimer.Dispose();
                ctx.ReTransmitTimer.Dispose();
            }

            internal override StateResult HandleResponse(SipInviteClientTransaction ctx, SipResponse response)
            {
                var statusCode = response.StatusLine.StatusCode;

                if (statusCode >= 100 && statusCode < 200)
                {
                    return new StateResult() {InformToUser = true};
                }
                if (statusCode >= 200 && statusCode < 300)
                {
                    ctx.ChangeState(SipInviteClientTransaction.TerminatedState);
                    return new StateResult() {InformToUser = true, Dispose = true};
                }
                if (statusCode >= 300 && statusCode < 700)
                {
                    ctx.ChangeState(SipInviteClientTransaction.CompletedState);
                    ctx.SendAck();
                    return new StateResult() {InformToUser = true};
                }

                return new StateResult();
            }

            internal override void Retransmit(SipInviteClientTransaction ctx)
            {
                //do nothing
            }

            public override SipTransactionStateName Name
            {
                get { return SipTransactionStateName.Proceeding; }
            }
        }
    }
}
