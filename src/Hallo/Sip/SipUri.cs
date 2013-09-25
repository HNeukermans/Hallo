using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip.Headers;
using Hallo.Sip.Util;
using Hallo.Util;

namespace Hallo.Sip
{
    public class SipUri : IStringFormatable, IEquatable<SipUri>
    {
        private int? _port;

        [ExcludeFromEquality]
        public SipParameterCollection Parameters { get; set; }
        
        public SipUri()
        {
           Parameters = new SipParameterCollection();
        }

        public string User { get; set; }

        public string Host { get; set; }

        public int Port 
        { 
            get    
            {
                if(!_port.HasValue) return 5060;
                return _port.Value;
            }
            set
            {
                _port = value;
            }
        }

        /// <summary>
        /// returns the host and port as an IPEndPoint
        /// </summary>
        /// <returns></returns>
        public IPEndPoint GetHostAndPort()
        {
            if(!SipUtil.IsIPAddress(Host)) throw new InvalidOperationException("Failed to convert Host to IpEndPoint. Host must be a numeric Ip address.");
            return new IPEndPoint(IPAddress.Parse(Host), Port);
        }

        /// <summary>
        /// returns true if the host is named. Returns false if the host is a numeric IpAddress.
        /// </summary>
        /// <returns></returns>
        public bool HasNamedHost()
        {
            return !SipUtil.IsIPAddress(Host);
        }
        
        public string FormatToString()
        {
            var sb = new StringBuilder();
            sb.Append("sip:");
            if (User != null) sb.Append(User + "@");
            sb.Append(Host);
            if (_port.HasValue) sb.Append(":" + _port.Value);
            if (this.Parameters.IsNotEmpty()) sb.AppendFormat(";{0}", this.Parameters.FormatToString());
           
            return sb.ToString();
        }

        public bool Equals(SipUri other)
        {
            if (other == null) return false;

            //TODO: compare parameters
            return this.Host == other.Host && this.Port == other.Port && this.User == other.User;
        }

        internal SipUri Clone()
        {
            var p = new SipUriParser();
            return p.Parse(FormatToString());
        }

        public bool IsLooseRouting
        {
            get
            {
                var parameter = this.Parameters[SipParameterNames.LooseRouting];
                return parameter != null;
            }
            set
            {
                if (value == false)
                {
                    this.Parameters.Remove(SipParameterNames.LooseRouting);
                }
                else
                {
                    this.Parameters.Set(SipParameterNames.LooseRouting, string.Empty);
                }
            }
        }

        public string Transport
        {
            get
            {
                var parameter = this.Parameters[SipParameterNames.Transport];
                return parameter == null ? null : parameter.Value;
            }
            set
            {
                if (value == null)
                {
                    this.Parameters.Remove(SipParameterNames.Transport);
                }
                else
                {
                    this.Parameters.Set(SipParameterNames.Transport, value);
                }
            }
        }

    }
}
