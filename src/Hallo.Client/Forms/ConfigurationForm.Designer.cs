namespace Hallo.Client.Forms
{
    partial class ConfigurationForm
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
            this._propGridConfiguration = new System.Windows.Forms.PropertyGrid();
            this._btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _propGridConfiguration
            // 
            this._propGridConfiguration.Dock = System.Windows.Forms.DockStyle.Top;
            this._propGridConfiguration.Location = new System.Drawing.Point(0, 0);
            this._propGridConfiguration.Name = "_propGridConfiguration";
            this._propGridConfiguration.Size = new System.Drawing.Size(284, 209);
            this._propGridConfiguration.TabIndex = 0;
            this._propGridConfiguration.ToolbarVisible = false;
            this._propGridConfiguration.Click += new System.EventHandler(this._propGridConfiguration_Click);
            // 
            // _btnSave
            // 
            this._btnSave.Location = new System.Drawing.Point(197, 227);
            this._btnSave.Name = "_btnSave";
            this._btnSave.Size = new System.Drawing.Size(75, 23);
            this._btnSave.TabIndex = 1;
            this._btnSave.Text = "Save";
            this._btnSave.UseVisualStyleBackColor = true;
            this._btnSave.Click += new System.EventHandler(this._btnSave_Click);
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this._btnSave);
            this.Controls.Add(this._propGridConfiguration);
            this.MaximumSize = new System.Drawing.Size(300, 300);
            this.Name = "ConfigurationForm";
            this.Text = "Configuration Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid _propGridConfiguration;
        private System.Windows.Forms.Button _btnSave;
    }
}