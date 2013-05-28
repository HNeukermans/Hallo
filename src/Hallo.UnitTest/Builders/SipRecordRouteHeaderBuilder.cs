using Hallo.Sip;
using Hallo.Sip.Headers;

namespace Hallo.UnitTest.Builders
{
    public class SipRecordRouteHeaderBuilder : ObjectBuilder<SipRecordRouteHeader>
    {
        private SipUri _sipUri;

        public SipRecordRouteHeaderBuilder()
        {
            _sipUri = new SipUriBuilder().Build();
        }


        public SipRecordRouteHeaderBuilder WithSipUri(SipUri value)
        {
            _sipUri = value;
            return this;
        }

        public override SipRecordRouteHeader Build()
        {
            var rrh = new SipRecordRouteHeader()
                          {
                              SipUri = _sipUri
                          };
            return rrh;
        }
    }
}