using Hannes.Net.Sip.Stack;

namespace Hannes.Net.Udp
{
    public interface IUdpMessageChannelStarter
    {
        void StartChannel(UdpPacket packet, UdpMessageProcessor udpMessageProcesor);
    }
}