namespace Hannes.Net.Server.WinForms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label4 = new System.Windows.Forms.Label();
            this._txtSipDomain = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._btnStart = new System.Windows.Forms.Button();
            this._txtLocalIPAddress = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._txtLog = new System.Windows.Forms.TextBox();
            this._btnStop = new System.Windows.Forms.Button();
            this._grbDiagnostics = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this._lnkResetDiagnostics = new System.Windows.Forms.LinkLabel();
            this._lblPacketsReceived = new System.Windows.Forms.Label();
            this._lblBytesReceived = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this._lblBytesSent = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._lblPacketSent = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this._tmrDiagnostics = new System.Windows.Forms.Timer(this.components);
            this.usageHistorySTP = new UsageControl.UsageHistoryControl();
            this._usageThreadsInPool = new UsageControl.UsageControl();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.groupBox1.SuspendLayout();
            this._grbDiagnostics.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Local IP Address:";
            // 
            // _txtSipDomain
            // 
            this._txtSipDomain.Location = new System.Drawing.Point(107, 12);
            this._txtSipDomain.Name = "_txtSipDomain";
            this._txtSipDomain.Size = new System.Drawing.Size(139, 20);
            this._txtSipDomain.TabIndex = 13;
            this._txtSipDomain.Text = "ocean.com";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Sip Domain:";
            // 
            // _btnStart
            // 
            this._btnStart.Location = new System.Drawing.Point(265, 10);
            this._btnStart.Name = "_btnStart";
            this._btnStart.Size = new System.Drawing.Size(75, 23);
            this._btnStart.TabIndex = 9;
            this._btnStart.Text = "Start";
            this._btnStart.UseVisualStyleBackColor = true;
            this._btnStart.Click += new System.EventHandler(this._btnStart_Click);
            // 
            // _txtLocalIPAddress
            // 
            this._txtLocalIPAddress.Location = new System.Drawing.Point(108, 42);
            this._txtLocalIPAddress.Name = "_txtLocalIPAddress";
            this._txtLocalIPAddress.Size = new System.Drawing.Size(139, 20);
            this._txtLocalIPAddress.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._txtLog);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(3, 246);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(712, 96);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log";
            // 
            // _txtLog
            // 
            this._txtLog.AcceptsReturn = true;
            this._txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtLog.Location = new System.Drawing.Point(3, 16);
            this._txtLog.Multiline = true;
            this._txtLog.Name = "_txtLog";
            this._txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtLog.Size = new System.Drawing.Size(706, 77);
            this._txtLog.TabIndex = 19;
            // 
            // _btnStop
            // 
            this._btnStop.Location = new System.Drawing.Point(265, 40);
            this._btnStop.Name = "_btnStop";
            this._btnStop.Size = new System.Drawing.Size(75, 23);
            this._btnStop.TabIndex = 19;
            this._btnStop.Text = "Stop";
            this._btnStop.UseVisualStyleBackColor = true;
            this._btnStop.Click += new System.EventHandler(this._btnStop_Click);
            // 
            // _grbDiagnostics
            // 
            this._grbDiagnostics.Controls.Add(this.label5);
            this._grbDiagnostics.Controls.Add(this._lnkResetDiagnostics);
            this._grbDiagnostics.Controls.Add(this._lblPacketsReceived);
            this._grbDiagnostics.Controls.Add(this._lblBytesReceived);
            this._grbDiagnostics.Controls.Add(this.label7);
            this._grbDiagnostics.Controls.Add(this._lblBytesSent);
            this._grbDiagnostics.Controls.Add(this.label6);
            this._grbDiagnostics.Controls.Add(this._lblPacketSent);
            this._grbDiagnostics.Controls.Add(this.label8);
            this._grbDiagnostics.Location = new System.Drawing.Point(346, 10);
            this._grbDiagnostics.Name = "_grbDiagnostics";
            this._grbDiagnostics.Size = new System.Drawing.Size(241, 89);
            this._grbDiagnostics.TabIndex = 23;
            this._grbDiagnostics.TabStop = false;
            this._grbDiagnostics.Text = "Diagnostics";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(188, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "/";
            // 
            // _lnkResetDiagnostics
            // 
            this._lnkResetDiagnostics.AutoSize = true;
            this._lnkResetDiagnostics.Location = new System.Drawing.Point(188, 61);
            this._lnkResetDiagnostics.Name = "_lnkResetDiagnostics";
            this._lnkResetDiagnostics.Size = new System.Drawing.Size(35, 13);
            this._lnkResetDiagnostics.TabIndex = 28;
            this._lnkResetDiagnostics.TabStop = true;
            this._lnkResetDiagnostics.Text = "Reset";
            this._lnkResetDiagnostics.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._lnkResetDiagnostics_LinkClicked);
            // 
            // _lblPacketsReceived
            // 
            this._lblPacketsReceived.AutoSize = true;
            this._lblPacketsReceived.Location = new System.Drawing.Point(206, 17);
            this._lblPacketsReceived.Name = "_lblPacketsReceived";
            this._lblPacketsReceived.Size = new System.Drawing.Size(13, 13);
            this._lblPacketsReceived.TabIndex = 27;
            this._lblPacketsReceived.Text = "0";
            // 
            // _lblBytesReceived
            // 
            this._lblBytesReceived.AutoSize = true;
            this._lblBytesReceived.Location = new System.Drawing.Point(137, 17);
            this._lblBytesReceived.Name = "_lblBytesReceived";
            this._lblBytesReceived.Size = new System.Drawing.Size(13, 13);
            this._lblBytesReceived.TabIndex = 24;
            this._lblBytesReceived.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(188, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "/";
            // 
            // _lblBytesSent
            // 
            this._lblBytesSent.AutoSize = true;
            this._lblBytesSent.Location = new System.Drawing.Point(137, 38);
            this._lblBytesSent.Name = "_lblBytesSent";
            this._lblBytesSent.Size = new System.Drawing.Size(13, 13);
            this._lblBytesSent.TabIndex = 22;
            this._lblBytesSent.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Sent Bytes/Packets:";
            // 
            // _lblPacketSent
            // 
            this._lblPacketSent.AutoSize = true;
            this._lblPacketSent.Location = new System.Drawing.Point(206, 37);
            this._lblPacketSent.Name = "_lblPacketSent";
            this._lblPacketSent.Size = new System.Drawing.Size(13, 13);
            this._lblPacketSent.TabIndex = 25;
            this._lblPacketSent.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(132, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = " Received Bytes/Packets:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this._usageThreadsInPool);
            this.groupBox2.Location = new System.Drawing.Point(24, 108);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(80, 128);
            this.groupBox2.TabIndex = 36;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "STP Usage";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.usageHistorySTP);
            this.groupBox4.Location = new System.Drawing.Point(112, 108);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(494, 128);
            this.groupBox4.TabIndex = 37;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "STP Usage History";
            // 
            // _tmrDiagnostics
            // 
            this._tmrDiagnostics.Tick += new System.EventHandler(this.GetDiagnosticInfo_Tick);
            // 
            // usageHistorySTP
            // 
            this.usageHistorySTP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.usageHistorySTP.BackColor = System.Drawing.Color.Black;
            this.usageHistorySTP.Location = new System.Drawing.Point(8, 16);
            this.usageHistorySTP.Maximum = 25;
            this.usageHistorySTP.Name = "usageHistorySTP";
            this.usageHistorySTP.Size = new System.Drawing.Size(480, 104);
            this.usageHistorySTP.TabIndex = 0;
            // 
            // _usageThreadsInPool
            // 
            this._usageThreadsInPool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._usageThreadsInPool.BackColor = System.Drawing.Color.Black;
            this._usageThreadsInPool.Location = new System.Drawing.Point(18, 18);
            this._usageThreadsInPool.Maximum = 25;
            this._usageThreadsInPool.Name = "_usageThreadsInPool";
            this._usageThreadsInPool.Size = new System.Drawing.Size(41, 104);
            this._usageThreadsInPool.TabIndex = 37;
            this._usageThreadsInPool.Value1 = 1;
            this._usageThreadsInPool.Value2 = 24;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(537, 87);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(178, 151);
            this.propertyGrid1.TabIndex = 38;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 345);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this._grbDiagnostics);
            this.Controls.Add(this._btnStop);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._txtLocalIPAddress);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._txtSipDomain);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._btnStart);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "Server";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this._grbDiagnostics.ResumeLayout(false);
            this._grbDiagnostics.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox _txtSipDomain;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button _btnStart;
        private System.Windows.Forms.TextBox _txtLocalIPAddress;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox _txtLog;
        private System.Windows.Forms.Button _btnStop;
        private System.Windows.Forms.GroupBox _grbDiagnostics;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel _lnkResetDiagnostics;
        private System.Windows.Forms.Label _lblPacketsReceived;
        private System.Windows.Forms.Label _lblBytesReceived;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label _lblBytesSent;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label _lblPacketSent;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private UsageControl.UsageControl _usageThreadsInPool;
        private System.Windows.Forms.GroupBox groupBox4;
        private UsageControl.UsageHistoryControl usageHistorySTP;
        private System.Windows.Forms.Timer _tmrDiagnostics;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
    }
}

