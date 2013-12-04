using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Diagnostics.Contracts;
using System.Threading;
using Amib.Threading;
using Hallo.Sip.Stack;
using NLog;
using Hallo.Util;

namespace Hallo.Sip
{
    public class SipStack
    {
        private ITimerFactory _txTimerFactory;
        
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
       
        public const int MAX_DATAGRAM_SIZE = 64 * 1024;

        public IPEndPoint ProxyServer { get; set; }

        
        
        private int _maxUdpMessageSize = 1400;
        private int _maxWorkerThreads = SmartThreadPool.DefaultMaxWorkerThreads;
        private int _minWorkerThreads = SmartThreadPool.DefaultMinWorkerThreads;
        private bool _enableThreadPoolPerformanceCounters;
        private SmartThreadPool _threadPool;
        private IPEndPoint _outBoundProxy;
        private SipMessageFactory _messageFactory;
        private SipHeaderFactory _headerFactory;
        private SipAddressFactory _addressFactory;

        public int MaxUdpMessageSize 
        {
            get { return _maxUdpMessageSize; }
            set 
            {
                Check.GreaterThenOrEqual(value, 0, "MaxUdpMessageSize");
                Check.IsTrue(value <= MAX_DATAGRAM_SIZE, "MaxUdpMessageSize can not be greater then 'MAX_DATAGRAM_SIZE'");

                _maxUdpMessageSize = value; 
            }
        }

        public int MaxWorkerThreads
        {
            get
            { 
                return _maxWorkerThreads;
            }
            set
            {
                Check.GreaterThenOrEqual(value, 0, "MaxWorkerThreads");
                _maxWorkerThreads = value;
            }
        }

        public int MinWorkerThreads
        {
            get
            {
                return _minWorkerThreads;
            }
            set
            {
                Check.GreaterThenOrEqual(value, 0, "MinWorkerThreads");
                _minWorkerThreads = value;
            }
        }

        public IPEndPoint OutBoundProxy
        {
            get
            {
                return _outBoundProxy;
            }
            set
            {
                Check.Require(value, "OutBoundProxy");
                _outBoundProxy = value;
            }
        } 
        
        public bool EnableThreadPoolPerformanceCounters
        {
            get { return _enableThreadPoolPerformanceCounters; }
            set { _enableThreadPoolPerformanceCounters = value; }
        }
        
        internal SmartThreadPool CreateThreadPool()
        {
            
            var startInfo = new STPStartInfo();
            startInfo.MaxWorkerThreads = _maxWorkerThreads;
            startInfo.MinWorkerThreads = _minWorkerThreads;
            startInfo.EnableLocalPerformanceCounters = _enableThreadPoolPerformanceCounters;
            startInfo.IdleTimeout = Timeout.Infinite;
            _threadPool = new SmartThreadPool(startInfo);

            return _threadPool;
        }

        public SipListeningPoint CreateUdpListeningPoint(IPAddress address, int port)
        {
            return new SipListeningPoint(address, port);
        }

        public SipListeningPoint CreateUdpListeningPoint(IPEndPoint endPoint)
        {
            return CreateUdpListeningPoint(endPoint.Address, endPoint.Port);
        }

        public ISipProvider CreateSipProvider(SipListeningPoint listeningPoint)
        {
            Check.Require(listeningPoint, "listeningPoint");
            
            var contextSource = new SipContextSource(
                listeningPoint,
                CreateThreadPool(),
                CreateMessageFactory(),
                CreateHeaderFactory());

            var sipProvider = new SipProvider(this, contextSource);
            
            return sipProvider;
        }

        public Stack.SipHeaderFactory CreateHeaderFactory()
        {
            return _headerFactory ?? (_headerFactory = new SipHeaderFactory());
        }

        public SipMessageFactory CreateMessageFactory()
        {
            return _messageFactory ?? (_messageFactory = new SipMessageFactory());
        }

        public SipAddressFactory CreateAddressFactory()
        {
            return _addressFactory ?? (_addressFactory = new SipAddressFactory());
        }

        public void SetTimerFactory(ITimerFactory timerFactory)
        {
            _txTimerFactory = timerFactory;
        }

        public ITimerFactory GetTimerFactory()
        {
            return _txTimerFactory ?? (_txTimerFactory = new TimerFactory());
        }

        //public void Start()
        //{
        //    if (_state == SipStackState.Started) return;

        //    if(_sipProvider == null) throw new InvalidOperationException("Create an SipProvider first.");
        //    CreateThreadPool().Start();
        //    _sipProvider.Start();
        //    _state = SipStackState.Started;
        //}

        //public void Stop()
        //{
        //    _state = SipStackState.Stopped;
        //    _sipProvider.Stop();
        //    _threadPool.Shutdown();
        //}

        
    }
}
