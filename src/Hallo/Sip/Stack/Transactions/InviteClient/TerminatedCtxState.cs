namespace Hallo.Sip.Stack.Transactions.InviteClient
{
    public partial class SipInviteClientTransaction
    {
        internal class TerminatedCtxState : AbstractCtxState
        {
            public override SipTransactionStateName Name
            {
                get { return SipTransactionStateName.Terminated; }
            }

            internal override void Initialize(SipInviteClientTransaction transaction)
            {
                
            }

            internal override void Retransmit(SipInviteClientTransaction transaction)
            {
                throw new System.NotImplementedException();
            }

            internal override StateResult HandleResponse(SipInviteClientTransaction ctx, SipResponse response)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}