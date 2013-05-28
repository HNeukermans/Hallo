using System.ComponentModel;
using Hallo.Component.Logic;

namespace Hallo.Component.Forms
{
    partial class TxDiagnosticsForm
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
            this.components = new System.ComponentModel.Container();
            this._dgvTxInfos = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Method = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProceedingTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CompletedTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this._btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._dgvTxInfos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _dgvTxInfos
            // 
            this._dgvTxInfos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dgvTxInfos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Method,
            this.State,
            this.Column2,
            this.Column4,
            this.ProceedingTime,
            this.CompletedTime,
            this.Column3});
            this._dgvTxInfos.Location = new System.Drawing.Point(0, 29);
            this._dgvTxInfos.Name = "_dgvTxInfos";
            this._dgvTxInfos.Size = new System.Drawing.Size(794, 280);
            this._dgvTxInfos.TabIndex = 1;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "DisplayId";
            this.Column1.HeaderText = "Id";
            this.Column1.Name = "Column1";
            this.Column1.Width = 30;
            // 
            // Method
            // 
            this.Method.DataPropertyName = "Method";
            this.Method.HeaderText = "Method";
            this.Method.Name = "Method";
            // 
            // State
            // 
            this.State.DataPropertyName = "State";
            this.State.HeaderText = "CurrentState";
            this.State.Name = "State";
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "TimeOffset";
            this.Column2.HeaderText = "TimeOffset";
            this.Column2.Name = "Column2";
            this.Column2.Width = 150;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "TryingTime";
            this.Column4.HeaderText = "TryingTime";
            this.Column4.Name = "Column4";
            // 
            // ProceedingTime
            // 
            this.ProceedingTime.DataPropertyName = "ProceedingTime";
            this.ProceedingTime.HeaderText = "ProceedingTime";
            this.ProceedingTime.Name = "ProceedingTime";
            // 
            // CompletedTime
            // 
            this.CompletedTime.DataPropertyName = "CompletedTime";
            this.CompletedTime.HeaderText = "CompletedTime";
            this.CompletedTime.Name = "CompletedTime";
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "TxType";
            this.Column3.HeaderText = "Type";
            this.Column3.Name = "Column3";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._btnClear);
            this.panel1.Controls.Add(this._dgvTxInfos);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(794, 280);
            this.panel1.TabIndex = 2;
            // 
            // _btnClear
            // 
            this._btnClear.Location = new System.Drawing.Point(3, 3);
            this._btnClear.Name = "_btnClear";
            this._btnClear.Size = new System.Drawing.Size(97, 23);
            this._btnClear.TabIndex = 2;
            this._btnClear.Text = "Clear";
            this._btnClear.UseVisualStyleBackColor = true;
            this._btnClear.Click += new System.EventHandler(this._btnClear_Click);
            // 
            // TxDiagnosticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 373);
            this.Controls.Add(this.panel1);
            this.Name = "TxDiagnosticsForm";
            this.Text = "TxDiagnosticsForm";
            this.Load += new System.EventHandler(this.TxDiagnosticsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this._dgvTxInfos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView _dgvTxInfos;
        private BindingList<TxRowInfo> _bindingList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Method;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProceedingTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn CompletedTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Button _btnClear;
    }
}