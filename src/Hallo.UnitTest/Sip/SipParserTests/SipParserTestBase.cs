using Hallo.Parsers;
using Hallo.Sip;
using Hallo.UnitTest.Stubs;

namespace Hallo.UnitTest.Sip
{
    public class SipParserTestBase : Specification
    {
        protected SipParser _parser;
        protected SipParserListenerStub _listenerStub;
        protected byte[] _bytes;

        protected override void When()
        {
            _parser.Parse(_bytes, 0, _bytes.Length);
        }

        protected override void Given()
        {
            _listenerStub = new SipParserListenerStub();
            _parser = new SipParser(_listenerStub);
        }
    }
}