using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Parsers
{
    [ParserFor(SipHeaderNames.Subject)]
    public class SipSubjectHeaderParser : AbstractParser<SipSubjectHeader>
    {
        public override SipSubjectHeader Parse(StringReader r)
        {
            var header = new SipSubjectHeader();

            var line = r.ReadToEnd();

            IfNullOrEmptyThrowParseExceptionInvalidFormat(line);

            IfFalseThrowParseException(line.Length <= 250, string.Format(ExceptionMessage.HeaderValueCanNotBeLongerThen,SipHeaderNames.Subject, 250));
            
            header.Value = line.Trim();

            return header;
        }
    }
}
