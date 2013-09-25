using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip.Headers;
using System.Diagnostics.Contracts;
using Hallo.Util;

namespace Hallo.Sip
{
    public class SipMessageFactory
    {
        public SipMessageFactory()
        {

        }

        internal SipRequest CreateRequest(SipRequestLine requestLine)
        {
            var request = new SipRequest { RequestLine = requestLine };
            return request;
        }

        public SipResponse CreateResponse(SipRequest request, string responseCode)
        {
            return request.CreateResponse(responseCode);
        }

        public SipRequest CreateRequest(SipUri requestUri, string method, SipCallIdHeader callId, SipCSeqHeader cSeq, SipFromHeader from, SipToHeader to, SipViaHeader viaHeader, SipMaxForwardsHeader maxForwards) 
        {
            Check.Require(requestUri, "requestUri");
             Check.Require(method, "method");
             Check.Require(callId, "callId");
             Check.Require(cSeq, "cseq");
             Check.Require(from, "from");
             Check.Require(to , "to");
             Check.Require( viaHeader,"viaHeader");
             Check.Require(maxForwards,  "maxforwards");

            var message = new SipRequest();
            message.RequestLine.Method = method;
            message.RequestLine.Uri = requestUri;
            message.RequestLine.Version = SipConstants.SipTwoZeroString;
            message.CallId = callId;
            message.To = to;
            message.From = from;
            message.CSeq = cSeq;
            message.Vias.SetTopMost(viaHeader);
            message.MaxForwards = maxForwards;
            return message;
        }

        internal SipResponse CreateResponse(SipStatusLine statusLine)
        {
            var response = new SipResponse() { StatusLine = statusLine };
            return response;
        }
    }
}
