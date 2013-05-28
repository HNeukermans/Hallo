using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Parsers;

namespace Hallo.Sip
{
    public class SipResponse : SipMessage
    {
       public SipStatusLine StatusLine { get; set; }

        public SipResponse()
        {
            StatusLine = new SipStatusLine();
        }

        public string GetResponseCode()
        {
            if (StatusLine == null) return null;
            return string.Format("{0} {1}", StatusLine.StatusCode, StatusLine.ReasonPhrase);
        }
        
    }
    
}
