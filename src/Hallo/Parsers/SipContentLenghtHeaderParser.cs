using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Parsers
{
    [ParserFor(SipHeaderNames.ContentLength)]
    public class SipContentLenghtHeaderParser : AbstractParser<SipContentLengthHeader>
    {
        public override SipContentLengthHeader Parse(StringReader r)
        {
            var header = new SipContentLengthHeader();
            var word = r.ReadWord();

            IfNullThrowParseExceptionInvalidFormat(word);

            word = word.Trim();

            IfFalseThrowParseException(ParseUtil.IsInteger(word), ExceptionMessage.InvalidFormat);

            IfFalseThrowParseException(string.IsNullOrWhiteSpace(r.ReadToEnd()), ExceptionMessage.InvalidFormat);

            header.Value = int.Parse(word);

            return header;
        }
    }
}
