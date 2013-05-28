using System.Net.Sockets;
using Hallo.Sip;
using Hallo.UnitTest.Helpers;

namespace Hallo.UnitTest.Stubs
{
    public class FakeSipContextSource : ISipContextSource
    {
        public void Start()
        {
            
        }

        public void Stop()
        {
           
        }

        public System.Net.IPEndPoint ListeningPoint
        {
            get { return TestConstants.IpEndPoint1; }
        }
        
        public int BytesReceived
        {
            get { return 0; }
        }

        public int PacketsReceived
        {
            get { return 0; }
        }

        public Amib.Threading.ISTPPerformanceCountersReader PerformanceCountersReader
        {
            get { return null; }
        }

        /// <summary>
        /// fakes a new incoming message received from the socket that is already build up to a sipcontext.
        /// Directly invokes the observer's OnNext method.
        /// </summary>
        /// <param name="sipContext"></param>
        internal void FireNewContextReceivedEvent(SipContext sipContext)
        {
            NewContextReceived(this, new SipContextReceivedEventArgs{ Context =  sipContext});
        }

        public void SendTo(byte[] bytes, System.Net.IPEndPoint ipEndPoint)
        {
            
        }

        public int BytesSent
        {
            get { throw new System.NotImplementedException(); }
        }

        public int PacketsSent
        {
            get { throw new System.NotImplementedException(); }
        }
        
        public event System.EventHandler<SipContextReceivedEventArgs> NewContextReceived;
    }
}