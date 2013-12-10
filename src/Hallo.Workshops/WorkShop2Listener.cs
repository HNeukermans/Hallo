using System;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.Workshops
{
    public class WorkShop2Listener : ISipListener
    {
        private readonly ISipProvider _receiverProvider;

        public WorkShop2Listener(ISipProvider receiverProvider)
        {
            _receiverProvider = receiverProvider;
        }

        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {

        }

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            Console.WriteLine("Received '{0}' request", requestEvent.Request.CSeq.Command);

            var okResponse = requestEvent.Request.CreateResponse(SipResponseCodes.x200_Ok);

            var serverTransaction = _receiverProvider.CreateServerTransaction(requestEvent.Request);
            serverTransaction.SendResponse(okResponse);
            
            /*instruct the sipprovider that we have already sent the response*/
            requestEvent.IsSent = true;
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
            Console.WriteLine("Received '{0}' response, from '{1}' request", responseEvent.Response.StatusLine.ResponseCode, responseEvent.Response.CSeq.Command);
        }
    }

    public class WorkShop22Listener : ISipListener
    {
        private readonly ISipProvider _receiverProvider;

        public WorkShop22Listener(ISipProvider receiverProvider)
        {
            _receiverProvider = receiverProvider;
        }

        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {

        }

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
           
            var okResponse = requestEvent.Request.CreateResponse(SipResponseCodes.x200_Ok);

            var serverTransaction = _receiverProvider.CreateServerTransaction(requestEvent.Request);
            serverTransaction.SendResponse(okResponse);

            /*instruct the sipprovider that we have already sent the response*/
            requestEvent.IsSent = true;
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
        }
    }
}