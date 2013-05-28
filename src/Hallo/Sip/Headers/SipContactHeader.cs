using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Hallo.Parsers;
using Hallo.Util;

namespace Hallo.Sip.Headers
{
    [HeaderFor(SipHeaderNames.Contact)]
    public class SipContactHeader : SipHeader
    {
        private bool _isWildCard;

        [ExcludeFromEquality]
        public SipParameterCollection Parameters { get; set; }
       
        public SipUri SipUri { get; set; }

        public bool IsWildCard { get { return _isWildCard; } }

        public static SipContactHeader CreateWildCard()
        {
            var h = new SipContactHeader();
            h._isWildCard = true;
            return h;
        }

        public SipContactHeader()
        {
            Name = SipHeaderNames.Contact;
            Parameters = new SipParameterCollection();
        }

        public override string FormatBodyToString()
        {
            if (_isWildCard) return "*";

            var b = new StringBuilder();
            b.AppendFormat("<{0}>", this.SipUri.FormatToString());
            if (this.Parameters.IsNotEmpty()) b.AppendFormat(";{0}", this.Parameters.FormatToString());
            return b.ToString();
        }

        public override SipHeader Clone()
        {
            var p = new SipContactHeaderParser();
            return p.Parse(this.FormatBodyToString());
        }

        public int? Expires
        {
            get
            {
                var parameter = this.Parameters[SipParameterNames.Expires];
                if (parameter != null)
                {
                    return int.Parse(parameter.Value);
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    this.Parameters.Remove(SipParameterNames.Expires);
                }
                else
                {
                    this.Parameters.Set(SipParameterNames.Expires, value.ToString());
                }
            }
        }
    }
}
