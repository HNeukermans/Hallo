using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Util;

namespace Hallo.Sip
{
    public class SipRequest : SipMessage
    {
        public SipRequestLine RequestLine { get; set; }

        public SipRequest()
        {
            RequestLine = new SipRequestLine();
        }
        
        public SipResponse CreateResponse(string responseCode)
        {
            //Contract.Requires(!string.IsNullOrWhiteSpace(responseCode));
            
            if (this.RequestLine.Method == SipMethods.Ack)
            {
                throw new InvalidOperationException(ExceptionMessage.AckRequestCanNotHaveAResponse);
            }

            int wsIndex = responseCode.IndexOf(' ');

            int code = int.Parse(responseCode.Substring(0, wsIndex + 1));
            string reason = responseCode.Substring(wsIndex + 1);


            /* RFC 3261 8.2.6.1.
                When a 100 (Trying) response is generated, any Timestamp header field
                present in the request MUST be copied into this 100 (Trying)
                response.

               RFC 3261 8.2.6.2.
                The From field of the response MUST equal the From header field of
                the request.  The Call-ID header field of the response MUST equal the
                Call-ID header field of the request.  The CSeq header field of the
                response MUST equal the CSeq field of the request.  The Via header
                field values in the response MUST equal the Via header field values
                in the request and MUST maintain the same ordering.

                If a request contained a To tag in the request, the To header field
                in the response MUST equal that of the request.  However, if the To
                header field in the request did not contain a tag, the URI in the To
                header field in the response MUST equal the URI in the To header
                field; additionally, the UAS MUST add a tag to the To header field in
                the response (with the exception of the 100 (Trying) response, in
                which a tag MAY be present).  This serves to identify the UAS that is
                responding, possibly resulting in a component of a dialog ID.  The
                same tag MUST be used for all responses to that request, both final
                and provisional (again excepting the 100 (Trying)).  Procedures for
                the generation of tags are defined in Section 19.3.

               RFC 3261 12.1.1.
                When a UAS responds to a request with a response that establishes a
                dialog (such as a 2xx to INVITE), the UAS MUST copy all Record-Route
                header field values from the request into the response (including the
                URIs, URI parameters, and any Record-Route header field parameters,
                whether they are known or unknown to the UAS) and MUST maintain the
                order of those values.            
            */

            SipResponse response = new SipResponse();
            response.StatusLine.StatusCode = code;
            response.StatusLine.ReasonPhrase = reason;
            response.StatusLine.Version = SipConstants.SipTwoZeroString;

            foreach (var viaHeader in this.Vias)
            {
                response.Vias.Add(CloneHeader(viaHeader));
            }

            response.From = CloneHeader(this.From);
            response.To = CloneHeader(this.To);

            //TODO: write out same tag for all response to this request
            if (this.To != null && this.To.Tag == null)
            {
                response.To.Tag = SipUtil.CreateTag();
            }

            response.CallId = CloneHeader(this.CallId);
            response.CSeq = CloneHeader(this.CSeq);
            response.MaxForwards = CloneHeader(this.MaxForwards);
            
            //TODO: record route 

            return response;
        }

        private T CloneHeader<T>(T header) where T : SipHeader
        {
            //if (header == null) return null;
            return header != null ? (T)header.Clone() : null;
        }

    }
}
