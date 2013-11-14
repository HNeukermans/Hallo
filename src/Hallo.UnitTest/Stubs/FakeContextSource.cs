using System;
using System.Net;
using System.Net.Sockets;
using Hallo.Sip;
using Hallo.UnitTest.Helpers;

namespace Hallo.UnitTest.Stubs
{
    public class FakeSipContextSource : ISipContextSource
    {
        private readonly IPEndPoint _ipEndPoint;
        private FakeNetwork _network;

        public FakeSipContextSource()
        {
            _ipEndPoint = TestConstants.IpEndPoint1;
        }

        public FakeSipContextSource(IPEndPoint listeningPoint)
        {
            _ipEndPoint = listeningPoint;
        }

        public void Start()
        {
            
        }

        public void Stop()
        {
           
        }

        /// <summary>
        /// Registers itself as a receiver to the network
        /// </summary>
        /// <param name="network"></param>
        public void AddToNetwork(FakeNetwork network)
        {
            _network = network;
            _network.AddReceiver(this.ListeningPoint, FireNewContextReceivedEvent);
        }

        public System.Net.IPEndPoint ListeningPoint
        {
            get { return _ipEndPoint; }
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
        /// Directly invokes the SipProvider's OnNext method.
        /// </summary>
        /// <param name="sipContext"></param>
        internal void FireNewContextReceivedEvent(SipContext sipContext)
        {
            /*let the SipProvider know*/
            try
            {
                NewContextReceived(this, new SipContextReceivedEventArgs { Context = sipContext });
            }
            catch (Exception e)
            {
                UnhandledException(this,new ExceptionEventArgs(){ Exception = e});
            }
        }

        public void SendTo(byte[] bytes, System.Net.IPEndPoint ipEndPoint)
        {
            if(_network != null)
            {
                _network.SendTo(bytes, ListeningPoint, ipEndPoint);
            }
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


        public event System.EventHandler<ExceptionEventArgs> UnhandledException;
    }
}