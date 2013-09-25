using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Parsers
{
    //[ParserFor(SipHeaderNames.To)]
    [ParserFor(SipHeaderNames.From)]
    public class SipFromHeaderParser : AbstractParser<SipFromHeader>
    {
        public override SipFromHeader Parse(StringReader r)
        {
            SipFromHeader header = new SipFromHeader();

            r.ReadToFirstChar();

            if (r.StartsWith("\""))
            {
                header.DisplayInfo = r.ReadWord();
                r.ReadToFirstChar();

                //IfNullOrEmptyThrowParseExceptionInvalidFormat(header.DisplayInfo); not required!!
                if (string.IsNullOrEmpty(header.DisplayInfo))
                {
                    IfFalseThrowParseException(header.DisplayInfo.Length <= 50, string.Format(ExceptionMessage.HeaderFieldCanNotBeLongerThen, "DisplayInfo", SipHeaderNames.From, 50));
                }
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
