namespace Hallo.Sip.Stack.Transactions
{
    public interface ISipClientTransaction : ISipTransaction
    {
        void SendRequest();
    }
}