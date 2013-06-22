namespace Hallo.Component.Forms
{
    partial class ErrorForm
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
            this._dgvErrors = new System.Windows.Forms.DataGridView();
            this.TimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MessageColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StackTraceColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            ((System.ComponentModel.ISupportInitialize)(this._dgvErrors)).BeginInit();
            this.SuspendLayout();
            // 
            // _dgvErrors
            // 
            this._dgvErrors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dgvErrors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TimeColumn,
            this.MessageColumn,
            this.StackTraceColumn});
            this._dgvErrors.Dock = System.Windows.Forms.DockStyle.Top;
            this._dgvErrors.Location = new System.Drawing.Point(0, 0);
            this._dgvErrors.Name = "_dgvErrors";
            this._dgvErrors.Size = new System.Drawing.Size(682, 150);
            this._dgvErrors.TabIndex = 0;
            this._dgvErrors.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._dgvErrors_CellContentDoubleClick);
            this._dgvErrors.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this._dgvErrors_RowsAdded);
            // 
            // TimeColumn
            // 
            this.TimeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TimeColumn.HeaderText = "Time";
            this.TimeColumn.Name = "TimeColumn";
            this.TimeColumn.Width = 55;
            // 
            // MessageColumn
            // 
            this.MessageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MessageColumn.DataPropertyName = "Message";
            this.MessageColumn.HeaderText = "Message";
            this.MessageColumn.Name = "MessageColumn";
            // 
            // StackTraceColumn
            // 
            this.StackTraceColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.StackTraceColumn.HeaderText = "StackTrace";
            this.StackTraceColumn.Name = "StackTraceColumn";
            this.StackTraceColumn.Text = "Link";
            this.StackTraceColumn.Width = 69;
            // 
            // ErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 262);
            this.Controls.Add(this._dgvErrors);
            this.Name = "ErrorForm";
            this.Text = "ErrorForm";
            ((System.ComponentModel.ISupportInitialize)(this._dgvErrors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _dgvErrors;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MessageColumn;
        private System.Windows.Forms.DataGridViewLinkColumn StackTraceColumn;
    }
}