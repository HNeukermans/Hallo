using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip
{
    /// <summary>
    /// exception thrown from SIP server
    /// </summary>
    public class SipException : Exception
    {
        public string ResponseCode { get; set; }

        public SipException(string responseCode, string message)
            : base(message)
        {
            ResponseCode = responseCode;
        }

        public SipException(string responseCode, string message, params string[] args)
            : base(string.Format(message))
        {
            ResponseCode = responseCode;
        }

        public SipException(string responseCode)
            : base(null)
        {
            ResponseCode = responseCode;
        }

    }

    public class SipCoreException : Exception
    {
        public SipCoreException(string message, params string[] args)
            : base(string.Format(message))
        {
        }
    }
}
