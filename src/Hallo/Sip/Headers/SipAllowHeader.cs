using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip.Headers
{
    [HeaderFor(SipHeaderNames.Allow)]
    public class SipAllowHeader : SipHeader, IEquatable<SipAllowHeader>
    {
        public string Value { get; set; }

        public SipAllowHeader()
        {
            Name = SipHeaderNames.Allow;
        }

        public override string FormatBodyToString()
        {
            var b = new StringBuilder();
            b.Append(this.Value);
            return b.ToString();
        }

        public override SipHeader Clone()
        {
            return new SipAllowHeader()
            {
                Value = Value
            };
        }

        public bool Equals(SipAllowHeader other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Value, Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SipAllowHeader)) return false;
            return Equals((SipAllowHeader) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}
