using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip.Headers
{
    [HeaderFor(SipHeaderNames.UserAgent)]
    public class SipUserAgentHeader : SipHeader, IEquatable<SipUserAgentHeader>
    {
        public string Value { get; set; }

        public SipUserAgentHeader()
        {
            Name = SipHeaderNames.UserAgent;
        }

        public override string FormatBodyToString()
        {
            var b = new StringBuilder();
            b.Append(this.Value);
            return b.ToString();
        }

        public override SipHeader Clone()
        {
            return new SipUserAgentHeader()
                       {
                           Value = Value
                       };
        }

        public bool Equals(SipUserAgentHeader other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Value, Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(SipUserAgentHeader)) return false;
            return Equals((SipUserAgentHeader)obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}
