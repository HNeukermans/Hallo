using System.Net;
using System.Net.Sockets;

namespace Hallo.Sdp
{
    /// <summary>
    /// Represents an IP address or a host name found in the SDP
    /// </summary>
    public class SdpConnection
    {
        /// <summary>
        /// Gets or sets the FQDN of the connection.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the address family to use when a host name is specified rather than an IP address.
        /// </summary>
        public AddressFamily HostNameIPVersion { get; set; }

        /// <summary>
        /// Gets or sets the IP address unless a HostName is supplied.
        /// </summary>
        public IPAddress IPAddress { get; set; }

        /// <summary>
        /// Gets whether the connection has been assigned a value.
        /// </summary>
        public bool IsSet 
        { 
            get { return !string.IsNullOrEmpty(HostName) || IPAddress != null; }
        }
    }
}