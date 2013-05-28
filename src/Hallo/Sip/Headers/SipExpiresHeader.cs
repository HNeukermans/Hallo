using System.Text;
using Hallo.Sip.Headers;

namespace Hallo.Sip
{
    [HeaderFor(SipHeaderNames.Expires)]
    public class SipExpiresHeader : SipHeader
    {
        public int Value { get; set; }

        internal SipExpiresHeader()
        {
            Name = SipHeaderNames.Expires;
        }

        public override string FormatBodyToString()
        {
            StringBuilder b = new StringBuilder();
            b.Append(this.Value);
            return b.ToString();
        }

        public override SipHeader Clone()
        {
            return new SipExpiresHeader() { Value = this.Value };
        }
    }
}