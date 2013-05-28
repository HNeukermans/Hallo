using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Sip
{
    [HeaderFor(SipHeaderNames.CSeq)]
    public class SipCSeqHeader : SipHeader, IEquatable<SipCSeqHeader>
    {
        public int Sequence { get; set; }

        public string Command { get; set; }

        public SipCSeqHeader()
        {
            Name = SipHeaderNames.CSeq;
        }


        public override string FormatBodyToString()
        {
            var b = new StringBuilder();
            b.Append(Sequence).Append(" ").Append(Command);
            return b.ToString();
        }

        public override SipHeader Clone()
        {
            return new SipCSeqHeader() { Command = this.Command, Sequence = this.Sequence };
        }

        public bool Equals(SipCSeqHeader other)
        {
            return ObjectUtil.ComparePrimitiveProperties(this, other);
        }
    }
}
