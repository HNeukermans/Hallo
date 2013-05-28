using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Parsers
{
    [ParserFor(SipHeaderNames.CSeq)]
    public class SipCSeqHeaderParser : SipParser<SipCSeqHeader>
    {
        public override SipCSeqHeader Parse(StringReader r)
        {
            SipCSeqHeader header = new SipCSeqHeader();

            var word = r.ReadWord();

            IfNullThrowParseExceptionInvalidFormat(word);

            word = word.Trim();

            IfFalseThrowParseException(ParseUtil.IsInteger(word), ExceptionMessage.InvalidFormat);

            header.Sequence = int.Parse(word);
            
            word = r.ReadWord();

            IfNullOrEmptyThrowParseExceptionInvalidFormat(word);

            IfFalseThrowParseException(SipMethods.IsMethod(word), ExceptionMessage.SipMethodNotSupprted);
            
            IfFalseThrowParseException(string.IsNullOrWhiteSpace(r.ReadToEnd()), ExceptionMessage.InvalidFormat);
            
            header.Command = word.Trim();

            return header;
        }
    }
}
