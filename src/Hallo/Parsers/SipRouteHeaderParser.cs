using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Parsers
{
    [ParserFor(SipHeaderNames.Route)]
    public class SipRouteHeaderParser : SipParser<SipRouteHeader>
    {
        public SipRouteHeaderParser()
            : base()
        {

        }
        
        public override SipRouteHeader Parse(StringReader r)
        {
            var header = new SipRouteHeader();

            r.ReadToFirstChar();

            Action action = () =>
            {
                header.SipUri = new SipUriParser().Parse(r.ReadParenthesized());
            };

            TryActionElseThrowParseExceptionInvalidFormat(action);

            IfFalseThrowParseException(string.IsNullOrWhiteSpace(r.ReadToEnd()), ExceptionMessage.InvalidFormat);
            
            return header;
        }
    }
}
