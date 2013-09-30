using System;

namespace Hallo.Sip.Stack.Transactions.InviteServer
{
    public partial class SipInviteServerTransaction
    {
        internal class ProceedingStxState : AbstractStxState
        {
            internal override void Initialize(SipInviteServerTransaction tx)
            {
                tx.SendTryingTimer.Start();
            }

            internal override StateResult ProcessRequest(SipInviteServerTransaction tx, SipRequestEvent requestEvent)
            {
                tx.SendResponseInternal();
                requestEvent.IsSent = true;
                return new StateResult();
            }

            internal override StateResult HandleSendingResponse(SipInviteServerTransaction tx, SipResponse response)
            {
                var stateResult = new StateResult();

                var statusCode = response.StatusLine.StatusCode;

                if (statusCode >= 100 && statusCode < 200)
                {
                    tx.SendResponseInternal(response);
                }
                else if (statusCode >= 200 && statusCode < 300)
                {
                    tx.SendResponseInternal(response);
                    stateResult.Dispose = true;
                }
                else if (statusCode >= 300 && statusCode < 700)
                {
                    tx.SendResponseInternal(response);
                    tx.ChangeState(SipInviteServerTransaction.CompletedState);
                }

                return stateResult;
            }

            public override SipTransactionStateName Name
            {
                get { return SipTransactionStateName.Proceeding; }
            }
        }
    }
}