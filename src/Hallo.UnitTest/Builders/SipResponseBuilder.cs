using Hallo.Sip;

namespace Hallo.UnitTest.Builders
{
    internal class SipResponseBuilder : SipMessageBuilder<SipResponse>
    {
        private SipStatusLine _statusLine;

        public SipResponseBuilder()
            : base()
        {
            _statusLine = new SipStatusLineBuilder().Build();
        }

        public SipResponseBuilder WithStatusLine(SipStatusLine value)
        {
            _statusLine = value;
            return this;
        }

        public override SipResponse Build()
        {
            SipResponse item = base.Build();
            item.StatusLine = _statusLine;

            return item;
        }
    }
}