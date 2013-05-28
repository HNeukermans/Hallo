using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip.Stack.Transactions.InviteServer
{

    public partial class SipInviteServerTransaction
    {
        internal class CompletedStxState : AbstractStxState
        {
            public override SipTransactionStateName Name
            {
                get { return SipTransactionStateName.Completed; }
            }

            internal override void Initialize(SipInviteServerTransaction tx)
            {
                tx.SendTryingTimer.Dispose();
                tx.ReTransmitNonx200FinalResponseTimer.Start();
                tx.EndCompletedTimer.Start();
            }

            internal override StateResult ProcessRequest(SipInviteServerTransaction tx, SipRequestEvent requestEvent)
            {
                /* 17.2.1 If an ACK is received while the server transaction is in the
                   "Completed" state, the server transaction MUST transition to the
                   "Confirmed" state. */
                if (requestEvent.Request.RequestLine.Method == SipMethods.Ack)
                {
                    tx.ChangeState(SipInviteServerTransaction.ConfirmedState);
                }
                else
                {
                    tx.SendResponseInternal();
                }

                return new StateResult();
            }

            internal override StateResult HandleSendingResponse(SipInviteServerTransaction transaction,
                                                                SipResponse response)
            {
                //in this state the uas is not suposed to sent anymore
                throw new InvalidOperationException();
            }

            public void RetransmitNonx200FinalResponse(SipInviteServerTransaction tx)
            {
                tx.ReTransmitNonx200FinalResponseTimer.Interval =
                    Math.Min(2*tx.ReTransmitNonx200FinalResponseTimer.Interval, SipConstants.T2);
                tx.SendResponseInternal();
            }
        }
    }
}
