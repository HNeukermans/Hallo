using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.UnitTest.Builders
{
    internal class SipRequestBuilder : SipMessageBuilder<SipRequest>
    {
        private SipRequestLine _requestLine;
        
        public SipRequestBuilder():base()
        {
            _requestLine = new SipRequestLineBuilder().Build();
        }

        public SipRequestBuilder WithRequestLine(SipRequestLine value)
        {
            _requestLine = value;
            return this;
        }
	
        public override SipRequest Build()
        {
            SipRequest item = base.Build();
            item.RequestLine = _requestLine;
            
            return item;
        }
    }

    internal class SipRequestEventBuilder : ObjectBuilder<SipRequestEvent>
    {
        private SipRequest _request;

        public SipRequestEventBuilder()
        {
            _request = new SipRequestBuilder().Build();
        }

        public SipRequestEventBuilder WithRequest(SipRequest request)
        {
            _request = request;
            return this;
        }

        public override SipRequestEvent Build()
        {
            var ctx = new SipContext();
            ctx.Request = _request;
            return new SipRequestEvent(ctx);
        }
    }

}