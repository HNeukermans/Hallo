using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Parsers
{
    public class ParseException : Exception
    {
        public ParseException(string message):base(message)
        {}

        public ParseException(Exception ex)
            : base(ex.Message, ex)
        {
            
        }

        public ParseException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
    
}
