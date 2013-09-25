using System.Net.Sockets;

namespace Hallo.Sdp
{
    /// <summary>
    /// Represents the "o" line from the SDP.
    /// </summary>
    public class SdpOrigin
    {
        public long SessionId { get; set; }
        public string UserName { get; set; }
        public long SessionVersion { get; set; }
        public AddressFamily AddressFamily { get; set; }
        public string NetType { get; set; }
        public string UnicastAddress { get; set; }
       
    }
}