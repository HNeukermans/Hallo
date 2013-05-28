using System;
using System.Text;
using System.Threading;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.UnitTest.Sip
{
    public class SipParserContextTestBase : Specification
    {
        protected readonly ManualResetEvent waitHandle = new ManualResetEvent(false);
        protected SipParserContext _parserContext;
        protected SipRequest _sipRequest;
        protected SipResponse _sipResponse;

        protected override void Given()
        {
            _parserContext = new SipParserContext(new SipMessageFactory(), new SipHeaderFactory());
            _parserContext.ParseCompleted += new EventHandler<ParseCompletedEventArgs>(_parserContext_ParseCompleted);
        }

        private void _parserContext_ParseCompleted(object sender, ParseCompletedEventArgs e)
        {
            waitHandle.Set();
            _sipRequest = e.Message as SipRequest;
            _sipResponse = e.Message as SipResponse;
        }

        
    }
}