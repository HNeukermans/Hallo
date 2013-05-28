using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip;
using Hallo.Util;

namespace Hallo.Parsers
{
    public class SipUriParser : SipParser<SipUri>
    {
        public SipUriParser() : base() { }

        

        public override SipUri Parse(StringReader r)
        {
            SipUri result = new SipUri();

            string scheme = r.QuotedReadToDelimiter(':').ToLower();

            if (scheme != "sip")
            {
                throw new SipParseException(ExceptionMessage.InvalidFormat);
            }

            // Get username
            if (r.SourceString.IndexOf('@') > -1)
            {
                result.User = r.QuotedReadToDelimiter('@');
            }

            // Gets host[:port]
            string[] host_port = r.QuotedReadToDelimiter(new char[] { ';' }, false).Split(':');
            result.Host = host_port[0];
            
            // Optional port specified
            if (host_port.Length == 2)
            {
                result.Port = Convert.ToInt32(host_port[1]);
            }


            string readToEnd = r.ReadToEnd();

            if (!string.IsNullOrWhiteSpace(readToEnd))
            {
                readToEnd = readToEnd.Trim();

                Action action = () =>
                {
                    result.Parameters = new SipParameterCollectionParser().Parse(readToEnd);
                };

                TryActionElseThrowParseExceptionInvalidFormat(action);
            }

            //TODO: we do not support SipUri parameters.

            return result;
        }
    }
}
