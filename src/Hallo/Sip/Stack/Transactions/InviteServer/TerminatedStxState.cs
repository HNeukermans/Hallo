namespace Hallo.Sip.Stack.Transactions.InviteServer
{
    public partial class SipInviteServerTransaction
    {
        internal class TerminatedStxState : AbstractStxState
        {
            public override SipTransactionStateName Name
            {
                get { return SipTransactionStateName.Terminated; }
            }

            internal override void Initialize(SipInviteServerTransaction transaction)
            {
                throw new System.NotImplementedException();
            }

            internal override StateResult ProcessRequest(SipInviteServerTransaction transaction, SipRequestEvent request)
            {
                throw new System.NotImplementedException();
            }

            internal override StateResult HandleSendingResponse(SipInviteServerTransaction transaction,
                                                                SipResponse response)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}