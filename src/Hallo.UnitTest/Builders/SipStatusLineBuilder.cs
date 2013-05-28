using Hallo.Parsers;
using Hallo.Sip;

namespace Hallo.UnitTest.Builders
{
    public class SipStatusLineBuilder : ObjectBuilder<SipStatusLine>
    {
        private string _reasonPhrase;
        private int _statusCode;
        private string _version;


        public SipStatusLineBuilder()
        {
            _reasonPhrase = "OK";
            _statusCode = 200;
            _version = SipConstants.SipTwoZeroString;
        }

        public SipStatusLineBuilder WithReason(string value)
        {
            _reasonPhrase = value;
            return this;
        }

        public SipStatusLineBuilder WithStatusCode(int value)
        {
            _statusCode = value;
            return this;
        }

        public SipStatusLineBuilder WithVersion(string value)
        {
            _version = value;
            return this;
        }

        public override SipStatusLine Build()
        {
            SipStatusLine item = new SipStatusLine();
            item.ReasonPhrase = _reasonPhrase;
            item.StatusCode = _statusCode;
            item.Version = _version;

            return item;
        }
    }
}