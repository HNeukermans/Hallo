using Hallo.Sip;

namespace Hallo.UnitTest.Builders
{
    public class SipRequestLineBuilder : ObjectBuilder<SipRequestLine>
    {
        private string _method;
        private SipUri _uri;
        private string _version;


        public SipRequestLineBuilder()
        {
            _method = SipMethods.Register;
            _uri = new SipUriBuilder().Build();
            _version = SipConstants.SipTwoZeroString;
        }

        public SipRequestLineBuilder WithMethod(string value)
        {
            _method = value;
            return this;
        }

        public SipRequestLineBuilder WithUri(SipUri value)
        {
            _uri = value;
            return this;
        }

        public SipRequestLineBuilder WithVersion(string value)
        {
            _version = value;
            return this;
        }

        public override SipRequestLine Build()
        {
            SipRequestLine item = new SipRequestLine();
            item.Method = _method;
            item.Uri = _uri;
            item.Version = _version;

            return item;
        }
    }
}