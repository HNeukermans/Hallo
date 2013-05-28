namespace Hallo.Client.Forms
{
    partial class SendStringForm
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
            this._pnlLeft = new System.Windows.Forms.Panel();
            this._grpActions = new System.Windows.Forms.GroupBox();
            this._cmbHeaders = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this._chbActionExcludeViaHeaders = new System.Windows.Forms.CheckBox();
            this._grpTemplates = new System.Windows.Forms.GroupBox();
            this._lnkTemplateRegister = new System.Windows.Forms.LinkLabel();
            this._pnlMain = new System.Windows.Forms.Panel();
            this._txtMessage = new System.Windows.Forms.TextBox();
            this._btnSend = new System.Windows.Forms.Button();
            this._txtSendTo = new System.Windows.Forms.TextBox();
            this._pnlLeft.SuspendLayout();
            this._grpActions.SuspendLayout();
            this._grpTemplates.SuspendLayout();
            this._pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pnlLeft
            // 
            this._pnlLeft.Controls.Add(this._grpActions);
            this._pnlLeft.Controls.Add(this._grpTemplates);
            this._pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._pnlLeft.Location = new System.Drawing.Point(0, 0);
            this._pnlLeft.Name = "_pnlLeft";
            this._pnlLeft.Size = new System.Drawing.Size(115, 260);
            this._pnlLeft.TabIndex = 32;
            // 
            // _grpActions
            // 
            this._grpActions.Controls.Add(this._cmbHeaders);
            this._grpActions.Controls.Add(this.label1);
            this._grpActions.Controls.Add(this._chbActionExcludeViaHeaders);
            this._grpActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._grpActions.Location = new System.Drawing.Point(0, 85);
            this._grpActions.Name = "_grpActions";
            this._grpActions.Size = new System.Drawing.Size(115, 175);
            this._grpActions.TabIndex = 1;
            this._grpActions.TabStop = false;
            this._grpActions.Text = "Actions";
            // 
            // _cmbHeaders
            // 
            this._cmbHeaders.FormattingEnabled = true;
            this._cmbHeaders.Location = new System.Drawing.Point(6, 55);
            this._cmbHeaders.Name = "_cmbHeaders";
            this._cmbHeaders.Size = new System.Drawing.Size(103, 21);
            this._cmbHeaders.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Remove header";
            // 
            // _chbActionExcludeViaHeaders
            // 
            this._chbActionExcludeViaHeaders.AutoSize = true;
            this._chbActionExcludeViaHeaders.Location = new System.Drawing.Point(6, 19);
            this._chbActionExcludeViaHeaders.Name = "_chbActionExcludeViaHeaders";
            this._chbActionExcludeViaHeaders.Size = new System.Drawing.Size(82, 17);
            this._chbActionExcludeViaHeaders.TabIndex = 0;
            this._chbActionExcludeViaHeaders.Text = "Exclude Via";
            this._chbActionExcludeViaHeaders.UseVisualStyleBackColor = true;
            this._chbActionExcludeViaHeaders.CheckedChanged += new System.EventHandler(this._chbActionExcludeViaHeaders_CheckedChanged);
            // 
            // _grpTemplates
            // 
            this._grpTemplates.Controls.Add(this._lnkTemplateRegister);
            this._grpTemplates.Dock = System.Windows.Forms.DockStyle.Top;
            this._grpTemplates.Location = new System.Drawing.Point(0, 0);
            this._grpTemplates.Name = "_grpTemplates";
            this._grpTemplates.Size = new System.Drawing.Size(115, 69);
            this._grpTemplates.TabIndex = 0;
            this._grpTemplates.TabStop = false;
            this._grpTemplates.Text = "Templates";
            // 
            // _lnkTemplateRegister
            // 
            this._lnkTemplateRegister.AutoSize = true;
            this._lnkTemplateRegister.Location = new System.Drawing.Point(12, 25);
            this._lnkTemplateRegister.Name = "_lnkTemplateRegister";
            this._lnkTemplateRegister.Size = new System.Drawing.Size(46, 13);
            this._lnkTemplateRegister.TabIndex = 1;
            this._lnkTemplateRegister.TabStop = true;
            this._lnkTemplateRegister.Text = "Register";
            this._lnkTemplateRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._lnkTemplateRegister_LinkClicked);
            // 
            // _pnlMain
            // 
            this._pnlMain.Controls.Add(this._txtMessage);
            this._pnlMain.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlMain.Location = new System.Drawing.Point(115, 0);
            this._pnlMain.Name = "_pnlMain";
            this._pnlMain.Size = new System.Drawing.Size(481, 258);
            this._pnlMain.TabIndex = 33;
            // 
            // _txtMessage
            // 
            this._txtMessage.AcceptsReturn = true;
            this._txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtMessage.Location = new System.Drawing.Point(0, 0);
            this._txtMessage.Multiline = true;
            this._txtMessage.Name = "_txtMessage";
            this._txtMessage.Size = new System.Drawing.Size(481, 258);
            this._txtMessage.TabIndex = 0;
            // 
            // _btnSend
            // 
            this._btnSend.Location = new System.Drawing.Point(367, 310);
            this._btnSend.Name = "_btnSend";
            this._btnSend.Size = new System.Drawing.Size(75, 23);
            this._btnSend.TabIndex = 34;
            this._btnSend.Text = "Send";
            this._btnSend.UseVisualStyleBackColor = true;
            this._btnSend.Click += new System.EventHandler(this._btnSend_Click);
            // 
            // _txtSendTo
            // 
            this._txtSendTo.Location = new System.Drawing.Point(192, 312);
            this._txtSendTo.Name = "_txtSendTo";
            this._txtSendTo.Size = new System.Drawing.Size(169, 20);
            this._txtSendTo.TabIndex = 35;
            // 
            // SendStringForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 260);
            this.Controls.Add(this._txtSendTo);
            this.Controls.Add(this._btnSend);
            this.Controls.Add(this._pnlMain);
            this.Controls.Add(this._pnlLeft);
            this.Name = "SendStringForm";
            this.Text = "SendStringForm";
            this._pnlLeft.ResumeLayout(false);
            this._grpActions.ResumeLayout(false);
            this._grpActions.PerformLayout();
            this._grpTemplates.ResumeLayout(false);
            this._grpTemplates.PerformLayout();
            this._pnlMain.ResumeLayout(false);
            this._pnlMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel _pnlLeft;
        private System.Windows.Forms.GroupBox _grpActions;
        private System.Windows.Forms.CheckBox _chbActionExcludeViaHeaders;
        private System.Windows.Forms.GroupBox _grpTemplates;
        private System.Windows.Forms.Panel _pnlMain;
        private System.Windows.Forms.TextBox _txtMessage;
        private System.Windows.Forms.Button _btnSend;
        private System.Windows.Forms.TextBox _txtSendTo;
        private System.Windows.Forms.LinkLabel _lnkTemplateRegister;
        private System.Windows.Forms.ComboBox _cmbHeaders;
        private System.Windows.Forms.Label label1;

    }
}