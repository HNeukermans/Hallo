using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Util;
using System.Net;

namespace Hallo.Parsers
{
    [ParserFor(SipHeaderNames.Via)]
    public class SipViaHeaderParser : AbstractParser<SipViaHeader>
    {
        public SipViaHeaderParser()
            : base()
        {

        }

        public override SipViaHeader Parse(StringReader r)
        {
            SipViaHeader header = new SipViaHeader();

            #region via-parm
            
            // protocol-name
            string word = r.QuotedReadToDelimiter('/');

            IfNullOrEmptyThrowParseExceptionInvalidFormat(word);
            
            string version = word.Trim();

            word = r.QuotedReadToDelimiter('/');
            
            IfNullOrEmptyThrowParseExceptionInvalidFormat(word);
            
            version += "/" + word.Trim();

            if (!version.Equals(SipConstants.SipTwoZeroString, StringComparison.InvariantCultureIgnoreCase))
                throw new ParseException(ExceptionMessage.InvalidFormat);

            word = r.ReadWord();

            IfNullOrEmptyThrowParseExceptionInvalidFormat(word);

            IfFalseThrowParseException(word.Equals(SipConstants.Udp, StringComparison.InvariantCultureIgnoreCase), ExceptionMessage.InvalidTransport);

            header.Transport = word.Trim();

            word = r.QuotedReadToDelimiter(new char[]{';',','},false);

            if (word == null)
            {
                throw new ParseException(ExceptionMessage.InvalidFormat);
            }

            word = word.Trim();

            try
            {
                if (ParseUtil.IsIPAddress(word))
                {
                    header.SentBy = new IPEndPoint(IPAddress.Parse(word), 5060);
                }
                else
                {
                    header.SentBy = ParseUtil.ParseIpEndPoint(word);
                }
            }
            catch (Exception e)
            {
                throw new ParseException(ExceptionMessage.InvalidFormat, e);
            }

            #endregion

            #region via-params

            header.Parameters = new SipParameterCollectionParser().Parse(r.ReadToEnd());

            if (header.Parameters[SipParameterNames.Branch] == null)
            {
                throw new ParseException(ExceptionMessage.BranchParameterCanNotBeNull);
            }

            return header;

            #endregion

        }
    }
    
}
