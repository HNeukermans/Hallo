using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip.Headers;
using Hallo.Util;
using Hallo.Sip;

namespace Hallo.Parsers
{
    public class SipParameterCollectionParser : AbstractParser<SipParameterCollection>
    {
        public SipParameterCollectionParser() : base() { }

        public override SipParameterCollection Parse(StringReader r)
        {
            SipParameterCollection result = new SipParameterCollection();

            // Parse parameters
            while (r.Available > 0)
            {
                r.ReadToFirstChar();

                // We have parameter
                if (r.SourceString.StartsWith(";"))
                {
                    r.ReadSpecifiedLength(1);
                    string paramString = r.QuotedReadToDelimiter(new char[] { ';', ',' }, false);
                    
                    if (string.IsNullOrWhiteSpace(paramString))
                    {
                        throw new ParseException("Parameter can not be found. A parameter is expected after ';'. OriginalString: '" + r.OriginalString + "' !");
                    }
                    
                    string[] name_value = paramString.Split(new char[] { '=' }, 2);
                        
                    if(name_value[0].StartsWith(" "))
                    {
                        throw new ParseException(string.Format("Unexpected token:' '. A parameter can not begin with a ' '. Parameter :'" + name_value + "' !"));
                    }

                    name_value[0] = name_value[0].TrimEnd();

                    if (name_value.Length == 2)
                    {
                        name_value[1] = name_value[1].TrimEnd();

                        result.Add(new SipNameValue(name_value[0], TextUtils.UnQuoteString(name_value[1])));
                    }
                    else
                    {
                        result.Add(new SipNameValue(name_value[0]));
                    }
                }
                // Unknown data
                else
                {
                    throw new ParseException("Parameter prefix ';' can not be found. Each parameter must be prefixed. OriginalString: '" + r.OriginalString + "' !");
                }
            }

            return result;
        }
    }
}
