namespace Hallo.Client.Forms
{
    partial class PhoneForm
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
            this._btnStartStop = new System.Windows.Forms.Button();
            this._grpMainPanel = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this._grpMainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnStartStop
            // 
            this._btnStartStop.Location = new System.Drawing.Point(12, 12);
            this._btnStartStop.Name = "_btnStartStop";
            this._btnStartStop.Size = new System.Drawing.Size(75, 23);
            this._btnStartStop.TabIndex = 0;
            this._btnStartStop.Text = "Start";
            this._btnStartStop.UseVisualStyleBackColor = true;
            this._btnStartStop.Click += new System.EventHandler(this._btnStart_Click);
            // 
            // _grpMainPanel
            // 
            this._grpMainPanel.Controls.Add(this.button1);
            this._grpMainPanel.Location = new System.Drawing.Point(12, 41);
            this._grpMainPanel.Name = "_grpMainPanel";
            this._grpMainPanel.Size = new System.Drawing.Size(460, 162);
            this._grpMainPanel.TabIndex = 1;
            this._grpMainPanel.TabStop = false;
            this._grpMainPanel.Text = "Main";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // PhoneForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 262);
            this.Controls.Add(this._grpMainPanel);
            this.Controls.Add(this._btnStartStop);
            this.Name = "PhoneForm";
            this.Text = "PhoneForm";
            this._grpMainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _btnStartStop;
        private System.Windows.Forms.GroupBox _grpMainPanel;
        private System.Windows.Forms.Button button1;
    }
}