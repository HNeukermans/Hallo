using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip;

namespace Hallo.UnitTest.Stubs
{
    public class SipParserListenerStub : ISipParserListener
    {
        public bool OnCompleteExecuteReceived { get; set; }
        public Dictionary<string, List<string>> OnHeaderExecuteReceived { get; set; }
        public List<byte> OnBodyBytesExecuteReceived { get; set; }
        public SipStatusLine OnResponseExecuteReceived { get; set; }
        public SipRequestLine OnRequestExecuteReceived { get; set; }

        public SipParserListenerStub()
        {
            OnHeaderExecuteReceived = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);
        }

        public void OnRequest(SipRequestLine requestLine)
        {
            OnRequestExecuteReceived = requestLine;
        }

        public void OnResponse(SipStatusLine statusLine)
        {
            OnResponseExecuteReceived = statusLine;
        }

        public void OnComplete()
        {
            OnCompleteExecuteReceived = true;
        }

        public void OnHeader(string name, string value)
        {
            if(!OnHeaderExecuteReceived.ContainsKey(name))OnHeaderExecuteReceived.Add(name, new List<string>());

            OnHeaderExecuteReceived[name].Add(value);
        }

        public void OnBodyBytes(byte[] body)
        {
            OnBodyBytesExecuteReceived = body.ToList();
        }
    }
}
