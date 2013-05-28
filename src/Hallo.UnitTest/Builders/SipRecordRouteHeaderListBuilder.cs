using Hallo.Sip;
using Hallo.Sip.Headers;

namespace Hallo.UnitTest.Builders
{
    public class SipRecordRouteHeaderListBuilder : ObjectBuilder<SipHeaderList<SipRecordRouteHeader>>
    {
        private SipHeaderList<SipRecordRouteHeader> _list;

        public SipRecordRouteHeaderListBuilder()
        {
            _list = new SipHeaderList<SipRecordRouteHeader>();
        }

        public SipRecordRouteHeaderListBuilder Add(SipRecordRouteHeader value)
        {
            _list.Add(value);
            return this;
        }

        public override SipHeaderList<SipRecordRouteHeader> Build()
        {
            return _list;
        }
    }
}