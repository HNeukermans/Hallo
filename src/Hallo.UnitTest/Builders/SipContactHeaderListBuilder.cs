using Hallo.Sip;
using Hallo.Sip.Headers;

namespace Hallo.UnitTest.Builders
{
    public class SipContactHeaderListBuilder : ObjectBuilder<SipHeaderList<SipContactHeader>>
    {
        private SipHeaderList<SipContactHeader> _Contacts;

        public SipContactHeaderListBuilder()
        {
            _Contacts = new SipHeaderList<SipContactHeader>();
        }

        public SipContactHeaderListBuilder Add(SipContactHeader value)
        {
            _Contacts.Add(value);
            return this;
        }

        public override SipHeaderList<SipContactHeader> Build()
        {
            return _Contacts;
        }
    }
}