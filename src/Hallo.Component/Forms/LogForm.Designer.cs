namespace Hallo.Component.Forms
{
    partial class LogForm
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
            this._txtLog = new System.Windows.Forms.TextBox();
            this._nmrRefreshRate = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this._btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._nmrRefreshRate)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _txtLog
            // 
            this._txtLog.AcceptsReturn = true;
            this._txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtLog.Location = new System.Drawing.Point(0, 30);
            this._txtLog.Multiline = true;
            this._txtLog.Name = "_txtLog";
            this._txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtLog.Size = new System.Drawing.Size(765, 391);
            this._txtLog.TabIndex = 0;
            // 
            // _nmrRefreshRate
            // 
            this._nmrRefreshRate.Location = new System.Drawing.Point(112, 7);
            this._nmrRefreshRate.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._nmrRefreshRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._nmrRefreshRate.Name = "_nmrRefreshRate";
            this._nmrRefreshRate.Size = new System.Drawing.Size(35, 20);
            this._nmrRefreshRate.TabIndex = 1;
            this._nmrRefreshRate.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Refresh rate (sec):";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._btnClear);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(765, 30);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // _btnClear
            // 
            this._btnClear.Location = new System.Drawing.Point(173, 4);
            this._btnClear.Name = "_btnClear";
            this._btnClear.Size = new System.Drawing.Size(75, 23);
            this._btnClear.TabIndex = 3;
            this._btnClear.Text = "Clear";
            this._btnClear.UseVisualStyleBackColor = true;
            this._btnClear.Click += new System.EventHandler(this._btnClear_Click);
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 421);
            this.Controls.Add(this._nmrRefreshRate);
            this.Controls.Add(this._txtLog);
            this.Controls.Add(this.panel1);
            this.Name = "LogForm";
            this.Text = "Log Form";
            ((System.ComponentModel.ISupportInitialize)(this._nmrRefreshRate)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _txtLog;
        private System.Windows.Forms.NumericUpDown _nmrRefreshRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _btnClear;

    }
}