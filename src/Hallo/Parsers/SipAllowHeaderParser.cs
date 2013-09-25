using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Parsers
{
    [ParserFor(SipHeaderNames.Allow)]
    public class SipAllowHeaderParser : AbstractParser<SipAllowHeader>
    {
        public override SipAllowHeader Parse(StringReader r)
        {
            var header = new SipAllowHeader();

            var word = r.ReadToEnd();

            header.Value = word.Trim();

            return header;
        }
    }
}