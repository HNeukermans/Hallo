using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.Parsers
{
    public interface ISipParserListener
    {
        void OnRequest(Hallo.Sip.SipRequestLine requestLine);
        void OnResponse(SipStatusLine statusLine);
        void OnComplete();
        void OnHeader(string name, string value);
        void OnBodyBytes(byte[] buffer);
    }

   
}
