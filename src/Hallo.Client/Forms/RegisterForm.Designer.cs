namespace Hallo.Client.Forms
{
    partial class RegisterForm
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
            this._btnRegister = new System.Windows.Forms.Button();
            this._btnQueryBindings = new System.Windows.Forms.Button();
            this._btnRemoveBindings = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this._txtExpirationTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._txtContactAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._txtUser = new System.Windows.Forms.TextBox();
            this._txtQueryUser = new System.Windows.Forms.TextBox();
            this._grpRemoveBindings = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this._txtRemoveUser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this._grpRegister = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this._grpQueryBindings = new System.Windows.Forms.GroupBox();
            this._chbSendTx = new System.Windows.Forms.CheckBox();
            this._lblDomain = new System.Windows.Forms.Label();
            this._txtCallId = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this._chkRegister = new System.Windows.Forms.CheckBox();
            this._grpRemoveBindings.SuspendLayout();
            this._grpRegister.SuspendLayout();
            this._grpQueryBindings.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnRegister
            // 
            this._btnRegister.Location = new System.Drawing.Point(18, 45);
            this._btnRegister.Name = "_btnRegister";
            this._btnRegister.Size = new System.Drawing.Size(58, 23);
            this._btnRegister.TabIndex = 30;
            this._btnRegister.Text = "Register";
            this._btnRegister.UseVisualStyleBackColor = true;
            this._btnRegister.Click += new System.EventHandler(this._btnRegister_Click);
            // 
            // _btnQueryBindings
            // 
            this._btnQueryBindings.Location = new System.Drawing.Point(6, 19);
            this._btnQueryBindings.Name = "_btnQueryBindings";
            this._btnQueryBindings.Size = new System.Drawing.Size(129, 23);
            this._btnQueryBindings.TabIndex = 34;
            this._btnQueryBindings.Text = "Query Bindings";
            this._btnQueryBindings.UseVisualStyleBackColor = true;
            this._btnQueryBindings.Click += new System.EventHandler(this._btnQueryBindings_Click);
            // 
            // _btnRemoveBindings
            // 
            this._btnRemoveBindings.Location = new System.Drawing.Point(6, 19);
            this._btnRemoveBindings.Name = "_btnRemoveBindings";
            this._btnRemoveBindings.Size = new System.Drawing.Size(129, 23);
            this._btnRemoveBindings.TabIndex = 35;
            this._btnRemoveBindings.Text = "Remove Bindings";
            this._btnRemoveBindings.UseVisualStyleBackColor = true;
            this._btnRemoveBindings.Click += new System.EventHandler(this._btnRemoveBindings_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1138, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 36;
            this.label1.Text = "ExpirationTime";
            // 
            // _txtExpirationTime
            // 
            this._txtExpirationTime.Location = new System.Drawing.Point(551, 17);
            this._txtExpirationTime.Name = "_txtExpirationTime";
            this._txtExpirationTime.Size = new System.Drawing.Size(39, 20);
            this._txtExpirationTime.TabIndex = 37;
            this._txtExpirationTime.Text = "3600";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(265, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 38;
            this.label2.Text = "to host";
            // 
            // _txtContactAddress
            // 
            this._txtContactAddress.Location = new System.Drawing.Point(314, 17);
            this._txtContactAddress.Name = "_txtContactAddress";
            this._txtContactAddress.Size = new System.Drawing.Size(129, 20);
            this._txtContactAddress.TabIndex = 39;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(72, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 40;
            this.label3.Text = "Bind user";
            // 
            // _txtUser
            // 
            this._txtUser.Location = new System.Drawing.Point(129, 16);
            this._txtUser.Name = "_txtUser";
            this._txtUser.Size = new System.Drawing.Size(129, 20);
            this._txtUser.TabIndex = 41;
            // 
            // _txtQueryUser
            // 
            this._txtQueryUser.Location = new System.Drawing.Point(176, 21);
            this._txtQueryUser.Name = "_txtQueryUser";
            this._txtQueryUser.Size = new System.Drawing.Size(129, 20);
            this._txtQueryUser.TabIndex = 43;
            // 
            // _grpRemoveBindings
            // 
            this._grpRemoveBindings.Controls.Add(this._btnQueryBindings);
            this._grpRemoveBindings.Controls.Add(this.label4);
            this._grpRemoveBindings.Controls.Add(this._txtQueryUser);
            this._grpRemoveBindings.Location = new System.Drawing.Point(10, 81);
            this._grpRemoveBindings.Name = "_grpRemoveBindings";
            this._grpRemoveBindings.Size = new System.Drawing.Size(696, 50);
            this._grpRemoveBindings.TabIndex = 0;
            this._grpRemoveBindings.TabStop = false;
            this._grpRemoveBindings.Text = "Remove Bindings";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(141, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 42;
            this.label4.Text = "User";
            // 
            // _txtRemoveUser
            // 
            this._txtRemoveUser.Location = new System.Drawing.Point(176, 22);
            this._txtRemoveUser.Name = "_txtRemoveUser";
            this._txtRemoveUser.Size = new System.Drawing.Size(129, 20);
            this._txtRemoveUser.TabIndex = 45;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(141, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 44;
            this.label5.Text = "User";
            // 
            // _grpRegister
            // 
            this._grpRegister.Controls.Add(this.label6);
            this._grpRegister.Controls.Add(this._txtExpirationTime);
            this._grpRegister.Controls.Add(this.label1);
            this._grpRegister.Controls.Add(this._txtUser);
            this._grpRegister.Controls.Add(this.label2);
            this._grpRegister.Controls.Add(this.label3);
            this._grpRegister.Controls.Add(this._txtContactAddress);
            this._grpRegister.Location = new System.Drawing.Point(10, 31);
            this._grpRegister.Name = "_grpRegister";
            this._grpRegister.Size = new System.Drawing.Size(717, 45);
            this._grpRegister.TabIndex = 46;
            this._grpRegister.TabStop = false;
            this._grpRegister.Text = "Register";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(449, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 42;
            this.label6.Text = "with expiration time";
            // 
            // _grpQueryBindings
            // 
            this._grpQueryBindings.Controls.Add(this._txtRemoveUser);
            this._grpQueryBindings.Controls.Add(this._btnRemoveBindings);
            this._grpQueryBindings.Controls.Add(this.label5);
            this._grpQueryBindings.Location = new System.Drawing.Point(10, 137);
            this._grpQueryBindings.Name = "_grpQueryBindings";
            this._grpQueryBindings.Size = new System.Drawing.Size(696, 50);
            this._grpQueryBindings.TabIndex = 1;
            this._grpQueryBindings.TabStop = false;
            this._grpQueryBindings.Text = "Query Bindings";
            // 
            // _chbSendTx
            // 
            this._chbSendTx.AutoSize = true;
            this._chbSendTx.Location = new System.Drawing.Point(455, 8);
            this._chbSendTx.Name = "_chbSendTx";
            this._chbSendTx.Size = new System.Drawing.Size(125, 17);
            this._chbSendTx.TabIndex = 47;
            this._chbSendTx.Text = "Send Transactionally";
            this._chbSendTx.UseVisualStyleBackColor = true;
            // 
            // _lblDomain
            // 
            this._lblDomain.AutoSize = true;
            this._lblDomain.Location = new System.Drawing.Point(51, 9);
            this._lblDomain.Name = "_lblDomain";
            this._lblDomain.Size = new System.Drawing.Size(56, 13);
            this._lblDomain.TabIndex = 48;
            this._lblDomain.Text = "XXXXXXX";
            // 
            // _txtCallId
            // 
            this._txtCallId.Location = new System.Drawing.Point(226, 6);
            this._txtCallId.Name = "_txtCallId";
            this._txtCallId.Size = new System.Drawing.Size(220, 20);
            this._txtCallId.TabIndex = 44;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(129, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 43;
            this.label7.Text = "Device Id (Call Id)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 13);
            this.label8.TabIndex = 49;
            this.label8.Text = "Domain";
            // 
            // _chkRegister
            // 
            this._chkRegister.AutoSize = true;
            this._chkRegister.Location = new System.Drawing.Point(-62, 50);
            this._chkRegister.Name = "_chkRegister";
            this._chkRegister.Size = new System.Drawing.Size(15, 14);
            this._chkRegister.TabIndex = 50;
            this._chkRegister.UseVisualStyleBackColor = true;
            // 
            // RegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 221);
            this.Controls.Add(this._chkRegister);
            this.Controls.Add(this.label8);
            this.Controls.Add(this._txtCallId);
            this.Controls.Add(this._lblDomain);
            this.Controls.Add(this.label7);
            this.Controls.Add(this._chbSendTx);
            this.Controls.Add(this._btnRegister);
            this.Controls.Add(this._grpRegister);
            this.Controls.Add(this._grpRemoveBindings);
            this.Controls.Add(this._grpQueryBindings);
            this.Name = "RegisterForm";
            this.Text = "RegisterForm";
            this._grpRemoveBindings.ResumeLayout(false);
            this._grpRemoveBindings.PerformLayout();
            this._grpRegister.ResumeLayout(false);
            this._grpRegister.PerformLayout();
            this._grpQueryBindings.ResumeLayout(false);
            this._grpQueryBindings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _btnRegister;
        private System.Windows.Forms.Button _btnQueryBindings;
        private System.Windows.Forms.Button _btnRemoveBindings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _txtExpirationTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _txtContactAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _txtUser;
        private System.Windows.Forms.TextBox _txtQueryUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox _txtRemoveUser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox _grpRegister;
        private System.Windows.Forms.GroupBox _grpRemoveBindings;
        private System.Windows.Forms.GroupBox _grpQueryBindings;
        private System.Windows.Forms.CheckBox _chbSendTx;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label _lblDomain;
        private System.Windows.Forms.TextBox _txtCallId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox _chkRegister;
    }
}