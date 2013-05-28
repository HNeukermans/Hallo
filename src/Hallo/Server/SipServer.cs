using System;
using System.Net;
using Hallo.Sip;
using Hallo.Sip.Util;
using Hallo.Util;

namespace Hallo.Server
{
    public class SipServer
    {
        private SipStack _stack;
        private SipProvider _provider;
        private SipServerListener _listener;
        private SipServerConfiguration _configuration;
        private IPEndPoint _ipEndPoint;
        private SipRegistrar _registrar;

        public void Configure(SipServerConfiguration configuration)
        {
            Check.Require(configuration, "configuration");
            _configuration = configuration;
            _ipEndPoint = SipUtil.ParseIpEndPoint(_configuration.BindIpEndPoint);
            
        }

        public void Start()
        {
            if(_configuration == null) throw new InvalidOperationException("The server is not configured.");

            _stack = new SipStack();
            _stack.MaxWorkerThreads = _configuration.MaxThreadPoolSize;
            _stack.MinWorkerThreads = _configuration.MinThreadPoolSize;
            _stack.EnableThreadPoolPerformanceCounters = _configuration.EnableThreadPoolPerformanceCounters;
             var listeningPoint = _stack.CreateUdpListeningPoint(_ipEndPoint);
            _provider = _stack.CreateSipProvider(listeningPoint);
            _listener = new SipServerListener();
            _registrar = InitializeRegistrar();
            _listener.AddRequestHandler(_registrar);
            _provider.AddSipListener(_listener);
            _stack.Start();
        }

        public void Stop()
        {
            _stack.Stop();
        }

        /// <summary>
        /// extension point. Allows for custom handlers to plugin into the server processing.
        /// </summary>
        /// <param name="handler"></param>
        public void AddRequestHandler(ISipRequestHandler handler)
        {
            Check.Require(handler, "handler");
            _listener.AddRequestHandler(handler);
        }

        private SipRegistrar InitializeRegistrar()
        {
            var settings = new SipRegistrarSettings();
            settings.DefaultExpires = _configuration.RegistrarDefaultExpires;
            settings.MinimumExpires = _configuration.RegistrarMinimumExpires;
            settings.Domain = _configuration.RegistrarDomain;
            return new SipRegistrar(settings);
        }

        public SipProviderDiagnosticInfo GetDiagnosticsInfo()
        {
            return _provider.GetDiagnosticsInfo();
        }

        public IObservable<SipAddressBindingDiagnosticsInfo> GetRegistrarDiagnosticsInfo()
        {
            return _registrar.ObserveDiagnostics();
        }
    }
}