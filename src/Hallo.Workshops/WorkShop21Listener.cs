using System;
using Hallo.Sip.Stack;

namespace Hallo.Workshops
{
    public class WorkShop21Listener : ISipListener
    {
        public WorkShop21Listener()
        {

        }

        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {
            Console.WriteLine("Received timeout for request '{0}' on {1}", timeOutEvent.Request.CSeq.Command, DateTime.Now.ToString("hh:mm:ss"));
        }

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            Console.WriteLine("Received '{0}' request on {1}", requestEvent.Request.CSeq.Command, DateTime.Now.ToString("hh:mm:ss"));

            requestEvent.IsSent = true;
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
            Console.WriteLine("Received '{0}' response, from '{1}' request", responseEvent.Response.StatusLine.ResponseCode, responseEvent.Response.CSeq.Command);
        }
    }
}