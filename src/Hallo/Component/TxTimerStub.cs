using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip.Stack;

namespace Hallo.Component
{
    /// <summary>
    /// TxTimer test decorator. 
    /// </summary>
    public class TxTimerStub : ITimer
    {
        private readonly Action _realCallBack;
        private readonly Action _afterCallBack;
        private TxTimer _RealTimer { get; set; }
       
        public TxTimerStub(Action realCallBack, int interval, bool isPeriodic, Action afterCallBack)
        {
            _realCallBack = realCallBack;
            _afterCallBack = afterCallBack ?? delegate { };
            _RealTimer = new TxTimer(OnCallBack, interval, isPeriodic);
        }

        private void OnCallBack()
        {
            _realCallBack();
            _afterCallBack();
        }

        public void Start()
        {
            _RealTimer.Start();
            IsStarted = true;
        }

        public void Stop()
        {
            _RealTimer.Stop();
            IsStarted = false;
        }

        public int Interval
        {
            get { return _RealTimer.Interval; }
            set { _RealTimer.Interval = value; }
        }

        public bool IsDisposed { get; private set; }
        public bool IsStarted { get; private set; }

        public void Dispose()
        {
            Stop();
            _RealTimer.Dispose();
            IsDisposed = true;
        }
    }
}
