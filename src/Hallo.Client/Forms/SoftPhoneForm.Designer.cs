namespace Hallo.Client.Forms
{
    partial class SoftPhoneForm
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
            this.label2 = new System.Windows.Forms.Label();
            this._txtToUri = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._txtFromUri = new System.Windows.Forms.TextBox();
            this._txtFromAlias = new System.Windows.Forms.TextBox();
            this._txtToAlias = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._chkPeerToPeer = new System.Windows.Forms.CheckBox();
            this._grpbForm = new System.Windows.Forms.GroupBox();
            this._btnPhone = new System.Windows.Forms.Button();
            this._txtLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "To:";
            // 
            // _txtToUri
            // 
            this._txtToUri.Location = new System.Drawing.Point(137, 53);
            this._txtToUri.Name = "_txtToUri";
            this._txtToUri.Size = new System.Drawing.Size(178, 20);
            this._txtToUri.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "From:";
            // 
            // _txtFromUri
            // 
            this._txtFromUri.Location = new System.Drawing.Point(137, 28);
            this._txtFromUri.Name = "_txtFromUri";
            this._txtFromUri.ReadOnly = true;
            this._txtFromUri.Size = new System.Drawing.Size(178, 20);
            this._txtFromUri.TabIndex = 2;
            // 
            // _txtFromAlias
            // 
            this._txtFromAlias.Location = new System.Drawing.Point(71, 27);
            this._txtFromAlias.Name = "_txtFromAlias";
            this._txtFromAlias.ReadOnly = true;
            this._txtFromAlias.Size = new System.Drawing.Size(53, 20);
            this._txtFromAlias.TabIndex = 6;
            // 
            // _txtToAlias
            // 
            this._txtToAlias.Location = new System.Drawing.Point(71, 53);
            this._txtToAlias.Name = "_txtToAlias";
            this._txtToAlias.Size = new System.Drawing.Size(53, 20);
            this._txtToAlias.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(124, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "_";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(124, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "_";
            // 
            // _chkPeerToPeer
            // 
            this._chkPeerToPeer.AutoSize = true;
            this._chkPeerToPeer.Checked = true;
            this._chkPeerToPeer.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkPeerToPeer.Location = new System.Drawing.Point(322, 55);
            this._chkPeerToPeer.Name = "_chkPeerToPeer";
            this._chkPeerToPeer.Size = new System.Drawing.Size(85, 17);
            this._chkPeerToPeer.TabIndex = 10;
            this._chkPeerToPeer.Text = "Peer to Peer";
            this._chkPeerToPeer.UseVisualStyleBackColor = true;
            this._chkPeerToPeer.CheckedChanged += new System.EventHandler(this._chkPeerToPeer_CheckedChanged);
            // 
            // _grpbForm
            // 
            this._grpbForm.Location = new System.Drawing.Point(12, 12);
            this._grpbForm.Name = "_grpbForm";
            this._grpbForm.Size = new System.Drawing.Size(421, 77);
            this._grpbForm.TabIndex = 20;
            this._grpbForm.TabStop = false;
            this._grpbForm.Text = "Form";
            // 
            // _btnPhone
            // 
            this._btnPhone.Location = new System.Drawing.Point(439, 18);
            this._btnPhone.Name = "_btnPhone";
            this._btnPhone.Size = new System.Drawing.Size(132, 71);
            this._btnPhone.TabIndex = 17;
            this._btnPhone.Text = "Call ";
            this._btnPhone.UseVisualStyleBackColor = true;
            this._btnPhone.Click += new System.EventHandler(this._btnPhone_Click);
            // 
            // _txtLog
            // 
            this._txtLog.AcceptsReturn = true;
            this._txtLog.Location = new System.Drawing.Point(12, 95);
            this._txtLog.Multiline = true;
            this._txtLog.Name = "_txtLog";
            this._txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtLog.Size = new System.Drawing.Size(559, 303);
            this._txtLog.TabIndex = 31;
            // 
            // SoftPhoneForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 405);
            this.Controls.Add(this._txtLog);
            this.Controls.Add(this._btnPhone);
            this.Controls.Add(this._txtToUri);
            this.Controls.Add(this._chkPeerToPeer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._txtToAlias);
            this.Controls.Add(this._txtFromAlias);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._txtFromUri);
            this.Controls.Add(this._grpbForm);
            this.Name = "SoftPhoneForm";
            this.Text = "SoftPhone";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _txtFromUri;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _txtToUri;
        private System.Windows.Forms.TextBox _txtFromAlias;
        private System.Windows.Forms.TextBox _txtToAlias;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox _chkPeerToPeer;
        private System.Windows.Forms.GroupBox _grpbForm;
        private System.Windows.Forms.Button _btnPhone;
        private System.Windows.Forms.TextBox _txtLog;
    }
}