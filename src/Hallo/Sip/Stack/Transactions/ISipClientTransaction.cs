namespace Hallo.Sip.Stack.Transactions
{
    public interface ISipClientTransaction
    {
        void Dispose(); //TODO //really ??
        void SendRequest();
        SipRequest Request { get;  }
        SipTransactionType Type { get;  } //really ??
        string GetId();
    }
}