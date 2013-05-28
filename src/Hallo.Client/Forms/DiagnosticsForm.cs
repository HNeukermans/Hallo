using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hallo.Sip;

namespace Hallo.Client.Forms
{
    public partial class DiagnosticsForm : ClientChildForm
    {
        private int _packetsSentReset;
        private int _bytesSentReset;
        private int _packetsReceivedReset;
        private int _bytesReceivedReset;
        private SipProvider _sipProvider;
        
        public DiagnosticsForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(DiagnosticsForm_Load);
            this.Closing += new CancelEventHandler(DiagnosticsForm_Closing);
        }

        void DiagnosticsForm_Closing(object sender, CancelEventArgs e)
        {
            _tmrRefresh.Stop();
        }

        void DiagnosticsForm_Load(object sender, EventArgs e)
        {
            _sipProvider = MainForm.SipProvider;
            _tmrRefresh.Start();
            _usageThreadsInPool.Maximum = MainForm.SipStack.MaxWorkerThreads;
        }

        private void _tmrRefresh_Tick(object sender, EventArgs e)
        {
            if (_sipProvider == null) return;

            var info = _sipProvider.GetDiagnosticsInfo();
            _lblBytesSent.Text = (info.BytesSent - _bytesSentReset).ToString();
            _lblBytesReceived.Text = (info.BytesReceived - _bytesReceivedReset).ToString();
            _lblPacketSent.Text = (info.PacketsSent - _packetsSentReset).ToString();
            _lblPacketsReceived.Text = (info.PacketsReceived - _packetsReceivedReset).ToString();

            _usageThreadsInPool.Value1 = info.InUseThreads;
            _usageThreadsInPool.Value2 = info.ActiveThreads;

            _lblThreadsActive.Text = info.ActiveThreads.ToString();
            _lblThreadsInUse.Text = info.InUseThreads.ToString();

            _lblQueued.Text = info.WorkItemsQueued.ToString();
            _lblCompleted.Text = info.WorkItemsProcessed.ToString();

            usageHistorySTP.AddValues(_usageThreadsInPool.Value1, _usageThreadsInPool.Value2);
        }
        
        private void _lnkResetDiagnostics_LinkClicked_2(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_sipProvider == null) return;

            var info = MainForm.SipProvider.GetDiagnosticsInfo();

            _bytesSentReset = info.BytesSent;
            _bytesReceivedReset = info.BytesReceived;
            _packetsSentReset = info.PacketsSent;
            _packetsReceivedReset = info.PacketsReceived;

            _tmrRefresh_Tick(null, null);
        }

        private void _usageThreadsInPool_Load(object sender, EventArgs e)
        {

        }

    }
}
