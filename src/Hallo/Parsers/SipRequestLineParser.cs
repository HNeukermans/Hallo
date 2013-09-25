using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Parsers
{
    public class SipRequestLineParser : AbstractParser<SipRequestLine>
    {        
        public override SipRequestLine Parse(StringReader r)
        {
            string[] splitted = r.OriginalString.Split(' ');

            if (splitted.Length != 3) throw new ParseException(ExceptionMessage.InvalidRequestLineFormat);

            SipRequestLine result = new SipRequestLine();
            result.Method = splitted[0];
            result.Uri = new SipUriParser().Parse(splitted[1]);
            result.Version = splitted[2];

            return result;
        }
    }
}
