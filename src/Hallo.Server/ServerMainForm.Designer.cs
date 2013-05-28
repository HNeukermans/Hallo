namespace Hallo.Server
{
    partial class ServerMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerMainForm));
            this._btnLog = new System.Windows.Forms.Button();
            this._btnDiagnostics = new System.Windows.Forms.Button();
            this._btnStartStop = new System.Windows.Forms.Button();
            this._btnConfigure = new System.Windows.Forms.Button();
            this._btnTxDiagnostics = new System.Windows.Forms.Button();
            this._btnRegistrar = new System.Windows.Forms.Button();
            this._pnlNavigation.SuspendLayout();
            this._grbActions.SuspendLayout();
            this._grbNavigation.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pnlNavigation
            // 
            this._pnlNavigation.Location = new System.Drawing.Point(0, 25);
            this._pnlNavigation.Size = new System.Drawing.Size(115, 461);
            // 
            // _grbActions
            // 
            this._grbActions.Controls.Add(this._btnConfigure);
            this._grbActions.Controls.Add(this._btnStartStop);
            // 
            // _grbNavigation
            // 
            this._grbNavigation.Controls.Add(this._btnRegistrar);
            this._grbNavigation.Controls.Add(this._btnTxDiagnostics);
            this._grbNavigation.Controls.Add(this._btnLog);
            this._grbNavigation.Controls.Add(this._btnDiagnostics);
            // 
            // _btnLog
            // 
            this._btnLog.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnLog.Location = new System.Drawing.Point(3, 42);
            this._btnLog.Name = "_btnLog";
            this._btnLog.Size = new System.Drawing.Size(103, 26);
            this._btnLog.TabIndex = 4;
            this._btnLog.Text = "Log";
            this._btnLog.UseVisualStyleBackColor = true;
            this._btnLog.Click += new System.EventHandler(this._btnLog_Click);
            // 
            // _btnDiagnostics
            // 
            this._btnDiagnostics.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnDiagnostics.Location = new System.Drawing.Point(3, 16);
            this._btnDiagnostics.Name = "_btnDiagnostics";
            this._btnDiagnostics.Size = new System.Drawing.Size(103, 26);
            this._btnDiagnostics.TabIndex = 6;
            this._btnDiagnostics.Text = "Diagnostics";
            this._btnDiagnostics.UseVisualStyleBackColor = true;
            this._btnDiagnostics.Click += new System.EventHandler(this._btnDiagnostics_Click);
            // 
            // _btnStartStop
            // 
            this._btnStartStop.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnStartStop.Location = new System.Drawing.Point(3, 16);
            this._btnStartStop.Name = "_btnStartStop";
            this._btnStartStop.Size = new System.Drawing.Size(103, 26);
            this._btnStartStop.TabIndex = 6;
            this._btnStartStop.Text = "Start";
            this._btnStartStop.UseVisualStyleBackColor = true;
            this._btnStartStop.Click += new System.EventHandler(this._btnStartStop_Click);
            // 
            // _btnConfigure
            // 
            this._btnConfigure.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnConfigure.Location = new System.Drawing.Point(3, 42);
            this._btnConfigure.Name = "_btnConfigure";
            this._btnConfigure.Size = new System.Drawing.Size(103, 26);
            this._btnConfigure.TabIndex = 7;
            this._btnConfigure.Text = "Configure";
            this._btnConfigure.UseVisualStyleBackColor = true;
            this._btnConfigure.Click += new System.EventHandler(this._btnConfigure_Click);
            // 
            // _btnTxDiagnostics
            // 
            this._btnTxDiagnostics.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnTxDiagnostics.Location = new System.Drawing.Point(3, 68);
            this._btnTxDiagnostics.Name = "_btnTxDiagnostics";
            this._btnTxDiagnostics.Size = new System.Drawing.Size(103, 26);
            this._btnTxDiagnostics.TabIndex = 13;
            this._btnTxDiagnostics.Text = "Tx Diagnostics";
            this._btnTxDiagnostics.UseVisualStyleBackColor = true;
            this._btnTxDiagnostics.Click += new System.EventHandler(this._btnTxDiagnostics_Click);
            // 
            // _btnRegistrar
            // 
            this._btnRegistrar.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnRegistrar.Location = new System.Drawing.Point(3, 94);
            this._btnRegistrar.Name = "_btnRegistrar";
            this._btnRegistrar.Size = new System.Drawing.Size(103, 26);
            this._btnRegistrar.TabIndex = 14;
            this._btnRegistrar.Text = "Registrar";
            this._btnRegistrar.UseVisualStyleBackColor = true;
            this._btnRegistrar.Click += new System.EventHandler(this._btnRegistrar_Click);
            // 
            // ServerMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 486);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ServerMainForm";
            this.Text = "Server";
            this._pnlNavigation.ResumeLayout(false);
            this._grbActions.ResumeLayout(false);
            this._grbNavigation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Button _btnStartStop;
        protected System.Windows.Forms.Button _btnLog;
        protected System.Windows.Forms.Button _btnDiagnostics;
        protected System.Windows.Forms.Button _btnConfigure;
        protected System.Windows.Forms.Button _btnTxDiagnostics;
        protected System.Windows.Forms.Button _btnRegistrar;
    }
}