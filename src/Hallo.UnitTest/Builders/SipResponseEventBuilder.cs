using System.Net;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions;
using Hallo.UnitTest.Helpers;

namespace Hallo.UnitTest.Builders
{
    public class SipResponseEventBuilder : ObjectBuilder<SipResponseEvent>
    {
        private IPEndPoint _remoteEndPoint;
        private SipResponse _response;
        private ISipClientTransaction _tx;

        public SipResponseEventBuilder()
        {
            _response = new SipResponseBuilder().Build();
            _remoteEndPoint = TestConstants.IpEndPoint1;
        }

        public SipResponseEventBuilder WithRemoteEndPoint(IPEndPoint ipEndPoint)
        {
            _remoteEndPoint = ipEndPoint;
            return this;
        }

        public override SipResponseEvent Build()
        {
            var c = new SipContextBuilder()
                .WithRemoteEndPoint(_remoteEndPoint)
                .WithResponse(_response).Build();
            var re = new SipResponseEvent(c);
            re.ClientTransaction = _tx;

            return re;
        }

        public SipResponseEventBuilder WithResponse(SipResponse response)
        {
            _response = response;
            return this;
        }

        public SipResponseEventBuilder WithClientTx(ISipClientTransaction tx)
        {
            _tx = tx;
            return this;
        }
    }
}