using System;
using System.Net;
using System.Net.Sockets;
using Amib.Threading;

namespace Hallo.Sip
{
    public interface ISipContextSource
    {
        void Start();
        void Stop();
        void SendTo(byte[] bytes, IPEndPoint ipEndPoint);
        event EventHandler<SipContextReceivedEventArgs> NewContextReceived;
        event EventHandler<ExceptionEventArgs> UnhandledException;
        IPEndPoint ListeningPoint { get; }
        ISTPPerformanceCountersReader PerformanceCountersReader { get; }
        int BytesReceived { get; }
        int PacketsReceived { get; }
        int BytesSent { get; }
        int PacketsSent { get; }
    }
}