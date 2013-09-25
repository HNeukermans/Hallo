using Hallo.Sip.Stack.Transactions;

namespace Hallo.Sip.Stack
{
    public class SipClientTxTimeOutEvent : SipTimeOutEvent
    {
        public ISipClientTransaction ClientTransaction { get; set; }
    }
}