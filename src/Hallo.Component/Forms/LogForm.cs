using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Hallo.Component.Logic.Reactive;
using Hallo.Sip;
using System.Reactive.Linq;

namespace Hallo.Component.Forms
{
    public partial class LogForm : CoreChildForm
    {
        private ConcurrentQueue<LogEvent> _eventQueue;
        private IDisposable _subScription;

        public LogForm()
        {
            InitializeComponent();
            _eventQueue = new ConcurrentQueue<LogEvent>();
            this.Load += new EventHandler(Form_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(LogForm_Closing);
        }

        void LogForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _subScription.Dispose();
        }

        void Form_Load(object sender, EventArgs e)
        {
            SubscribeToMessageObservable();

            Observable.FromEventPattern<EventHandler, EventArgs>(
                s => _nmrRefreshRate.ValueChanged += s,
                s => _nmrRefreshRate.ValueChanged -= s)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .ObserveOn(this)
                .Subscribe(s => SubscribeToMessageObservable());
        }

        private TimeSpan GetLogInterval()
        {
            return TimeSpan.FromSeconds((int)_nmrRefreshRate.Value);
        }
       
        private void WriteToLog(IList<SipMessage> messages)
        {
            var sb = new StringBuilder();

            foreach (var m in messages)
            {
                sb.AppendLine(SipFormatter.FormatMessageEnvelope(m));
            }

            _txtLog.Text += sb.ToString();
            _txtLog.SelectionStart = _txtLog.Text.Length;
            _txtLog.ScrollToCaret();
        }

        private void SubscribeToMessageObservable()
        {
            if (_subScription != null) _subScription.Dispose();

           _subScription = MainForm.SipProvider.ObserveMesssageDiagnosticsInfo()
               .Buffer(GetLogInterval())
               .Where((l) => l.Count > 0)
               .ObserveOn(this)
               .Subscribe(WriteToLog);

        }

        private void _btnClear_Click(object sender, EventArgs e)
        {
            _txtLog.Text = string.Empty;
        }

        private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }
    }
}
