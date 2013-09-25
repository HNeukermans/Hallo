using Hallo.Sip.Stack.Transactions;

namespace Hallo.Sip.Stack
{
    public class SipServerTxTimeOutEvent : SipTimeOutEvent
    {
        public ISipServerTransaction ServerTransaction { get; set; }
    }
}