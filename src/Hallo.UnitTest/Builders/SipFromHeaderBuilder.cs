using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Util;
using Hallo.UnitTest.Helpers;

namespace Hallo.UnitTest.Builders
{
    public class SipFromHeaderBuilder : ObjectBuilder<SipFromHeader>
    {
        private SipUri _sipUri;
        private string _displayInfo;
        private string _tag;
        private string _name;
        private bool _isList;
        
        public SipFromHeaderBuilder()
        {
            _sipUri = new SipUriBuilder().Build();
            _displayInfo = TestConstants.MyUserName;
            _tag = SipUtil.CreateTag();
            _name = SipHeaderNames.From;
        }

        public SipFromHeaderBuilder WithSipUri(SipUri value)
        {
            _sipUri = value;
            return this;
        }

        
        public SipFromHeaderBuilder WithDisplayInfo(string value)
        {
            _displayInfo = value;
            return this;
        }

        public SipFromHeaderBuilder WithTag(string value)
        {
            _tag = value;
            return this;
        }

        public SipFromHeaderBuilder WithName(string value)
        {
            _name = value;
            return this;
        }

        public SipFromHeaderBuilder WithIsList(bool value)
        {
            _isList = value;
            return this;
        }

        public override SipFromHeader Build()
        {
            SipFromHeader item = new SipFromHeader();
            item.SipUri = _sipUri;
            item.DisplayInfo = _displayInfo;
            item.Tag = _tag;

            return item;
        }
    }
}