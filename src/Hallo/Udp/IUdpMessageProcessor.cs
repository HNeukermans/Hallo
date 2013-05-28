namespace Hallo.Udp
{
    public interface IUdpMessageProcessor
    {
        void ProcessMessage(UdpPacket packet, UdpServer server);
    }
}