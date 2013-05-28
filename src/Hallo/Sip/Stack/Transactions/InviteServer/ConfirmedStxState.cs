using System;

namespace Hallo.Sip.Stack.Transactions.InviteServer
{
    /* The purpose of the "Confirmed" state is to absorb any additional ACK
       messages that arrive, triggered from retransmissions of the final
       response. */

    public partial class SipInviteServerTransaction
    {
        internal class ConfirmedStxState : AbstractStxState
        {
            public override SipTransactionStateName Name
            {
                get { return SipTransactionStateName.Confirmed; }
            }

            internal override void Initialize(SipInviteServerTransaction tx)
            {
                tx.ReTransmitNonx200FinalResponseTimer.Dispose();
                tx.EndCompletedTimer.Dispose();
                tx.EndConfirmedTimer.Start();
            }

            internal override StateResult ProcessRequest(SipInviteServerTransaction transaction, SipRequestEvent request)
            {
                //absorb ack's
                return new StateResult();
            }

            internal override StateResult HandleSendingResponse(SipInviteServerTransaction transaction,
                                                                SipResponse response)
            {
                throw new InvalidOperationException();
            }
        }
    }
}