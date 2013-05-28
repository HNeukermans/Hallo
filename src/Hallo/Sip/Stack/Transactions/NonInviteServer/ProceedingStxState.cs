using System;

namespace Hallo.Sip.Stack.Transactions.NonInviteServer
{
    public partial class SipNonInviteServerTransaction
    {
        internal class ProceedingStxState : AbstractStxState
    {
        internal override void Initialize(SipNonInviteServerTransaction transaction)
        {

        }

        internal override StateResult ProcessRequest(SipNonInviteServerTransaction tx, SipRequestEvent request)
        {
            tx.SendResponseInternal();
            return new StateResult();
        }

        internal override void HandleSendingResponse(SipNonInviteServerTransaction tx, SipResponse response)
        {
            var statusCode = response.StatusLine.StatusCode;

            if (statusCode >= 100 && statusCode < 200)
            {
                tx.SendResponseInternal(response);
            }
            else if (statusCode >= 200 && statusCode < 700)
            {
                tx.SendResponseInternal(response);
                tx.ChangeState(SipNonInviteServerTransaction.CompletedState);
            }
        }

        public override SipTransactionStateName Name
        {
            get { return SipTransactionStateName.Proceeding; }
        }

    }
    }

}