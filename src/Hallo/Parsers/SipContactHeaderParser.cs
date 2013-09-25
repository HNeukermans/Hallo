using System;
using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Parsers
{
    [ParserFor(SipHeaderNames.Contact)]
    public class SipContactHeaderParser : AbstractParser<SipContactHeader>
    {        
        public override SipContactHeader Parse(StringReader r)
        {
            var header = new SipContactHeader();

            r.ReadToFirstChar();

            if(r.OriginalString[r.Position] == '*') return SipContactHeader.CreateWildCard();

            IfFalseThrowParseException(r.StartsWith("<"), ExceptionMessage.InvalidFormat);

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
