namespace Hallo.Sip.Stack.Transactions
{
    public interface ISipClientTransaction : ISipTransaction
    {
        void Dispose(); //TODO //really ??
        void SendRequest();
    }
}