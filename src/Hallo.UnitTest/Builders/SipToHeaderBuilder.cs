using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Util;
using Hallo.UnitTest.Helpers;

namespace Hallo.UnitTest.Builders
{
    public class SipToHeaderBuilder : ObjectBuilder<SipToHeader>
    {
        private string _displayInfo;
        private SipUri _sipUri;
        private string _tag;
        private string _name;
        private bool _isList;
        
        public SipToHeaderBuilder()
        {
            _displayInfo = TestConstants.MyUserName;
            _sipUri = new SipUriBuilder().Build();
            _tag = SipUtil.CreateTag();
            _name = SipHeaderNames.To;
        }

        public SipToHeaderBuilder WithDisplayInfo(string value)
        {
            _displayInfo = value;
            return this;
        }

        public SipToHeaderBuilder WithSipUri(SipUri value)
        {
            _sipUri = value;
            return this;
        }
        
        public SipToHeaderBuilder WithTag(string value)
        {
            _tag = value;
            return this;
        }

        public SipToHeaderBuilder WithName(string value)
        {
            _name = value;
            return this;
        }

        public SipToHeaderBuilder WithIsList(bool value)
        {
            _isList = value;
            return this;
        }

        public override SipToHeader Build()
        {
            SipToHeader item = new SipToHeader();
            item.DisplayInfo = _displayInfo;
            item.SipUri = _sipUri;
            item.Tag = _tag;

            return item;
        }
    }
}