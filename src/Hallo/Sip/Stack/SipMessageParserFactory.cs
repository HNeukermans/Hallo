using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hannes.Net.Parsers;

namespace Hannes.Net.Sip
{
    public class SipMessageParserFactory : ISipMessageParserFactory
    {
        public SipMessageParser CreateMessageParser()
        {
            return new SipMessageParser();
        }
   } 
}
