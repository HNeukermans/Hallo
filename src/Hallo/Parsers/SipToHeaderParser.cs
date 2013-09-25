using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip.Headers;
using Hallo.Util;
using Hallo.Sip;

namespace Hallo.Parsers
{
    // [ParserFor(SipHeaderNames.From)] TODO
    [ParserFor(SipHeaderNames.To)]
    public class SipToHeaderParser : AbstractParser<SipToHeader>
    {
        public SipToHeaderParser() : base()
        {

        }
        
        public override SipToHeader Parse(StringReader r)
        {
            SipToHeader header = new SipToHeader();

            r.ReadToFirstChar();

            if (r.StartsWith("\""))
            {
                header.DisplayInfo = r.ReadWord();
                r.ReadToFirstChar();
            }

            Action action = () =>
            {
                header.SipUri = new SipUriParser().Parse(r.ReadParenthesized());
            };

            TryActionElseThrowParseExceptionInvalidFormat(action);

            //TODO: also support ip address spec
            string readToEnd = r.ReadToEnd();
            
            if (!string.IsNullOrWhiteSpace(readToEnd))
            {
                readToEnd = readToEnd.Trim();

                action = () =>
                {
                    header.Parameters = new SipParameterCollectionParser().Parse(readToEnd);
                };

                TryActionElseThrowParseExceptionInvalidFormat(action);
            }

            return header;
        }
    }
}
