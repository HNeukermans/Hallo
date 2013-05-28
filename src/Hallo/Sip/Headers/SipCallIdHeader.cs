using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Sip.Headers
{
    [HeaderFor(SipHeaderNames.CallId)]
    public class SipCallIdHeader : SipHeader, IEquatable<SipCallIdHeader>
    {
        public string Value { get; set; }

        public SipCallIdHeader() 
        { 
            Name = SipHeaderNames.CallId; 
        }

        public override string FormatBodyToString()
        {
            var b = new StringBuilder();
            b.Append(this.Value);
            return b.ToString();
        }
        
        public override SipHeader Clone()
        {
            return new SipCallIdHeader()
                       {
                           Value = Value
                       };
        }

        public bool Equals(SipCallIdHeader other)
        {
            return ObjectUtil.ComparePrimitiveProperties(this, other);
        }
    

}
}
