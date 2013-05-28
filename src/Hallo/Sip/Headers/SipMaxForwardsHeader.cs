using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip.Headers;

namespace Hallo.Sip
{
    [HeaderFor(SipHeaderNames.MaxForwards)]
    public class SipMaxForwardsHeader : SipHeader, IEquatable<SipMaxForwardsHeader>
    {
        internal SipMaxForwardsHeader(int value):this()
        {
            Value = value;
        }

        public int Value { get; set; }
        
        public override string FormatBodyToString()
        {
            var b = new StringBuilder();
            b.Append(this.Value);
            return b.ToString();
        }

        public SipMaxForwardsHeader() 
        { 
            Name = SipHeaderNames.MaxForwards;
        }

        public override SipHeader Clone()
        {
            return new SipMaxForwardsHeader(){ Value = this.Value};
        }

        public bool Equals(SipMaxForwardsHeader other)
        {
            if (other == null) return false;
            return Value.Equals(other.Value);
        }
    }
}
