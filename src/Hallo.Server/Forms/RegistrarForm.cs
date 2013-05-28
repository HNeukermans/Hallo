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
using Hallo.Sip.Stack.Transactions;
using Hallo.Server.Logic;

namespace Hallo.Server.Forms
{
    public partial class RegistrarForm : ServerChildForm
    {
        private IDisposable _subscription;
       private int _idCounter = 0;

        public RegistrarForm()
        {
            InitializeComponent();
            this.Closing += OnClosing;
            this.Load += new EventHandler(RegistrarForm_Load);
        }
        
        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            _subscription.Dispose();
        }

        private void RegistrarForm_Load(object sender, EventArgs e)
        {
            _subscription = Server.GetRegistrarDiagnosticsInfo().ObserveOn(this).Subscribe(OnNext);

            _bindingList = new BindingList<RegistrarRowInfo>();
            _dgvTxInfos.AutoGenerateColumns = false;
            _dgvTxInfos.DataSource = _bindingList;
        }

        private void OnNext(SipAddressBindingDiagnosticsInfo info)
        {
            
            var rowItem = new RegistrarRowInfo(
                info.AddressBinding.Host.ToString(),
                info.AddressBinding.StartTime.ToString("hh:mm:ss"),
                info.AddressBinding.AddressOfRecord,
                info.AddressBinding.Expires,
                info.AddressBinding.EndTime.ToString("hh:mm:ss"),
                info.AddressBinding);
            
            var found = _bindingList.FirstOrDefault(i => i.Item.AddressOfRecord == rowItem.AddressOfRecord && i.Item.Host.Equals(rowItem.Item.Host));

            if (found != null)
            {
                if (info.Operation == AddressBindingServiceOperation.Remove ||
                    info.Operation == AddressBindingServiceOperation.CleanUp)
                {
                    _bindingList.Remove(found);
                }
                else if (info.Operation == AddressBindingServiceOperation.Update)
                {
                    found.Update(rowItem);
                }
            }
            else if(info.Operation == AddressBindingServiceOperation.Add ||
                info.Operation == AddressBindingServiceOperation.Update)
            {
                 rowItem.DisplayId = _bindingList.Count.ToString();
                _bindingList.Insert(0, rowItem);
            }
        }

        private void _btnClear_Click(object sender, EventArgs e)
        {
            _dgvTxInfos.Rows.Clear();
        }
    }
}
