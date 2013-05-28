using System;
using System.Text;

namespace Hallo.Sip.Headers
{
    [HeaderFor(SipHeaderNames.Subject)]
    public class SipSubjectHeader : SipHeader, IEquatable<SipSubjectHeader>
    {
        public string Value { get; set; }

        public SipSubjectHeader()
        {
            Name = SipHeaderNames.Subject;
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

        public bool Equals(SipSubjectHeader other)
        {
            if (other == null) return false;
            return this.Value == other.Value;
        }
    }
}
