using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics.Contracts;
using System.Threading;
using Hallo.Sip.Stack;
using Hallo.Util;
using Ninject;
using Hallo.Sip;
using Amib.Threading;

namespace Hallo.Udp
{
    public class UdpPacket
    {
        private ISocket m_pSocket = null;
        private IPEndPoint m_pRemoteEP = null;
        private byte[] m_pData = null;
        public IPEndPoint LocalEndPoint;

        public UdpPacket(byte[] data, IPEndPoint remoteEp)
        {
            m_pRemoteEP = remoteEp;
            m_pData = data;
        }

        /// <summary>
        /// The source address from where the packet came from. 
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get { return m_pRemoteEP; }
        }

        public byte[] DataBytes
        {
            get { return m_pData; }
        }
    }

    public class UdpServer : IUdpServer
    {
        private Queue<UdpPacket> _queuedPackets = null;
        private volatile bool _isRunning = false;
        private ISocket _socket = null;
        private readonly IUdpMessageProcessor _messageProcessor;
        //private readonly IUdpMessageChannelFactory _messageChannelFactory;

        public const int Mtu = 1400;
        public int BytesReceived { get; set; }
        public int PacketsReceived { get; set; }
        public int BytesSent { get; set; }
        public int PacketsSent { get; set; }
        private readonly IPEndPoint _listeningEndPoint;
        private readonly SipStack _stack;
        private int _maxMessageSize;
        private SmartThreadPool _threadPool;

        Thread _thisThread = null;

        //public UdpServer(SipStack stack, IPEndPoint listeningEndPoint) : this(stack, listeningEndPoint, new UdpSocketImpl(), new UdpMessageChannelFactory())
        //{
        //}

        public UdpServer(SipStack stack, IPEndPoint listeningEndPoint, ISocket socket, IUdpMessageProcessor messageProcessor)
        {
            CodeContracts.RequiresNotNull(messageProcessor, "messageProcessor");
            CodeContracts.RequiresNotNull(socket, "socket");
            //Contract.Requires(listeningEndPoint != null);

            _listeningEndPoint = listeningEndPoint;
            _socket = socket;
            _messageProcessor = messageProcessor;
            _stack = stack;
            _threadPool = new SmartThreadPool(Timeout.Infinite, _stack.MaxWorkerThreads, _stack.MinWorkerThreads);
            //_messageChannelFactory = messageChannelFactory;
           
            //_queuedPackets = new Queue<UdpPacket>(); 
        }

        public void Start()
        {
            if(_isRunning) return;

            _isRunning = true;

            _threadPool.Start();
            _socket.Bind(_listeningEndPoint);

            _thisThread = new Thread(ReceiveIncomingUdp);
            _thisThread.Name = "UdpServer";
            _thisThread.IsBackground = true;
            _thisThread.Start();
        }

        private void ReceiveIncomingUdp()
        {
            byte[] buffer = new byte[_stack.MaxUdpMessageSize];  

             while(_isRunning)
             {
                try
                {             
                    if(_socket.Poll(0, SelectMode.SelectRead))
                    {
                        EndPoint remoteEp = new IPEndPoint(IPAddress.Any,0);                            
                        int received = _socket.ReceiveFrom(buffer,ref remoteEp);

                        /*diagnostics*/
                        BytesReceived += received;
                        PacketsReceived++;

                        byte[] data = new byte[received];
                        Array.Copy(buffer,data,received);

                        var packet = new UdpPacket(data, (IPEndPoint)remoteEp);

                        _threadPool.QueueWorkItem(StartNewMessageChannel, packet, PostExecuteWorkItem, CallToPostExecute.Always);

                    }
                }   
                catch(Exception e)
                {
                    OnError(e);
                }
             }
        }

        //private void ProcessQueuedPackets()
        //{
        //     while(_isRunning)
        //     {
        //        try
        //        {
        //            if(IsQueueEmpty()) 
        //            {
        //                Thread.Sleep(10);
        //            }
        //            else
        //            {
        //                UdpPacket udpPacket = null;
        //                lock(_queuedPackets)
        //                {
        //                    udpPacket = _queuedPackets.Dequeue();
        //                }

        //                    ThreadPool.QueueUserWorkItem(new WaitCallback((o) =>
        //                        {
        //                            OnUdpPacketReceived(udpPacket);
        //                        }));
        //            }                   
        //        }
        //        catch(Exception e)
        //        {
        //            OnError(e);
        //        }
        //    }
        //}

        public event EventHandler<ErrorEventArgs> Error = null;
        
        private void OnError(Exception x)
        {
            if(this.Error != null)
            {
                this.Error(this, new ErrorEventArgs(new UdpServerException(x),new System.Diagnostics.StackTrace()));
            }
        }
        
        public IPEndPoint ListeningPoint
        {
            get { return _listeningEndPoint; }
        }
        
        public void Stop()
        {
            _isRunning = false;

            _threadPool.Shutdown();
            _thisThread.Join();
            _socket.Close();
        }

        private object StartNewMessageChannel(object state)
        {
            var packet = state as UdpPacket;

            //new UdpMessageChannel(packet,)
            //_channelStarter.StartChannel(packet, this);
            _messageProcessor.ProcessMessage(packet, this);
            //var channel = _messageChannelFactory.CreateChannel(packet, this, _stack);
            //channel.Start();
            return 1;
        }

        private void PostExecuteWorkItem(IWorkItemResult workItemResult)
        {

        }

        public void SendBytes(byte[] bytes, IPEndPoint remoteEp)
        {
            CodeContracts.RequiresNotNull(remoteEp, "remoteEp");
            CodeContracts.RequiresNotNull(bytes, "bytes");
            CodeContracts.RequiresIsTrue(_isRunning, "Bytes could not be send. UdpServer has stopped running.");
            CodeContracts.RequiresIsTrue(_socket != null, "Bytes could not be send. socket is null.");
            
            _socket.SendTo(bytes, remoteEp);
        }
    }
}
