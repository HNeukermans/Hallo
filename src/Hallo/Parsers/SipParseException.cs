using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Parsers
{
    public class SipParseException : Exception
    {
        public SipParseException(string message):base(message)
        {}

        public SipParseException(Exception ex)
            : base(ex.Message, ex)
        {
            
        }

        public SipParseException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}
