using Hallo.Sip.Stack.Transactions;

namespace Hallo.Sip.Stack
{
    public interface ISipRequestProcessor
    {
        void ProcessRequest(SipRequestEvent requestEvent);
    }

   

}