using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Parsers;
using Hallo.Util;

namespace Hallo.Sip.Headers
{
    [HeaderFor(SipHeaderNames.To)]
    public class SipToHeader : SipHeader, IEquatable<SipToHeader>
    {
        public string DisplayInfo { get; set; }
        public SipUri SipUri { get; set; }

        [ExcludeFromEquality]
        public SipParameterCollection Parameters { get; set; }
        
        public SipToHeader() 
        { 
            Name = SipHeaderNames.To;
            Parameters = new SipParameterCollection();
        }

        public string Tag
        {
            get
            {
                var parameter = this.Parameters[SipParameterNames.Tag];
                if (parameter != null)
                {
                    return parameter.Value;
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    this.Parameters.Remove(SipParameterNames.Tag);
                }
                else
                {
                    this.Parameters.Set(SipParameterNames.Tag, value);
                }
            }
        }

        
        public override string FormatBodyToString()
        {
            var b = new StringBuilder();
            if (this.DisplayInfo != null)
            {
                b.AppendFormat("\"{0}\"", this.DisplayInfo);
                b.Append(" ");
            }

            b.AppendFormat("<{0}>", this.SipUri.FormatToString());
            if (this.Parameters.IsNotEmpty()) b.AppendFormat(";{0}", this.Parameters.FormatToString());
            return b.ToString();
        }

        public override SipHeader Clone()
        {
            var p = new SipToHeaderParser();
            return p.Parse(this.FormatBodyToString());
        }
        
        public bool Equals(SipToHeader other)
        {
            if (!ObjectUtil.ComparePrimitiveProperties(this, other)) return false;

            return SipUri.Equals(other.SipUri);
        }
    }
}


