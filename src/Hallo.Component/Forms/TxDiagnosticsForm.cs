using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Hallo.Component.Logic;
using Hallo.Sip.Stack.Transactions;
using Hallo.Component.Logic;

namespace Hallo.Component.Forms
{
    public partial class TxDiagnosticsForm : CoreChildForm
    {
        private IDisposable _subscription;
        private ObservableCollection<Timestamped<SipTransactionStateInfo>> _table;

        private int _idCounter = 0; 
        public TxDiagnosticsForm()
        {
            InitializeComponent();
            this.Closing += OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            _subscription.Dispose();
        }

        private void TxDiagnosticsForm_Load(object sender, EventArgs e)
        {
            Trace.WriteLine("Load: "+ Thread.CurrentThread.ManagedThreadId);

            _subscription = MainForm.SipProvider.ObserveTxDiagnosticsInfo().ObserveOn(this).Subscribe(AddTxInfoRow);

            _bindingList = new BindingList<TxRowInfo>();
            _dgvTxInfos.AutoGenerateColumns = false;
            _dgvTxInfos.DataSource = _bindingList;
        }
        
        private void AddTxInfoRow(Timestamped<SipTransactionStateInfo> s)
        {
            Trace.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Trace.WriteLine(Thread.CurrentThread.IsThreadPoolThread);

            var rtInfo = new TxRowInfo(s.Value.CurrentState.ToString() + " " + s.Value.Request.RequestLine.Method);
            rtInfo.TimeOffset = s.Timestamp.ToString("hh:mm:ss");
            rtInfo.State = s.Value.CurrentState.ToString();
            rtInfo.DisplayId = _idCounter++.ToString();
            rtInfo.Method = s.Value.Request.RequestLine.Method;
            rtInfo.TxType = s.Value.TransactionType.ToString();
            rtInfo.Item = s;
            
            var found = _bindingList.FirstOrDefault(i => i.Item.Value != null && i.Item.Value.Id == rtInfo.Item.Value.Id);

            if (found != null)
            {
                found.Update(rtInfo);
            }
            else
            {
                Trace.WriteLine("Add" + _bindingList.Count);
                _bindingList.Insert(0, rtInfo);
            }
        }

        private void _btnClear_Click(object sender, EventArgs e)
        {
            _dgvTxInfos.Rows.Clear();
        }
    }
}
