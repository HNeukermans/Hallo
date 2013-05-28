namespace Hallo.Sip.Stack.Transactions.InviteClient
{
    public partial class SipInviteClientTransaction
    {
        internal class CompletedCtxState : AbstractCtxState
        {
            internal override void Initialize(SipInviteClientTransaction ctx)
            {
                //dispose timer
                ctx.ReTransmitTimer.Dispose();

                //dispose timer
                ctx.TimeOutTimer.Dispose();

                //start the timer
                ctx.EndCompletedTimer.Start();
            }

            internal override StateResult HandleResponse(SipInviteClientTransaction ctx, SipResponse response)
            {
                var statusCode = response.StatusLine.StatusCode;

                if (statusCode >= 300 && statusCode < 700)
                {
                    ctx.SendAck();
                }

                return new StateResult() {};
            }

            internal override void Retransmit(SipInviteClientTransaction ctx)
            {
                //do nothing
            }

            public override SipTransactionStateName Name
            {
                get { return SipTransactionStateName.Completed; }
            }
        }
    }
}