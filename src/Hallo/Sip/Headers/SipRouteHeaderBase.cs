using System;
using System.Text;
using Hallo.Util;

namespace Hallo.Sip.Headers
{
    public abstract class SipRouteHeaderBase<T> : SipHeader, IEquatable<T> where T : SipRouteHeaderBase<T>
    {
        public SipUri SipUri { get; set; }

        [ExcludeFromEquality]
        public SipParameterCollection Parameters { get; set; }

        protected SipRouteHeaderBase()
        {
            Name = SipHeaderNames.RecordRoute;
            Parameters = new SipParameterCollection();
        }

        public override string FormatBodyToString()
        {
            var b = new StringBuilder();
            b.AppendFormat("<{0}>", this.SipUri.FormatToString());
            if (this.Parameters.IsNotEmpty()) b.AppendFormat(";{0}", this.Parameters.FormatToString());
            return b.ToString();
        }

        public override SipHeader Clone()
        {
            return new SipRouteHeader()
                       {
                           SipUri = this.SipUri.Clone(), Parameters = Parameters.Clone()
                       };
        }

        public bool Equals(T other)
        {
            if (other == null) return false;
            return SipUri.Equals(other.SipUri) && Parameters.Equals(other.Parameters);
        }
        
       

    }
}