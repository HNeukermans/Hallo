using System;
using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Parsers
{
    [ParserFor(SipHeaderNames.RecordRoute)]
    public class SipRecordRouteHeaderParser : SipParser<SipRecordRouteHeader>
    {
        public SipRecordRouteHeaderParser()
            : base()
        {

        }

        public override SipRecordRouteHeader Parse(StringReader r)
        {
            var header = new SipRecordRouteHeader();

            r.ReadToFirstChar();

            string word = r.ReadParenthesized();
            
            Action action = () =>
                                {
                                    header.SipUri = new SipUriParser().Parse(word);
                                };

            TryActionElseThrowParseExceptionInvalidFormat(action);

            IfFalseThrowParseException(string.IsNullOrWhiteSpace(r.ReadToEnd()), ExceptionMessage.InvalidFormat);
           
            return header;
        }
    }
}