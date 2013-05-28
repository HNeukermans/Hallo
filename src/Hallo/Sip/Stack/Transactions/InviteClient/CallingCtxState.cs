using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip.Stack.Dialogs;

namespace Hallo.Sip.Stack.Transactions.InviteClient
{
    public partial class SipInviteClientTransaction
    {
        internal class CallingCtxState : AbstractCtxState
        {
            public override SipTransactionStateName Name
            {
                get { return SipTransactionStateName.Calling; }
            }

            internal override void Initialize(SipInviteClientTransaction ctx)
            {
                ctx.TimeOutTimer.Start();
                ctx.ReTransmitTimer.Start();
            }

            internal override void Retransmit(SipInviteClientTransaction ctx)
            {
                ctx.ReTransmitTimer.Interval = ctx.ReTransmitTimer.Interval*2;

                ctx.SendRequestInternal();
            }

            internal override StateResult HandleResponse(SipInviteClientTransaction ctx, SipResponse response)
            {
                var statusCode = response.StatusLine.StatusCode;

                if (statusCode >= 100 && statusCode < 200)
                {
                    ctx.ChangeState(SipInviteClientTransaction.ProceedingState);
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

                return new StateResult() {};
            }
        }

    }
}
