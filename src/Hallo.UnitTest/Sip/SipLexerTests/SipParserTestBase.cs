using System.Collections.Generic;
using System.Linq;
using Hallo.Sip;

namespace Hallo.UnitTest.Sip.SipParserTests
{
    public class SipLexerTestBase : Specification
    {

        protected Dictionary<string, List<string>> OnHeaderExecuteReceived { get; set; }
        protected bool OnBodyBytesExecuteReceived { get; set; }
        protected List<byte> OnBodyBytesReceived { get; set; }
        protected string OnFirstLineExecuteReceived { get; set; }
        internal SipLexer _lexer;
        protected byte[] _bytes;

        protected SipLexerTestBase()
        {
            OnHeaderExecuteReceived = new Dictionary<string, List<string>>();
            OnBodyBytesReceived = null;

        }

        

        protected override void When()
        {
            _lexer.Lex(_bytes, OnFirstLine, OnHeader, OnBodyBytes);
        }

        protected override void Given()
        {
            _lexer = new SipLexer();
        }

        private void OnFirstLine(string line)
        {
            OnFirstLineExecuteReceived = line;
        }

        private void OnHeader(string name, string value)
        {
            if (!OnHeaderExecuteReceived.ContainsKey(name)) OnHeaderExecuteReceived.Add(name, new List<string>());

            OnHeaderExecuteReceived[name].Add(value);
        }

        private void OnBodyBytes(byte[] body)
        {
            OnBodyBytesExecuteReceived = true;
            OnBodyBytesReceived = body != null ? body.ToList() : null;
        }
    }
}