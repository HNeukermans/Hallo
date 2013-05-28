using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hallo.Component.Forms;
using Hallo.Component.Logic;
using Hallo.Server.Forms;
using Hallo.Server.Logic;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Util;

namespace Hallo.Server
{
    public partial class ServerMainForm : CoreMainForm
    {
        private ISipListener _sipListener;
        public SipServerConfiguration Configuration { get; set; }
        public SipServer Server { get; set; }

        public ServerMainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Configuration = new SipServerConfiguration();

            ExecuteActionHelper.ExecuteAction(delegate()
            {
                FormsManager.Configure(this, _toolStripOpenedForms);
            });
        }

        private void _btnConfigure_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(delegate()
            {
                FormsManager.OpenForm(typeof(ConfigurationForm), null);
            });
        }

        private bool _isStarted = false;

        private void _btnStartStop_Click(object sender, EventArgs e)
        {
            _isStarted = !_isStarted;

            if(_isStarted)
            {
                Server = new SipServer();
                Server.Configure(Configuration);
                Server.Start();
            }
            else
            {
                Server.Stop();
            }
            _btnStartStop.Text = _isStarted ? "Stop" : "Start";
            _grbNavigation.Enabled = _isStarted;
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

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void _btnTxDiagnostics_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(delegate()
            {
                FormsManager.OpenForm(typeof(TxDiagnosticsForm), null);
            });
        }

        private void _btnRegistrar_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(delegate()
            {
                FormsManager.OpenForm(typeof(RegistrarForm), null);
            });
        }

        


        
    }
}
