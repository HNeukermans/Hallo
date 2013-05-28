using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip.Headers;

namespace Hallo.Sip
{

    [HeaderFor(SipHeaderNames.ContentLength)]
    public class SipContentLengthHeader : SipHeader
    {
        public int Value { get; set; }

        public SipContentLengthHeader() 
        { 
            Name = SipHeaderNames.ContentLength; 
        }

        public override string FormatBodyToString()
        {
            StringBuilder b = new StringBuilder();
            b.Append(this.Value);
            return b.ToString();
        }

        public override SipHeader Clone()
        {
            return new SipContentLengthHeader() {Value = this.Value};
        }
    }
}
