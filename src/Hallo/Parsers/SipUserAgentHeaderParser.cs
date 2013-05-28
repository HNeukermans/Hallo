using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Parsers
{
    [ParserFor(SipHeaderNames.UserAgent)]
    public class SipUserAgentHeaderParser : SipParser<SipUserAgentHeader>
    {
        public override SipUserAgentHeader Parse(StringReader r)
        {
            var header = new SipUserAgentHeader();

            var word = r.ReadToEnd();

            header.Value = word.Trim();

            return header;
        }
    }
}