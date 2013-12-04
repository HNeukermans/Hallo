using System;
using Hallo.Sip.Stack;

namespace Hallo.Workshops
{
    public class SipListener : ISipListener
    {

        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {

        }

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            Console.WriteLine("Received '{0}' message", requestEvent.Request.CSeq.Command);

            /*prevent a exception from being thrown*/
            requestEvent.IsSent = true;
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {

        }
    }
}