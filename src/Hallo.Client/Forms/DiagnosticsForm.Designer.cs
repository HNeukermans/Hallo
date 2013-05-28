using Hallo.Component;

namespace Hallo.Client.Forms
{
    partial class DiagnosticsForm
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
            this._grbDiagnostics = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this._lblPacketsReceived = new System.Windows.Forms.Label();
            this._lblBytesReceived = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this._lblBytesSent = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._lblPacketSent = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.usageHistorySTP = new UsageHistoryControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._usageThreadsInPool = new UsageControl();
            this._tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this._lnkResetDiagnostics = new System.Windows.Forms.LinkLabel();
            this._grpWorkItems = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this._lblQueued = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this._lblCompleted = new System.Windows.Forms.Label();
            this._grpThreads = new System.Windows.Forms.GroupBox();
            this._lblThreadsActive = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this._lblThreadsInUse = new System.Windows.Forms.Label();
            this._lbl = new System.Windows.Forms.Label();
            this._grbDiagnostics.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this._grpWorkItems.SuspendLayout();
            this._grpThreads.SuspendLayout();
            this.SuspendLayout();
            // 
            // _grbDiagnostics
            // 
            this._grbDiagnostics.Controls.Add(this._lnkResetDiagnostics);
            this._grbDiagnostics.Controls.Add(this.label5);
            this._grbDiagnostics.Controls.Add(this._lblPacketsReceived);
            this._grbDiagnostics.Controls.Add(this._lblBytesReceived);
            this._grbDiagnostics.Controls.Add(this.label7);
            this._grbDiagnostics.Controls.Add(this._lblBytesSent);
            this._grbDiagnostics.Controls.Add(this.label6);
            this._grbDiagnostics.Controls.Add(this._lblPacketSent);
            this._grbDiagnostics.Controls.Add(this.label8);
            this._grbDiagnostics.Location = new System.Drawing.Point(12, 12);
            this._grbDiagnostics.Name = "_grbDiagnostics";
            this._grbDiagnostics.Size = new System.Drawing.Size(241, 89);
            this._grbDiagnostics.TabIndex = 24;
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
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.usageHistorySTP);
            this.groupBox4.Location = new System.Drawing.Point(100, 107);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(494, 128);
            this.groupBox4.TabIndex = 39;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "STP Usage History";
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
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this._usageThreadsInPool);
            this.groupBox2.Location = new System.Drawing.Point(12, 107);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(80, 128);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "STP Usage";
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
            this._usageThreadsInPool.Load += new System.EventHandler(this._usageThreadsInPool_Load);
            // 
            // _tmrRefresh
            // 
            this._tmrRefresh.Tick += new System.EventHandler(this._tmrRefresh_Tick);
            // 
            // _lnkResetDiagnostics
            // 
            this._lnkResetDiagnostics.AutoSize = true;
            this._lnkResetDiagnostics.Location = new System.Drawing.Point(188, 63);
            this._lnkResetDiagnostics.Name = "_lnkResetDiagnostics";
            this._lnkResetDiagnostics.Size = new System.Drawing.Size(35, 13);
            this._lnkResetDiagnostics.TabIndex = 31;
            this._lnkResetDiagnostics.TabStop = true;
            this._lnkResetDiagnostics.Text = "Reset";
            this._lnkResetDiagnostics.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._lnkResetDiagnostics_LinkClicked_2);
            // 
            // _grpWorkItems
            // 
            this._grpWorkItems.Controls.Add(this._lblCompleted);
            this._grpWorkItems.Controls.Add(this.label10);
            this._grpWorkItems.Controls.Add(this._lblQueued);
            this._grpWorkItems.Controls.Add(this.label12);
            this._grpWorkItems.Location = new System.Drawing.Point(259, 12);
            this._grpWorkItems.Name = "_grpWorkItems";
            this._grpWorkItems.Size = new System.Drawing.Size(171, 89);
            this._grpWorkItems.TabIndex = 32;
            this._grpWorkItems.TabStop = false;
            this._grpWorkItems.Text = "Work Items";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "Queued";
            // 
            // _lblQueued
            // 
            this._lblQueued.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblQueued.Location = new System.Drawing.Point(89, 14);
            this._lblQueued.Name = "_lblQueued";
            this._lblQueued.Size = new System.Drawing.Size(70, 19);
            this._lblQueued.TabIndex = 32;
            this._lblQueued.Text = "XXXXXXXXX";
            this._lblQueued.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(6, 32);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 24);
            this.label10.TabIndex = 35;
            this.label10.Text = "Completed";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lblCompleted
            // 
            this._lblCompleted.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblCompleted.Location = new System.Drawing.Point(89, 35);
            this._lblCompleted.Name = "_lblCompleted";
            this._lblCompleted.Size = new System.Drawing.Size(70, 19);
            this._lblCompleted.TabIndex = 36;
            this._lblCompleted.Text = "XXXXXXXXX";
            this._lblCompleted.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _grpThreads
            // 
            this._grpThreads.Controls.Add(this._lblThreadsActive);
            this._grpThreads.Controls.Add(this.label13);
            this._grpThreads.Controls.Add(this._lblThreadsInUse);
            this._grpThreads.Controls.Add(this._lbl);
            this._grpThreads.Location = new System.Drawing.Point(436, 12);
            this._grpThreads.Name = "_grpThreads";
            this._grpThreads.Size = new System.Drawing.Size(171, 89);
            this._grpThreads.TabIndex = 37;
            this._grpThreads.TabStop = false;
            this._grpThreads.Text = "Threads";
            // 
            // _lblThreadsActive
            // 
            this._lblThreadsActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblThreadsActive.Location = new System.Drawing.Point(89, 14);
            this._lblThreadsActive.Name = "_lblThreadsActive";
            this._lblThreadsActive.Size = new System.Drawing.Size(70, 19);
            this._lblThreadsActive.TabIndex = 34;
            this._lblThreadsActive.Text = "XXXXXXXXX";
            this._lblThreadsActive.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(6, 32);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 24);
            this.label13.TabIndex = 33;
            this.label13.Text = "Used (Green)";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lblThreadsInUse
            // 
            this._lblThreadsInUse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblThreadsInUse.Location = new System.Drawing.Point(89, 34);
            this._lblThreadsInUse.Name = "_lblThreadsInUse";
            this._lblThreadsInUse.Size = new System.Drawing.Size(70, 19);
            this._lblThreadsInUse.TabIndex = 32;
            this._lblThreadsInUse.Text = "XXXXXXXXX";
            this._lblThreadsInUse.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _lbl
            // 
            this._lbl.AutoSize = true;
            this._lbl.Location = new System.Drawing.Point(6, 17);
            this._lbl.Name = "_lbl";
            this._lbl.Size = new System.Drawing.Size(68, 13);
            this._lbl.TabIndex = 20;
            this._lbl.Text = "In pool (Red)";
            // 
            // DiagnosticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 366);
            this.Controls.Add(this._grpThreads);
            this.Controls.Add(this._grpWorkItems);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this._grbDiagnostics);
            this.Name = "DiagnosticsForm";
            this.Text = "DiagnosticsForm";
            this._grbDiagnostics.ResumeLayout(false);
            this._grbDiagnostics.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this._grpWorkItems.ResumeLayout(false);
            this._grpWorkItems.PerformLayout();
            this._grpThreads.ResumeLayout(false);
            this._grpThreads.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox _grbDiagnostics;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label _lblPacketsReceived;
        private System.Windows.Forms.Label _lblBytesReceived;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label _lblBytesSent;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label _lblPacketSent;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private UsageHistoryControl usageHistorySTP;
        private System.Windows.Forms.GroupBox groupBox2;
        private UsageControl _usageThreadsInPool;
        private System.Windows.Forms.Timer _tmrRefresh;
        private System.Windows.Forms.LinkLabel _lnkResetDiagnostics;
        private System.Windows.Forms.GroupBox _grpWorkItems;
        private System.Windows.Forms.Label _lblCompleted;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label _lblQueued;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox _grpThreads;
        private System.Windows.Forms.Label _lblThreadsActive;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label _lblThreadsInUse;
        private System.Windows.Forms.Label _lbl;
    }
}