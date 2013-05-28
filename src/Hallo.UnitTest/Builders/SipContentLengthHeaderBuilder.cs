using Hallo.Sip;
using Hallo.Sip.Headers;

namespace Hallo.UnitTest.Builders
{
    public class SipContentLengthHeaderBuilder : ObjectBuilder<SipContentLengthHeader>
    {
        private int _value;
        private string _name;
        private bool _isList;
        
        public SipContentLengthHeaderBuilder()
        {
            _value = 0;
            _name = SipHeaderNames.ContentLength;
        }

        public SipContentLengthHeaderBuilder WithValue(int value)
        {
            _value = value;
            return this;
        }

        public SipContentLengthHeaderBuilder WithName(string value)
        {
            _name = value;
            return this;
        }
        
        public override SipContentLengthHeader Build()
        {
            SipContentLengthHeader item = new SipContentLengthHeader();
            item.Value = _value;
            
            return item;
        }
    }

    public class SipContentTypeHeaderBuilder : ObjectBuilder<SipContentTypeHeader>
    {
        private string _value;
        private string _name;

        public SipContentTypeHeaderBuilder()
        {
            _value = "application/sdp";
            _name = SipHeaderNames.ContentType;
        }

        public SipContentTypeHeaderBuilder WithValue(string value)
        {
            _value = value;
            return this;
        }

        public SipContentTypeHeaderBuilder WithName(string value)
        {
            _name = value;
            return this;
        }

        public override SipContentTypeHeader Build()
        {
            var item = new SipContentTypeHeader();
            item.Value = _value;
           
            return item;
        }
    }
}