using System.Net;
using Hallo.Sip;
using Hallo.UnitTest.Helpers;

namespace Hallo.UnitTest.Builders
{
    public class SipContextBuilder : ObjectBuilder<SipContext>
    {
        private SipRequest _request;
        private IPEndPoint _remoteEndPoint;
        private SipResponse _response;

        public SipContextBuilder()
        {
            _request = new SipRequestBuilder().Build();
            _remoteEndPoint = TestConstants.IpEndPoint1;
        }

        public SipContextBuilder WithRemoteEndPoint(IPEndPoint ipEndPoint)
        {
            _remoteEndPoint = ipEndPoint;
            return this;
        }

        public SipContextBuilder WithRequest(SipRequest request)
        {
            _request = request;
            return this;
        }

        public override SipContext Build()
        {
            var c = new SipContext();
            c.Request = _request;
            c.Response = _response;
            c.RemoteEndPoint = _remoteEndPoint;
            return c;
        }

        public SipContextBuilder WithResponse(SipResponse response)
        {
            _response = response;
            return this;
        }
    }
}