using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.UnitTest.Builders
{
    public class SipMessageBodyHelperBuilder : ObjectBuilder<SipMessageBody>
    {
        private List<byte> _bytes;
        
        private string _contentType;
        
        public SipMessageBodyHelperBuilder()
        {
            _bytes = new List<byte>();
            _contentType = "application/sdp";
        }

        public SipMessageBodyHelperBuilder WithBytes(List<byte> bytes)
        {
            _bytes = bytes;
            return this;
        }

        public SipMessageBodyHelperBuilder WithContentType(string contentType)
        {
            _contentType = contentType;
            return this;
        }

        public override SipMessageBody Build()
        {
            return new SipMessageBody()
                       {
                           Bytes = _bytes,
                           ContentType = _contentType,
                       };
        }
    }
}
