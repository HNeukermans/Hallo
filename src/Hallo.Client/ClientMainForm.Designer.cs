namespace Hallo.Client
{
    partial class ClientMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientMainForm));
            this._btnLog = new System.Windows.Forms.Button();
            this._btnDiagnostics = new System.Windows.Forms.Button();
            this._btnStartStop = new System.Windows.Forms.Button();
            this._btnConfigure = new System.Windows.Forms.Button();
            this._btnSend = new System.Windows.Forms.Button();
            this._btnSendString = new System.Windows.Forms.Button();
            this._btnTxDiagnostics = new System.Windows.Forms.Button();
            this._btnRegister = new System.Windows.Forms.Button();
            this._btnDialog = new System.Windows.Forms.Button();
            this.@__btnPhone = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this._lblIpAddress = new System.Windows.Forms.Label();
            this._lblPID = new System.Windows.Forms.Label();
            this._pnlNavigation.SuspendLayout();
            this._grbActions.SuspendLayout();
            this._grbNavigation.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pnlNavigation
            // 
            this._pnlNavigation.Controls.Add(this.panel1);
            this._pnlNavigation.Location = new System.Drawing.Point(0, 25);
            this._pnlNavigation.Size = new System.Drawing.Size(125, 461);
            this._pnlNavigation.Controls.SetChildIndex(this.panel1, 0);
            this._pnlNavigation.Controls.SetChildIndex(this._grbActions, 0);
            this._pnlNavigation.Controls.SetChildIndex(this._grbNavigation, 0);
            // 
            // _grbActions
            // 
            this._grbActions.Controls.Add(this._btnConfigure);
            this._grbActions.Controls.Add(this._btnStartStop);
            this._grbActions.Dock = System.Windows.Forms.DockStyle.Top;
            this._grbActions.Location = new System.Drawing.Point(0, 37);
            this._grbActions.Size = new System.Drawing.Size(125, 83);
            // 
            // _grbNavigation
            // 
            this._grbNavigation.Controls.Add(this.@__btnPhone);
            this._grbNavigation.Controls.Add(this._btnDialog);
            this._grbNavigation.Controls.Add(this._btnRegister);
            this._grbNavigation.Controls.Add(this._btnTxDiagnostics);
            this._grbNavigation.Controls.Add(this._btnSendString);
            this._grbNavigation.Controls.Add(this._btnSend);
            this._grbNavigation.Controls.Add(this._btnLog);
            this._grbNavigation.Controls.Add(this._btnDiagnostics);
            this._grbNavigation.Dock = System.Windows.Forms.DockStyle.Top;
            this._grbNavigation.Location = new System.Drawing.Point(0, 120);
            this._grbNavigation.Size = new System.Drawing.Size(125, 232);
            // 
            // _btnLog
            // 
            this._btnLog.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnLog.Location = new System.Drawing.Point(3, 42);
            this._btnLog.Name = "_btnLog";
            this._btnLog.Size = new System.Drawing.Size(119, 26);
            this._btnLog.TabIndex = 8;
            this._btnLog.Text = "Log";
            this._btnLog.UseVisualStyleBackColor = true;
            this._btnLog.Click += new System.EventHandler(this._btnLog_Click);
            // 
            // _btnDiagnostics
            // 
            this._btnDiagnostics.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnDiagnostics.Location = new System.Drawing.Point(3, 16);
            this._btnDiagnostics.Name = "_btnDiagnostics";
            this._btnDiagnostics.Size = new System.Drawing.Size(119, 26);
            this._btnDiagnostics.TabIndex = 9;
            this._btnDiagnostics.Text = "Diagnostics";
            this._btnDiagnostics.UseVisualStyleBackColor = true;
            this._btnDiagnostics.Click += new System.EventHandler(this._btnDiagnostics_Click);
            // 
            // _btnStartStop
            // 
            this._btnStartStop.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnStartStop.Location = new System.Drawing.Point(3, 16);
            this._btnStartStop.Name = "_btnStartStop";
            this._btnStartStop.Size = new System.Drawing.Size(119, 26);
            this._btnStartStop.TabIndex = 7;
            this._btnStartStop.Text = "Start";
            this._btnStartStop.UseVisualStyleBackColor = true;
            this._btnStartStop.Click += new System.EventHandler(this._btnStartStop_Click);
            // 
            // _btnConfigure
            // 
            this._btnConfigure.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnConfigure.Location = new System.Drawing.Point(3, 42);
            this._btnConfigure.Name = "_btnConfigure";
            this._btnConfigure.Size = new System.Drawing.Size(119, 26);
            this._btnConfigure.TabIndex = 8;
            this._btnConfigure.Text = "Configure";
            this._btnConfigure.UseVisualStyleBackColor = true;
            this._btnConfigure.Click += new System.EventHandler(this._btnConfigure_Click);
            // 
            // _btnSend
            // 
            this._btnSend.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnSend.Location = new System.Drawing.Point(3, 68);
            this._btnSend.Name = "_btnSend";
            this._btnSend.Size = new System.Drawing.Size(119, 26);
            this._btnSend.TabIndex = 10;
            this._btnSend.Text = "Send";
            this._btnSend.UseVisualStyleBackColor = true;
            this._btnSend.Click += new System.EventHandler(this._btnSend_Click);
            // 
            // _btnSendString
            // 
            this._btnSendString.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnSendString.Location = new System.Drawing.Point(3, 94);
            this._btnSendString.Name = "_btnSendString";
            this._btnSendString.Size = new System.Drawing.Size(119, 26);
            this._btnSendString.TabIndex = 11;
            this._btnSendString.Text = "Send String";
            this._btnSendString.UseVisualStyleBackColor = true;
            this._btnSendString.Click += new System.EventHandler(this._btnSendString_Click);
            // 
            // _btnTxDiagnostics
            // 
            this._btnTxDiagnostics.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnTxDiagnostics.Location = new System.Drawing.Point(3, 120);
            this._btnTxDiagnostics.Name = "_btnTxDiagnostics";
            this._btnTxDiagnostics.Size = new System.Drawing.Size(119, 26);
            this._btnTxDiagnostics.TabIndex = 12;
            this._btnTxDiagnostics.Text = "Tx Diagnostics";
            this._btnTxDiagnostics.UseVisualStyleBackColor = true;
            this._btnTxDiagnostics.Click += new System.EventHandler(this._btnTxDiagnostics_Click);
            // 
            // _btnRegister
            // 
            this._btnRegister.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnRegister.Location = new System.Drawing.Point(3, 146);
            this._btnRegister.Name = "_btnRegister";
            this._btnRegister.Size = new System.Drawing.Size(119, 26);
            this._btnRegister.TabIndex = 13;
            this._btnRegister.Text = "Register";
            this._btnRegister.UseVisualStyleBackColor = true;
            this._btnRegister.Click += new System.EventHandler(this._btnRegister_Click);
            // 
            // _btnDialog
            // 
            this._btnDialog.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnDialog.Location = new System.Drawing.Point(3, 172);
            this._btnDialog.Name = "_btnDialog";
            this._btnDialog.Size = new System.Drawing.Size(119, 23);
            this._btnDialog.TabIndex = 14;
            this._btnDialog.Text = "Dialog";
            this._btnDialog.UseVisualStyleBackColor = true;
            this._btnDialog.Click += new System.EventHandler(this._btnDialog_Click);
            // 
            // __btnPhone
            // 
            this.@__btnPhone.Dock = System.Windows.Forms.DockStyle.Top;
            this.@__btnPhone.Location = new System.Drawing.Point(3, 195);
            this.@__btnPhone.Name = "__btnPhone";
            this.@__btnPhone.Size = new System.Drawing.Size(119, 23);
            this.@__btnPhone.TabIndex = 15;
            this.@__btnPhone.Text = "Phone";
            this.@__btnPhone.UseVisualStyleBackColor = true;
            this.@__btnPhone.Click += new System.EventHandler(this._btnPhone_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._lblIpAddress);
            this.panel1.Controls.Add(this._lblPID);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(125, 37);
            this.panel1.TabIndex = 5;
            // 
            // _lblIpAddress
            // 
            this._lblIpAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this._lblIpAddress.AutoSize = true;
            this._lblIpAddress.Location = new System.Drawing.Point(4, 20);
            this._lblIpAddress.Name = "_lblIpAddress";
            this._lblIpAddress.Size = new System.Drawing.Size(17, 13);
            this._lblIpAddress.TabIndex = 0;
            this._lblIpAddress.Text = "IP";
            // 
            // _lblPID
            // 
            this._lblPID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this._lblPID.AutoSize = true;
            this._lblPID.Location = new System.Drawing.Point(4, 6);
            this._lblPID.Name = "_lblPID";
            this._lblPID.Size = new System.Drawing.Size(25, 13);
            this._lblPID.TabIndex = 1;
            this._lblPID.Text = "PID";
            // 
            // ClientMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 486);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ClientMainForm";
            this.Text = "Client";
            this._pnlNavigation.ResumeLayout(false);
            this._grbActions.ResumeLayout(false);
            this._grbNavigation.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Button _btnLog;
        protected System.Windows.Forms.Button _btnDiagnostics;
        protected System.Windows.Forms.Button _btnStartStop;
        protected System.Windows.Forms.Button _btnConfigure;
        protected System.Windows.Forms.Button _btnSend;
        protected System.Windows.Forms.Button _btnSendString;
        protected System.Windows.Forms.Button _btnTxDiagnostics;
        protected System.Windows.Forms.Button _btnRegister;
        protected System.Windows.Forms.Button _btnDialog;
        protected System.Windows.Forms.Button __btnPhone;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label _lblIpAddress;
        private System.Windows.Forms.Label _lblPID;
    }
}