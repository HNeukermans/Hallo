namespace Hallo.Sip.Stack.Transactions
{
    public class SipTransactionStateInfo
    {
        public SipTransactionStateName CurrentState { get; set; }
        public SipRequest Request { get; set; }
        public SipTransactionType TransactionType { get; set; }
        public string Id { get; set; }
    }
}