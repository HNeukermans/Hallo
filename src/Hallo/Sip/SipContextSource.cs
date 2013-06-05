using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Amib.Threading;
using Hallo.Sip.Stack;
using NLog;
using Hallo.Util;

namespace Hallo.Sip
{
    /// <summary>
    /// listens for incoming sip messages received on a udp socket
    /// </summary>
    public class SipContextSource : ISipContextSource
    {
        private readonly IPEndPoint _ipEndPoint;
        private bool _isRunning;
        private readonly SmartThreadPool _threadPool;
        private Socket _socket;
        private Thread _thread;
        private volatile int _bytesSent;
        private volatile int _packetsSent;
        private readonly object _locker = new object();
        private DatagramSender _sender;
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly SipContextFactory _contextFactory;

        public int BytesReceived { get; private set; }
        public int PacketsReceived { get; private set; }
        public int PacketsSent
        {
            get { return _packetsSent; }
        }

       

        /// <summary>
        /// sends a messaege to the socket.
        /// </summary>
        /// <remarks>externalizing the socket would require to mock it.</remarks>
        /// <param name="bytes"></param>
        /// <param name="ipEndPoint"></param>
        public void SendTo(byte[] bytes, IPEndPoint ipEndPoint)
        {
            if(_sender.SendTo(bytes, ipEndPoint))
            {
                lock (_locker)
                {
                    _bytesSent += bytes.Length;
                    _packetsSent++;
                }
            }

            //Check.Require(bytes, "bytes");
            //Check.Require(ipEndPoint, "ipEndPoint");

            //_logger.Trace("Sending message from: '{0}' --> '{1}'", ListeningPoint, ipEndPoint);

            //_socket.SendTo(bytes, ipEndPoint);
        }

        public int BytesSent
        {
            get { return _bytesSent; }
        }

        /// <summary>
        /// ListeningPoint
        /// </summary>
        public IPEndPoint ListeningPoint
        {
            get { return _ipEndPoint; }
        }

        internal SipContextSource(IPEndPoint ipEndPoint, SmartThreadPool threadPool, SipMessageFactory messageFactory, SipHeaderFactory headerFactory)
        {
            Check.Require(ipEndPoint, "ipEndPoint");
            Check.Require(threadPool, "threadPool");
            Check.Require(messageFactory, "messageFactory");
            Check.Require(headerFactory, "headerFactory");

            _ipEndPoint = ipEndPoint;
            _threadPool = threadPool;
            _contextFactory = new SipContextFactory(messageFactory, headerFactory);
            _logger.Trace("Constructor called.");

            _sender = new DatagramSender(_ipEndPoint);
        }
        
        public void Start()
        {
            _logger.Trace("Starting udplistener. Trying to bind to IpEndPoint: {0}", _ipEndPoint);

            if (_isRunning)
            {
                _logger.Warn("Start was called on an already started instance.");
                return;
            }
            
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
            _socket.Bind(_ipEndPoint);

            _thread = new Thread(ReceiveIncomingDatagram);
            _thread.Name = "SipContextSource";
            _thread.IsBackground = true;
            _thread.Start();

            _isRunning = true;

            _logger.Info("Udplistener started. Bound at IPEndPoint: {0}", _ipEndPoint);
        }

        public ISTPPerformanceCountersReader PerformanceCountersReader
        {
            get { return _threadPool.PerformanceCountersReader; }
        }

        private void ReceiveIncomingDatagram()
        {
            var buffer = new byte[1024];

            while (_isRunning)
            {
                try
                {
                    if (_socket.Poll(0, SelectMode.SelectRead))
                    {
                        EndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);
                        int received = _socket.ReceiveFrom(buffer, ref remoteEp);

                        /*diagnostics*/
                        BytesReceived += received;
                        PacketsReceived++;

                        byte[] data = new byte[received];
                        Array.Copy(buffer, data, received);

                        var datagram = new Datagram(data, _ipEndPoint, (IPEndPoint) remoteEp);

                        _threadPool.QueueWorkItem(()=> ProcessIncomingDatagram(datagram));

                    }
                }
                catch (SocketException se)
                {
                    _logger.ErrorException(string.Format("Failed to receive message from socket. SocketErrorCode: {0}, ErrorCode: {1}", se.SocketErrorCode, se.ErrorCode), se);
                }
                catch (Exception e)
                {
                    _logger.ErrorException("Failed to receive message", e);
                }

                Thread.Sleep(10);
            }
        }

        private object ProcessIncomingDatagram(object argument)
        {
            var datagram = argument as Datagram;

            _logger.Trace("Processing incoming datagram ...");

            try
            {
                var c = _contextFactory.CreateContext(datagram);
                NewContextReceived(this, new SipContextReceivedEventArgs(){ Context = c });
            }
            catch (Exception err)
            {
                _logger.ErrorException("Failed starting a new SipContext instance. Exception: ", err);
            }
            return 1;
        }

        public void Stop()
        {
            _isRunning = false;
            _thread.Join();
            _socket.Close();
        }

        public event EventHandler<SipContextReceivedEventArgs> NewContextReceived = delegate { };
    }
}