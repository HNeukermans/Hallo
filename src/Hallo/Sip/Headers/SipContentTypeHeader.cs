using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Parsers;

namespace Hallo.Sip.Headers
{
    [HeaderFor(SipHeaderNames.ContentType)]
    public class SipContentTypeHeader: SipHeader
    {
        public string  Value { get; set; }

        public SipContentTypeHeader() 
        { 
            Name = SipHeaderNames.ContentType; 
        }

        public override string FormatBodyToString()
        {
            var b = new StringBuilder();
            b.Append(this.Value);
            return b.ToString();
        }

        public override SipHeader Clone()
        {
            return new SipContentTypeHeader() { Value = this.Value };
        }
    }
}
