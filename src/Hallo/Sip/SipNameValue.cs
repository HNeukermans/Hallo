using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Hallo.Sip
{
    public class SipNameValue
    {
        public string Name { get; set; }
        public string Value { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        public SipNameValue(string name)
            : this(name, "")
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="value">Parameter value.</param>
        public SipNameValue(string name, string value)
        {
            //Contract.Requires(!string.IsNullOrWhiteSpace(name));
           
            Name = name;
            Value = value;
        }

        public bool Equals(SipNameValue other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name) && Equals(other.Value, Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SipNameValue)) return false;
            return Equals((SipNameValue) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }
    }
}
