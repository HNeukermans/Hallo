namespace Hallo.Client.Forms
{
    partial class SendForm
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
            this._chbSendRegister = new System.Windows.Forms.CheckBox();
            this._nudSendRegisterRate = new System.Windows.Forms.NumericUpDown();
            this._btnRegister = new System.Windows.Forms.Button();
            this._chbSendStateFull = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this._nudSendRegisterRate)).BeginInit();
            this.SuspendLayout();
            // 
            // _chbSendRegister
            // 
            this._chbSendRegister.AutoSize = true;
            this._chbSendRegister.Location = new System.Drawing.Point(102, 15);
            this._chbSendRegister.Name = "_chbSendRegister";
            this._chbSendRegister.Size = new System.Drawing.Size(145, 17);
            this._chbSendRegister.TabIndex = 32;
            this._chbSendRegister.Text = "Send Register per minute";
            this._chbSendRegister.UseVisualStyleBackColor = true;
            this._chbSendRegister.CheckedChanged += new System.EventHandler(this._chbSendRegister_CheckedChanged);
            // 
            // _nudSendRegisterRate
            // 
            this._nudSendRegisterRate.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this._nudSendRegisterRate.Location = new System.Drawing.Point(247, 14);
            this._nudSendRegisterRate.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this._nudSendRegisterRate.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this._nudSendRegisterRate.Name = "_nudSendRegisterRate";
            this._nudSendRegisterRate.Size = new System.Drawing.Size(44, 20);
            this._nudSendRegisterRate.TabIndex = 31;
            this._nudSendRegisterRate.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this._nudSendRegisterRate.ValueChanged += new System.EventHandler(this._nudSendRegisterRate_ValueChanged);
            // 
            // _btnRegister
            // 
            this._btnRegister.Location = new System.Drawing.Point(12, 12);
            this._btnRegister.Name = "_btnRegister";
            this._btnRegister.Size = new System.Drawing.Size(75, 23);
            this._btnRegister.TabIndex = 30;
            this._btnRegister.Text = "Register";
            this._btnRegister.UseVisualStyleBackColor = true;
            this._btnRegister.Click += new System.EventHandler(this._btnRegister_Click);
            // 
            // _chbSendStateFull
            // 
            this._chbSendStateFull.AutoSize = true;
            this._chbSendStateFull.Location = new System.Drawing.Point(102, 38);
            this._chbSendStateFull.Name = "_chbSendStateFull";
            this._chbSendStateFull.Size = new System.Drawing.Size(125, 17);
            this._chbSendStateFull.TabIndex = 33;
            this._chbSendStateFull.Text = "Send Transactionally";
            this._chbSendStateFull.UseVisualStyleBackColor = true;
            // 
            // SendForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 69);
            this.Controls.Add(this._chbSendStateFull);
            this.Controls.Add(this._chbSendRegister);
            this.Controls.Add(this._nudSendRegisterRate);
            this.Controls.Add(this._btnRegister);
            this.Name = "SendForm";
            this.Text = "SendForm";
            ((System.ComponentModel.ISupportInitialize)(this._nudSendRegisterRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox _chbSendRegister;
        private System.Windows.Forms.NumericUpDown _nudSendRegisterRate;
        private System.Windows.Forms.Button _btnRegister;
        private System.Windows.Forms.CheckBox _chbSendStateFull;
    }
}