using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Parsers
{
    [ParserFor(SipHeaderNames.CallId)]
    public class SipCallIdHeaderParser : AbstractParser<SipCallIdHeader>
    {
        public override SipCallIdHeader Parse(StringReader r)
        {
            SipCallIdHeader header = new SipCallIdHeader();

            var word = r.ReadWord();

            IfNullThrowParseExceptionInvalidFormat(word);

            IfFalseThrowParseException(word.Length <= 50, string.Format(ExceptionMessage.HeaderValueCanNotBeLongerThen, SipHeaderNames.CallId, 50));

            IfFalseThrowParseException(string.IsNullOrWhiteSpace(r.ReadToEnd()), ExceptionMessage.InvalidFormat);

            header.Value = word.Trim();

            return header;
        }
    }
}
