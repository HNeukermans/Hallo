using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Util;

namespace Hallo
{
    public class SipValidator : ISipValidator
    {
        
        public SipValidator()
        {
            
        }

        public ValidateMessageResult ValidateMessage(SipMessage message)
        {

            //testing commit
            if(message is SipRequest)
            {
                var request = message as SipRequest;
                var result = new ValidateRequestResult();
                result.MissingRequiredHeader = FindFirstMissingRequiredHeader(message);
                result.HasUnSupportedSipVersion = !ValidateSipVersion(request.RequestLine.Version);
                if (result.HasRequiredHeadersMissing) return result;
                result.HasInvalidSCeqMethod = !ValidateCSeqMethod(request);
                result.InviteHasNoContactHeader = !ValidateContactHeader(request);
                return result;
            }
            else
            {
                var response = message as SipResponse;
                var result = new ValidateMessageResult();
                result.MissingRequiredHeader = FindFirstMissingRequiredHeader(message);
                result.HasUnSupportedSipVersion = !ValidateSipVersion(response.StatusLine.Version);
                return result;
            }
        }

        /// <summary>
        /// validates that invite requests have a contact header
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool ValidateContactHeader(SipRequest request)
        {
           if(request.RequestLine.Method != SipMethods.Invite) return true;
           if (request.Contacts.GetTopMost() == null) return false;
           return true;
        }

        public ValidateRequestResult ValidateRequest(SipRequest request)
        {
            var result = new ValidateRequestResult();
            if (!SipMethods.IsMethod(request.RequestLine.Method))
            {
                result.UnSupportedSipMethod = request.RequestLine.Method;
            }
            result.MissingRequiredHeader = FindFirstMissingRequiredHeader(request);
            result.HasUnSupportedSipVersion = !ValidateSipVersion(request.RequestLine.Version);
            if (result.HasRequiredHeadersMissing) return result;
            result.HasInvalidSCeqMethod = !ValidateCSeqMethod(request);
            return result;
        }

        private bool ValidateCSeqMethod(SipRequest sipMessage)
        {
            /*8.1.1.5: The cseq command MUST match method of the request.*/
            return sipMessage.RequestLine.Method.Equals(sipMessage.CSeq.Command, StringComparison.CurrentCultureIgnoreCase);
        }

        private bool ValidateSipVersion(string sipVersion)
        {
            return sipVersion.Equals(SipConstants.SipTwoZeroString);
        }

        private string FindFirstMissingRequiredHeader(SipMessage sipMessage)
        {
            if (sipMessage.CallId == null) return SipHeaderNames.CallId;
            if (sipMessage.CSeq == null) return SipHeaderNames.CSeq;
            if (sipMessage.MaxForwards == null) return SipHeaderNames.MaxForwards;
            if (sipMessage.To == null) return SipHeaderNames.To;
            if (sipMessage.From == null) return SipHeaderNames.From;
            if (sipMessage.Vias == null || sipMessage.Vias.Count == 0) return SipHeaderNames.Via;
            return null;
        }
    }
}
