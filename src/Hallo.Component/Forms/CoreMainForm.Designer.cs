namespace Hallo.Component.Forms
{
    partial class CoreMainForm
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
            this._pnlNavigation = new System.Windows.Forms.Panel();
            this._grbActions = new System.Windows.Forms.GroupBox();
            this._grbNavigation = new System.Windows.Forms.GroupBox();
            this._toolStripOpenedForms = new System.Windows.Forms.ToolStrip();
            this._pnlNavigation.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pnlNavigation
            // 
            this._pnlNavigation.Controls.Add(this._grbActions);
            this._pnlNavigation.Controls.Add(this._grbNavigation);
            this._pnlNavigation.Dock = System.Windows.Forms.DockStyle.Left;
            this._pnlNavigation.Location = new System.Drawing.Point(0, 0);
            this._pnlNavigation.Name = "_pnlNavigation";
            this._pnlNavigation.Size = new System.Drawing.Size(115, 686);
            this._pnlNavigation.TabIndex = 1;
            // 
            // _grbActions
            // 
            this._grbActions.Location = new System.Drawing.Point(3, 3);
            this._grbActions.Name = "_grbActions";
            this._grbActions.Size = new System.Drawing.Size(109, 83);
            this._grbActions.TabIndex = 4;
            this._grbActions.TabStop = false;
            this._grbActions.Text = "Actions";
            // 
            // _grbNavigation
            // 
            this._grbNavigation.Enabled = false;
            this._grbNavigation.Location = new System.Drawing.Point(3, 92);
            this._grbNavigation.Name = "_grbNavigation";
            this._grbNavigation.Size = new System.Drawing.Size(109, 177);
            this._grbNavigation.TabIndex = 3;
            this._grbNavigation.TabStop = false;
            this._grbNavigation.Text = "Navigation";
            // 
            // _toolStripOpenedForms
            // 
            this._toolStripOpenedForms.Location = new System.Drawing.Point(0, 0);
            this._toolStripOpenedForms.Name = "_toolStripOpenedForms";
            this._toolStripOpenedForms.Size = new System.Drawing.Size(464, 25);
            this._toolStripOpenedForms.TabIndex = 2;
            this._toolStripOpenedForms.Text = "toolStrip1";
            this._toolStripOpenedForms.Visible = false;
            // 
            // MainBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 686);
            this.Controls.Add(this._pnlNavigation);
            this.Controls.Add(this._toolStripOpenedForms);
            this.IsMdiContainer = true;
            this.Name = "MainBaseForm";
            this.Text = "MainForm";
            this._pnlNavigation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Panel _pnlNavigation;
        protected System.Windows.Forms.ToolStrip _toolStripOpenedForms;
        protected System.Windows.Forms.GroupBox _grbActions;
        protected System.Windows.Forms.GroupBox _grbNavigation;
    }
}