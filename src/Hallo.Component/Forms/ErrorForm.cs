﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hallo.Component.Logic;
using Hallo.Component.Logic.Reactive;
using System.Reactive.Linq;

namespace Hallo.Component.Forms
{
    public partial class ErrorForm : CoreChildForm
    {
        private IDisposable _subscription;
        private BindingList<ExceptionRowInfo> _bindingList;

        public ErrorForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(Form_Closing);
     
        }

        private void Form_Closing(object sender, CancelEventArgs e)
        {
        }

        private void AddException(Exception exception)
        {
            var eri = new ExceptionRowInfo(exception.Message, exception.StackTrace, exception);

            _bindingList.Insert(0, eri);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            if (_subscription != null) _subscription.Dispose();

            _subscription = EventAggregator.Instance.GetEvent<ExceptionEvent>().ObserveOn(this).Subscribe(s => AddException(s.Exception));

            _bindingList = new BindingList<ExceptionRowInfo>();
            _dgvErrors.AutoGenerateColumns = false;
            _dgvErrors.DataSource = _bindingList;
        }

        private void _dgvErrors_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(_dgvErrors.Columns[e.ColumnIndex].Name == "StackTraceColumn")
            {
                MessageBox.Show(((ExceptionRowInfo) _dgvErrors.Rows[e.RowIndex].DataBoundItem).StackTrace);
            }
        }

       
    }
}
