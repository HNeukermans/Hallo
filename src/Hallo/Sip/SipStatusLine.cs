using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip
{
    public class SipStatusLine : IStringFormatable
    {
        public string ReasonPhrase { get; set; }

        public int StatusCode { get; set; }
        
        public string Version { get; set; }

        public string FormatToString()
        {
            return string.Format("{0} {1} {2}", SipConstants.SipTwoZeroString, StatusCode, ReasonPhrase);
        }

        /// <summary>
        /// concatenates StatusCode and ReasonPhrase
        /// </summary>
        public string ResponseCode
        {
            get { return string.Format("{0} {1}", StatusCode, ReasonPhrase); }
        }
    }
}
