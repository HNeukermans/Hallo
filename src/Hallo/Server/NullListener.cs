using Hallo.Sip.Stack;

namespace Hallo.Server
{
    public class NullListener : ISipListener
    {
        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
            
        }

        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {
            
        }
    }
}