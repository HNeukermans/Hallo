namespace Hallo.Sip.Stack
{
    public interface ISipResponseProcessor
    {
        void ProcessResponse(SipResponseEvent responseEvent);
    }
}