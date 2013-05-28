using Hallo.Sip.Headers;
using Hallo.Sip.Util;

namespace Hallo.UnitTest.Builders
{
    public class SipCallIdHeaderBuilder : ObjectBuilder<SipCallIdHeader>
    {
        private string _value;
        private string _name;
        private bool _isList;


        public SipCallIdHeaderBuilder()
        {
            _value = SipUtil.CreateCallId();
            _name = SipHeaderNames.CallId;
        }

        public SipCallIdHeaderBuilder WithValue(string value)
        {
            _value = value;
            return this;
        }

        public SipCallIdHeaderBuilder WithName(string value)
        {
            _name = value;
            return this;
        }

        public SipCallIdHeaderBuilder WithIsList(bool value)
        {
            _isList = value;
            return this;
        }

        public override SipCallIdHeader Build()
        {
            SipCallIdHeader item = new SipCallIdHeader();
            item.Value = _value;
            
            return item;
        }
    }
}