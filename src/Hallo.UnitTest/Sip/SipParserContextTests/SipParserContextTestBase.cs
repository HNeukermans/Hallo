using System;
using System.Text;
using System.Threading;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.UnitTest.Sip
{
    public class SipParserContextTestBase : Specification
    {
        protected readonly ManualResetEvent waitHandle = new ManualResetEvent(false);
        protected SipParser2 _parser;
        protected SipRequest _sipRequest;
        protected SipResponse _sipResponse;

        protected override void Given()
        {
            _parser = new SipParser2(new SipMessageFactory(), new SipHeaderFactory());
        }
        
    }
}