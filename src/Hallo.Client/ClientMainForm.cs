using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using Hallo.Client.Forms;
using Hallo.Client.Logic;
using Hallo.Component.Forms;
using Hallo.Component.Logic;
using Hallo.Component.Logic.Extensions;
using Hallo.Component.Logic.Reactive;
using Hallo.Sip;
using Hallo.Sip.Util;

namespace Hallo.Client
{
    public partial class ClientMainForm : CoreMainForm, IExceptionHandler
    {
        public ClientConfiguration Configuration { get; set; }
        private bool _isStarted;
        
        public SipPipeLineListener MainSipListener { get; set; }

        public ClientMainForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(MainForm_Load);
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            _lblPID.Text = string.Format("PID:{0}", Process.GetCurrentProcess().Id);

            Configuration = new ClientConfiguration();
            var localIp4Address = Dns.GetHostAddresses(string.Empty).FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
            Configuration.OutboundProxyIpEndPoint = localIp4Address + ":33333";
            Configuration.BindIpEndPoint = localIp4Address + ":" + (22220 + ProcessLogic.CountProcessesByName(Process.GetCurrentProcess().ProcessName));

            ExecuteActionHelper.ExecuteAction(delegate()
            {
                FormsManager.Configure(this, _toolStripOpenedForms);
            });
        }

        private void _btnLog_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(delegate()
            {
                FormsManager.OpenForm(typeof(LogForm), null);
            });
        }

        private void _btnDiagnostics_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(delegate()
            {
                FormsManager.OpenForm(typeof(DiagnosticsForm), null);
            });
        }
        
        
        private void _btnStartStop_Click(object sender, EventArgs e)
        {
            if (!_isStarted)
            {
                var ipEndPoint = SipUtil.ParseIpEndPoint(Configuration.BindIpEndPoint);
                var outboundProxy = SipUtil.ParseIpEndPoint(Configuration.OutboundProxyIpEndPoint);
                SipStack = new SipStack();
                var listeningPoint = SipStack.CreateUdpListeningPoint(ipEndPoint);
                SipStack.MaxWorkerThreads = Configuration.MaxThreadPoolSize;
                SipStack.MinWorkerThreads = Configuration.MinThreadPoolSize;
                SipStack.OutBoundProxy = outboundProxy;
                SipStack.EnableThreadPoolPerformanceCounters = Configuration.EnableThreadPoolPerformanceCounters;
                //SipStack.IsStateFull = Configuration.IsStateFull;
                SipProvider = SipStack.CreateSipProvider(listeningPoint);
                MainSipListener = new SipPipeLineListener(this);
                SipProvider.AddSipListener(MainSipListener);
                SipProvider.AddExceptionHandler(this);
                SipStack.Start();


                HeaderFactory = SipStack.CreateHeaderFactory();
                MessageFactory = SipStack.CreateMessageFactory();
                AddressFactory = SipStack.CreateAddressFactory();
                
                ExecuteActionHelper.ExecuteAction(delegate()
                {
                    FormsManager.OpenForm(typeof(LogForm), null);
                });

                _lblIpAddress.Text = string.Format("IP:{0}", ipEndPoint.ToString());
            }
            else
            {
                SipStack.Stop();
            }

            _isStarted = !_isStarted;
            _btnStartStop.Text = _isStarted ? "Stop" : "Start";
            _grbNavigation.Enabled = _isStarted;
        }

        private void _btnConfigure_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(delegate()
            {
                FormsManager.OpenForm(typeof(ConfigurationForm), null);
            });
        }

        private void _btnSend_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(delegate()
            {
                FormsManager.OpenForm(typeof(SendForm), null);
            });
        }


        internal void HandleException(Exception ex)
        {
            EventAggregator.Instance.Publish(new ExceptionEvent(ex));
        }

        private void _btnSendString_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(delegate()
            {
                FormsManager.OpenForm(typeof(SendStringForm), null);
            });
        }

        private void _btnTxDiagnostics_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(delegate()
            {
                FormsManager.OpenForm(typeof(TxDiagnosticsForm), null);
            });
        }

        private void _btnRegister_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(delegate()
            {
                FormsManager.OpenForm(typeof(RegisterForm), null);
            });
        }

        private void _btnDialog_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(delegate()
            {
                FormsManager.OpenForm(typeof(DialogForm), null);
            });
        }

        private void _btnPhone_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(delegate()
            {
                FormsManager.OpenForm(typeof(PhoneForm), null);
            });
        }
        
        public new void Handle(Exception e)
        {
            //create the from, or bring it to front
            this.OnUIThread(()=>
            {
                ExecuteActionHelper.ExecuteAction(delegate()
                {
                    FormsManager.OpenForm(typeof (ErrorForm),null);
                });
            });

            EventAggregator.Instance.Publish(new ExceptionEvent(e));
        }
    }
}
