namespace Hallo.Sip.Stack.Transactions.NonInviteClient
{
    internal class CompletedCtxState : AbstractCtxState
    {
        internal override void Initialize(SipNonInviteClientTransaction ctx)
        {
            //start the timer
            ctx.EndCompletedTimer.Start();

            //dispose timers (that were started in the trying state)
            ctx.TimeOutTimer.Dispose();
            ctx.ReTransmitTimer.Dispose();
        }

        internal override StateResult HandleResponse(SipNonInviteClientTransaction ctx, SipResponse response)
        {
            return new StateResult(){ InformToUser = true };
        }

        internal override void Retransmit(SipNonInviteClientTransaction ctx)
        {
            //do nothing
        }

        public override SipTransactionStateName Name
        {
            get { return SipTransactionStateName.Completed; }
        }
    }
}