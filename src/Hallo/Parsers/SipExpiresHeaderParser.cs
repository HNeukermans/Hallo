using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Parsers
{
    [ParserFor(SipHeaderNames.Expires)]
    public class SipExpiresHeaderParser : SipParser<SipExpiresHeader>
    {
        public override SipExpiresHeader Parse(StringReader r)
        {
            var header = new SipExpiresHeader();
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