using System;

namespace Hallo.Sip.Stack.Transactions.NonInviteServer
{
    public partial class SipNonInviteServerTransaction
    {
        internal class CompletedStxState : AbstractStxState
        {
            internal override void Initialize(SipNonInviteServerTransaction ctx)
            {
                ctx.EndCompletedTimer.Start();
            }

            internal override StateResult ProcessRequest(SipNonInviteServerTransaction ctx, SipRequestEvent request)
            {
                /*17.2.2. : While in the "Completed" state, the server transaction MUST pass the final response to the transport
                layer for retransmission whenever a retransmission of the request is received. */
                ctx.SendResponseInternal();
                return new StateResult();
            }

            internal override void HandleSendingResponse(SipNonInviteServerTransaction transaction, SipResponse response)
            {
                /*Final responses passed by the TU to the server transaction MUST be discarded while in the "Completed" state.*/
            }

            public override SipTransactionStateName Name
            {
                get { return SipTransactionStateName.Completed; }
            }
        }
    }
}