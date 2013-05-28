using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Hallo.Udp
{
    public class UdpReceivedPacketEventArgs : EventArgs
    {
        public byte[] DataBytes { get; set; }
        public ISocket Socket { get; set; }
        public IPEndPoint RemoteEp { get; set; }
    }
}
