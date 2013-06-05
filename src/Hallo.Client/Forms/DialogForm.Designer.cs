namespace Hallo.Client.Forms
{
    partial class DialogForm
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
            this._chbUseTx = new System.Windows.Forms.CheckBox();
            this._txtFromAlias = new System.Windows.Forms.TextBox();
            this._txtToAlias = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._chkPeerToPeer = new System.Windows.Forms.CheckBox();
            this._chbStubTimerFactory = new System.Windows.Forms.CheckBox();
            this._txtState = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this._grpbForm = new System.Windows.Forms.GroupBox();
            this._grpbDialogForm = new System.Windows.Forms.GroupBox();
            this._txtRouteSet = new System.Windows.Forms.TextBox();
            this._txtRemoteTarget = new System.Windows.Forms.TextBox();
            this._txtRemoteUri = new System.Windows.Forms.TextBox();
            this._txtLocalUri = new System.Windows.Forms.TextBox();
            this._txtRemoteSeqNr = new System.Windows.Forms.TextBox();
            this._txtLocalSeqNr = new System.Windows.Forms.TextBox();
            this._txtRemoteTag = new System.Windows.Forms.TextBox();
            this._txtLocalTag = new System.Windows.Forms.TextBox();
            this._txtCallId = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._btnPhone = new System.Windows.Forms.Button();
            this._txtLog = new System.Windows.Forms.TextBox();
            this._grpbDialogForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "To:";
            // 
            // _txtToUri
            // 
            this._txtToUri.Location = new System.Drawing.Point(141, 76);
            this._txtToUri.Name = "_txtToUri";
            this._txtToUri.Size = new System.Drawing.Size(178, 20);
            this._txtToUri.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "From:";
            // 
            // _txtFromUri
            // 
            this._txtFromUri.Location = new System.Drawing.Point(141, 50);
            this._txtFromUri.Name = "_txtFromUri";
            this._txtFromUri.Size = new System.Drawing.Size(178, 20);
            this._txtFromUri.TabIndex = 2;
            // 
            // _chbUseTx
            // 
            this._chbUseTx.AutoSize = true;
            this._chbUseTx.Checked = true;
            this._chbUseTx.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chbUseTx.Enabled = false;
            this._chbUseTx.Location = new System.Drawing.Point(325, 52);
            this._chbUseTx.Name = "_chbUseTx";
            this._chbUseTx.Size = new System.Drawing.Size(125, 17);
            this._chbUseTx.TabIndex = 1;
            this._chbUseTx.Text = "Send Transactionally";
            this._chbUseTx.UseVisualStyleBackColor = true;
            this._chbUseTx.CheckedChanged += new System.EventHandler(this._chbUseTx_CheckedChanged);
            // 
            // _txtFromAlias
            // 
            this._txtFromAlias.Location = new System.Drawing.Point(80, 50);
            this._txtFromAlias.Name = "_txtFromAlias";
            this._txtFromAlias.Size = new System.Drawing.Size(53, 20);
            this._txtFromAlias.TabIndex = 6;
            // 
            // _txtToAlias
            // 
            this._txtToAlias.Location = new System.Drawing.Point(80, 76);
            this._txtToAlias.Name = "_txtToAlias";
            this._txtToAlias.Size = new System.Drawing.Size(53, 20);
            this._txtToAlias.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(131, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "_";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(131, 55);
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
            this._chkPeerToPeer.Location = new System.Drawing.Point(325, 78);
            this._chkPeerToPeer.Name = "_chkPeerToPeer";
            this._chkPeerToPeer.Size = new System.Drawing.Size(85, 17);
            this._chkPeerToPeer.TabIndex = 10;
            this._chkPeerToPeer.Text = "Peer to Peer";
            this._chkPeerToPeer.UseVisualStyleBackColor = true;
            this._chkPeerToPeer.CheckedChanged += new System.EventHandler(this._chkPeerToPeer_CheckedChanged);
            // 
            // _chbStubTimerFactory
            // 
            this._chbStubTimerFactory.AutoSize = true;
            this._chbStubTimerFactory.Checked = true;
            this._chbStubTimerFactory.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chbStubTimerFactory.Enabled = false;
            this._chbStubTimerFactory.Location = new System.Drawing.Point(325, 101);
            this._chbStubTimerFactory.Name = "_chbStubTimerFactory";
            this._chbStubTimerFactory.Size = new System.Drawing.Size(105, 17);
            this._chbStubTimerFactory.TabIndex = 11;
            this._chbStubTimerFactory.Text = "Stub timerfactory";
            this._chbStubTimerFactory.UseVisualStyleBackColor = true;
            this._chbStubTimerFactory.CheckedChanged += new System.EventHandler(this._chbStubTimerFactory_CheckedChanged);
            // 
            // _txtState
            // 
            this._txtState.Location = new System.Drawing.Point(66, 14);
            this._txtState.Name = "_txtState";
            this._txtState.Size = new System.Drawing.Size(253, 20);
            this._txtState.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "STATE:";
            // 
            // _grpbForm
            // 
            this._grpbForm.Location = new System.Drawing.Point(21, 35);
            this._grpbForm.Name = "_grpbForm";
            this._grpbForm.Size = new System.Drawing.Size(607, 94);
            this._grpbForm.TabIndex = 20;
            this._grpbForm.TabStop = false;
            this._grpbForm.Text = "Form";
            // 
            // _grpbDialogForm
            // 
            this._grpbDialogForm.Controls.Add(this._txtRouteSet);
            this._grpbDialogForm.Controls.Add(this._txtRemoteTarget);
            this._grpbDialogForm.Controls.Add(this._txtRemoteUri);
            this._grpbDialogForm.Controls.Add(this._txtLocalUri);
            this._grpbDialogForm.Controls.Add(this._txtRemoteSeqNr);
            this._grpbDialogForm.Controls.Add(this._txtLocalSeqNr);
            this._grpbDialogForm.Controls.Add(this._txtRemoteTag);
            this._grpbDialogForm.Controls.Add(this._txtLocalTag);
            this._grpbDialogForm.Controls.Add(this._txtCallId);
            this._grpbDialogForm.Controls.Add(this.label14);
            this._grpbDialogForm.Controls.Add(this.label13);
            this._grpbDialogForm.Controls.Add(this.label12);
            this._grpbDialogForm.Controls.Add(this.label11);
            this._grpbDialogForm.Controls.Add(this.label10);
            this._grpbDialogForm.Controls.Add(this.label9);
            this._grpbDialogForm.Controls.Add(this.label8);
            this._grpbDialogForm.Controls.Add(this.label7);
            this._grpbDialogForm.Controls.Add(this.label6);
            this._grpbDialogForm.Location = new System.Drawing.Point(384, 135);
            this._grpbDialogForm.Name = "_grpbDialogForm";
            this._grpbDialogForm.Size = new System.Drawing.Size(324, 302);
            this._grpbDialogForm.TabIndex = 21;
            this._grpbDialogForm.TabStop = false;
            this._grpbDialogForm.Text = "SipDialogForm";
            // 
            // _txtRouteSet
            // 
            this._txtRouteSet.AcceptsReturn = true;
            this._txtRouteSet.Location = new System.Drawing.Point(84, 234);
            this._txtRouteSet.Multiline = true;
            this._txtRouteSet.Name = "_txtRouteSet";
            this._txtRouteSet.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtRouteSet.Size = new System.Drawing.Size(223, 42);
            this._txtRouteSet.TabIndex = 30;
            // 
            // _txtRemoteTarget
            // 
            this._txtRemoteTarget.Location = new System.Drawing.Point(84, 208);
            this._txtRemoteTarget.Name = "_txtRemoteTarget";
            this._txtRemoteTarget.Size = new System.Drawing.Size(223, 20);
            this._txtRemoteTarget.TabIndex = 29;
            // 
            // _txtRemoteUri
            // 
            this._txtRemoteUri.Location = new System.Drawing.Point(84, 182);
            this._txtRemoteUri.Name = "_txtRemoteUri";
            this._txtRemoteUri.Size = new System.Drawing.Size(223, 20);
            this._txtRemoteUri.TabIndex = 28;
            // 
            // _txtLocalUri
            // 
            this._txtLocalUri.Location = new System.Drawing.Point(84, 156);
            this._txtLocalUri.Name = "_txtLocalUri";
            this._txtLocalUri.Size = new System.Drawing.Size(223, 20);
            this._txtLocalUri.TabIndex = 27;
            // 
            // _txtRemoteSeqNr
            // 
            this._txtRemoteSeqNr.Location = new System.Drawing.Point(84, 129);
            this._txtRemoteSeqNr.Name = "_txtRemoteSeqNr";
            this._txtRemoteSeqNr.Size = new System.Drawing.Size(223, 20);
            this._txtRemoteSeqNr.TabIndex = 26;
            // 
            // _txtLocalSeqNr
            // 
            this._txtLocalSeqNr.Location = new System.Drawing.Point(84, 102);
            this._txtLocalSeqNr.Name = "_txtLocalSeqNr";
            this._txtLocalSeqNr.Size = new System.Drawing.Size(223, 20);
            this._txtLocalSeqNr.TabIndex = 25;
            // 
            // _txtRemoteTag
            // 
            this._txtRemoteTag.Location = new System.Drawing.Point(84, 73);
            this._txtRemoteTag.Name = "_txtRemoteTag";
            this._txtRemoteTag.Size = new System.Drawing.Size(223, 20);
            this._txtRemoteTag.TabIndex = 24;
            // 
            // _txtLocalTag
            // 
            this._txtLocalTag.Location = new System.Drawing.Point(84, 47);
            this._txtLocalTag.Name = "_txtLocalTag";
            this._txtLocalTag.Size = new System.Drawing.Size(223, 20);
            this._txtLocalTag.TabIndex = 23;
            // 
            // _txtCallId
            // 
            this._txtCallId.Location = new System.Drawing.Point(84, 21);
            this._txtCallId.Name = "_txtCallId";
            this._txtCallId.Size = new System.Drawing.Size(223, 20);
            this._txtCallId.TabIndex = 22;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(20, 240);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 13);
            this.label14.TabIndex = 8;
            this.label14.Text = "Route Set:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(-3, 213);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(81, 13);
            this.label13.TabIndex = 7;
            this.label13.Text = "Remote Target:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(26, 159);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(52, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Local Uri:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 186);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "Remote Uri:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(2, 132);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Remote Seq#:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 105);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Local Seq#:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "RemoteTag:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Local Tag:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Call-Id:";
            // 
            // _btnPhone
            // 
            this._btnPhone.Location = new System.Drawing.Point(21, 140);
            this._btnPhone.Name = "_btnPhone";
            this._btnPhone.Size = new System.Drawing.Size(357, 40);
            this._btnPhone.TabIndex = 17;
            this._btnPhone.Text = "Call";
            this._btnPhone.UseVisualStyleBackColor = true;
            this._btnPhone.Click += new System.EventHandler(this._btnPhone_Click);
            // 
            // _txtLog
            // 
            this._txtLog.AcceptsReturn = true;
            this._txtLog.Location = new System.Drawing.Point(24, 186);
            this._txtLog.Multiline = true;
            this._txtLog.Name = "_txtLog";
            this._txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtLog.Size = new System.Drawing.Size(351, 251);
            this._txtLog.TabIndex = 31;
            // 
            // DialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 449);
            this.Controls.Add(this._txtLog);
            this.Controls.Add(this._btnPhone);
            this.Controls.Add(this._grpbDialogForm);
            this.Controls.Add(this._txtToUri);
            this.Controls.Add(this.label5);
            this.Controls.Add(this._txtState);
            this.Controls.Add(this._chbStubTimerFactory);
            this.Controls.Add(this._chkPeerToPeer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._txtToAlias);
            this.Controls.Add(this._txtFromAlias);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._txtFromUri);
            this.Controls.Add(this._chbUseTx);
            this.Controls.Add(this._grpbForm);
            this.Name = "DialogForm";
            this.Text = "Dialog";
            this._grpbDialogForm.ResumeLayout(false);
            this._grpbDialogForm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox _chbUseTx;
        private System.Windows.Forms.TextBox _txtFromUri;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _txtToUri;
        private System.Windows.Forms.TextBox _txtFromAlias;
        private System.Windows.Forms.TextBox _txtToAlias;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox _chkPeerToPeer;
        private System.Windows.Forms.CheckBox _chbStubTimerFactory;
        private System.Windows.Forms.TextBox _txtState;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox _grpbForm;
        private System.Windows.Forms.GroupBox _grpbDialogForm;
        private System.Windows.Forms.TextBox _txtRouteSet;
        private System.Windows.Forms.TextBox _txtRemoteTarget;
        private System.Windows.Forms.TextBox _txtRemoteUri;
        private System.Windows.Forms.TextBox _txtLocalUri;
        private System.Windows.Forms.TextBox _txtRemoteSeqNr;
        private System.Windows.Forms.TextBox _txtLocalSeqNr;
        private System.Windows.Forms.TextBox _txtRemoteTag;
        private System.Windows.Forms.TextBox _txtLocalTag;
        private System.Windows.Forms.TextBox _txtCallId;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button _btnPhone;
        private System.Windows.Forms.TextBox _txtLog;
    }
}