using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip;
using Hallo.Util;

namespace Hallo.Parsers
{
    public class SipStatusLineParser : AbstractParser<SipStatusLine>
    {
        public override SipStatusLine Parse(StringReader r)
        {
            string[] splitted = r.OriginalString.Split(new char[] {' '}, 3);

            IfFalseThrowParseException(splitted.Length == 3, ExceptionMessage.InvalidStatusLineFormat);
            IfFalseThrowParseException(splitted[0].Equals(SipConstants.SipTwoZeroString), ExceptionMessage.InvalidSipVersion);
            IfFalseThrowParseException(ParseUtil.IsInteger(splitted[1]), ExceptionMessage.InvalidFormat);
            IfNullOrEmptyThrowParseExceptionInvalidFormat(splitted[2]);
            IfFalseThrowParseException(splitted[2].Length <= 250, "The 'ReasonPhrase' field of the 'StatusLine' can not be longer than 250 characters"); 

            var result = new SipStatusLine();
            result.Version = SipConstants.SipTwoZeroString;
            result.StatusCode = int.Parse(splitted[1]);
            result.ReasonPhrase = splitted[2];
            
            return result;
        }
    }
}
