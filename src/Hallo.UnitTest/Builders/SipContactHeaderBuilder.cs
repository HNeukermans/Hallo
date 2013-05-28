using Hallo.Sip;
using Hallo.Sip.Headers;

namespace Hallo.UnitTest.Builders
{
    public class SipContactHeaderBuilder : ObjectBuilder<SipContactHeader>
    {
        private SipUri _sipUri;
        private string _name;
        private bool _isList;
        private int _expires;
        private bool _isWildCard;
        public SipContactHeaderBuilder()
        {
            _sipUri = new SipUriBuilder().Build();
            _name = SipHeaderNames.Contact;
            _expires = 3600;
        }

        public SipContactHeaderBuilder WithSipUri(SipUri value)
        {
            _sipUri = value;
            return this;
        }

        public SipContactHeaderBuilder WithName(string value)
        {
            _name = value;
            return this;
        }

        public SipContactHeaderBuilder WithExpires(int value)
        {
            _expires = value;
            return this;
        }

        public SipContactHeaderBuilder WithIsWildCard(bool value)
        {
            _isWildCard = value;
            return this;
        }

        public SipContactHeaderBuilder WithIsList(bool value)
        {
            _isList = value;
            return this;
        }
        
        public override SipContactHeader Build()
        {
            if (_isWildCard) return SipContactHeader.CreateWildCard();

            SipContactHeader item = new SipContactHeader();
            item.SipUri = _sipUri;
            item.Expires = _expires;
            return item;
        }
    }
}