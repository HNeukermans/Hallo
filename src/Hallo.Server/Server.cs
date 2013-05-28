using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using Hannes.Net.Sip;
using Hannes.Net.Sip.Stack;
using Hannes.Net.Sip.Util;
using Hannes.Net.Util;

namespace Hannes.Net.Server.WinForms
{
    public partial class Form1 : Form, ISipListener
    {
        private SipStack _stack;
        private SipProvider _sipProvider;

        public Form1()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            var localIp4Address = Dns.GetHostAddresses(string.Empty).FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);

            _txtLocalIPAddress.Text = localIp4Address.ToString() + ":33333";
            _txtSipDomain.Text = "ocean.com";
        }

        private void _btnStart_Click(object sender, EventArgs e)
        {
            var ipEndPoint = SipUtil.ParseIpEndPoint(_txtLocalIPAddress.Text);

            _stack = new SipStack();
            var listeningPoint = _stack.CreateUdpListeningPoint(ipEndPoint);
            _sipProvider = _stack.CreateSipProvider(listeningPoint);
            _sipProvider.AddSipListener(this);
            _stack.Start();
            _tmrDiagnostics.Start();
        }
       
        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            WriteToLog(">>>>" + SipFormatter.FormatMessageEnvelope(requestEvent.Request) + Environment.NewLine);
            requestEvent.Response = requestEvent.Request.CreateResponse(SipResponseCodes.x200_Ok);
            requestEvent.IsHandled = true;
            _sipProvider.SendResponse(requestEvent.Response);

            WriteToLog("<<<<" + SipFormatter.FormatMessageEnvelope(requestEvent.Response) + Environment.NewLine);
        }

        private void WriteToLog(string message)
        {
            _txtLog.Invoke(new MethodInvoker(() =>
            {
                _txtLog.Text += message;
                _txtLog.SelectionStart = _txtLog.Text.Length;
                _txtLog.ScrollToCaret();
            }));
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
            //_txtLog.Text += SipFormatter.FormatMessageEnvelope(responseEvent);
        }

        private void _btnStop_Click(object sender, EventArgs e)
        {
            _tmrDiagnostics.Stop();
            _stack.Stop();
        }

       
    }
}
