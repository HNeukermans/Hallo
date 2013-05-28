using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Amib.Threading;

namespace Hallo.Udp
{
    public interface IUdpServer
    {
        IPEndPoint ListeningPoint { get;  }

        void SendBytes(byte[] bytes, IPEndPoint remoteEp);
    }
}
