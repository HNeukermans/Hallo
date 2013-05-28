using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Parsers
{
    [ParserFor(SipHeaderNames.ContentType)]
    public class SipContentTypeHeaderParser: SipParser<SipContentTypeHeader>
    {

        public override SipContentTypeHeader Parse(StringReader r)
        {
            SipContentTypeHeader header = new SipContentTypeHeader();

            var word = r.ReadWord();

            IfNullThrowParseExceptionInvalidFormat(word);

            word = word.Trim();

            IfFalseThrowParseException(!string.IsNullOrWhiteSpace(word), ExceptionMessage.InvalidFormat);

            header.Value = word;

            return header;
        }
    }
    
}
