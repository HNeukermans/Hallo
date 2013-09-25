namespace Hallo.Sip.Stack
{
    public interface ISipRequestProcessor
    {
        void ProcessRequest(SipRequestEvent requestEvent);
    }
}