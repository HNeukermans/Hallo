namespace Hallo.Sdk
{
    public enum CallError
    {
        WaitForAckTimeOut,
        UnHandeldException,
        TransactionTimeout,
        SipResponse
    }

    public class CallErrorObject
    {
        public CallError Type { get; set; }
        public string Message { get; set; }
    }
}