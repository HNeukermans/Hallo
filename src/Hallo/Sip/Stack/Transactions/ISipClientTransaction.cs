namespace Hallo.Sip.Stack.Transactions
{
    public interface ISipClientTransaction
    {
        void Dispose(); //TODO 
        void SendRequest();
        SipRequest Request { get;  }
        SipTransactionType Type { get;  }
    }
}