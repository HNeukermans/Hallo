namespace Hallo.Udp
{
    public interface IUdpMessageChannel
    {
        UdpMessageChannelReadResult Read(UdpPacket packet, IUdpServer server);
    }
}