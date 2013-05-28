using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Parsers
{
    [ParserFor(SipHeaderNames.MaxForwards)]
    public class SipMaxForwardsHeaderParser: SipParser<SipMaxForwardsHeader>
    {
        public override SipMaxForwardsHeader Parse(StringReader r)
        {
            var header = new SipMaxForwardsHeader();

            var word = r.ReadWord();

            IfFalseThrowParseException(ParseUtil.IsInteger(word), ExceptionMessage.InvalidFormat);

            header.Value = int.Parse(word);

            IfFalseThrowParseException(string.IsNullOrWhiteSpace(r.ReadToEnd()), ExceptionMessage.InvalidFormat);
            
            return header;
        }
    }
    
}