using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using Hallo.Parsers;
using Hallo.Util;

namespace Hallo.Sip.Headers
{
    [HeaderFor(SipHeaderNames.Via)]
    public class SipViaHeader : SipHeader
    {
        public string Transport { get; set; }

        public IPEndPoint SentBy { get; set; }

        [ExcludeFromEquality]
        public SipParameterCollection Parameters { get; set; }

        public SipViaHeader()
        {
            Name = SipHeaderNames.Via;
            Parameters = new SipParameterCollection();
        }

        public IPAddress Received
        {
            get
            {
                var parameter = this.Parameters[SipParameterNames.Received];
                if (parameter != null)
                {
                    return IPAddress.Parse(parameter.Value);
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    this.Parameters.Remove(SipParameterNames.Received);
                }
                else
                {
                    this.Parameters.Set(SipParameterNames.Received, value.ToString());
                }
            }
        }

        public string Branch
        {
            get
            {
                var parameter = this.Parameters[SipParameterNames.Branch];
                if (parameter != null)
                {
                    return parameter.Value;
                }
                return null;
            }
            set
            {
                this.Parameters.Set(SipParameterNames.Branch, value);
            }
        }

        /// <summary>
        /// This parameter MUST have no
        /// value; it serves as a flag to indicate to the server that this
        /// extension is supported and requested for the transaction. RFC 3581: Symmetric routing response
        /// </summary>
        /// <example>
        /// The Session Initiation Protocol (SIP) operates over UDP and TCP,
        /// among others.  When used with UDP, responses to requests are returned
        /// to the source address the request came from, and to the port written
        /// into the topmost Via header field value of the request.  This
        /// behavior is not desirable in many cases, most notably, when the
        /// client is behind a Network Address Translator (NAT).  This extension
        /// defines a new parameter for the Via header field, called "rport",
        /// that allows a client to request that the server send the response
        /// back to the source IP address and port from which the request
        /// originated.
        /// </example>
        public int Rport
        {
            get
            {
                var parameter = this.Parameters[SipParameterNames.RPort];
                if (parameter != null && !string.IsNullOrEmpty(parameter.Value))
                {
                    return int.Parse(parameter.Value, NumberStyles.Integer);
                }
                return -1;
            }
            set
            {
                CodeContracts.RequiresIsTrue(value > 0, "rport");
                this.Parameters.Set(SipParameterNames.RPort, value.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// rfc 3581
        /// </summary>
        public bool UseRport
        {
            get
            {
                var parameter = this.Parameters[SipParameterNames.RPort];
                if (parameter != null)
                {
                    return true;
                }
                return false;
            }
            set
            {
                if (!value)
                {
                    this.Parameters.Remove(SipParameterNames.RPort);
                }
                else
                {
                    this.Parameters.Set(SipParameterNames.RPort, string.Empty);
                }
            }
        }
        
        public override string FormatBodyToString()
        {
            var b = new StringBuilder();
            b.AppendFormat("{0}/{1} {2};{3}", SipConstants.SipTwoZeroString, Transport, SentBy.ToString(), Parameters.FormatToString());            
            return b.ToString();
        }
        
        public override SipHeader Clone()
        {
            var p = new SipViaHeaderParser();
            return p.Parse(this.FormatBodyToString());
        }
    }
}
