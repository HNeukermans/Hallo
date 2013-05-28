using System;
using System.Threading;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.Sip.Util;

namespace Hallo.Sip.Stack
{
    /// <summary>
    /// Extends the Threading.Timer class. Allows the interval to be adapted more easily.
    /// Provides in a Start/Stop method. does not invokes the callback immedialetly on start but always after the first period
    /// has elapsed. 
    /// </summary>
    public class TxTimer : ITimer
    {
        private readonly object _lock = new object();

        private int _interval;
        public int Interval
        {
            get 
            { 
                lock (_lock) return _interval; 
            }
            set 
            { 
                lock(_lock)
                {
                    _interval = value;
                    ChangeInterval();
                }
            }
        }

        private void ChangeInterval()
        {
            _timer.Change(_interval, _interval);
        }

        public bool IsPeriodic
        {
            get { return _isPeriodic; }
        }

        public bool IsStopped
        {
            get { return !_isStarted; }
        }

        private readonly Action _callBack;
        private readonly bool _isPeriodic;
        private volatile bool _isStarted;
        private bool _isDisposed;

        public Timer _timer { get; set; }

        public TxTimer(Action callBack, int interval, bool isPeriodic)
        {
            _interval = interval;
            _isPeriodic = isPeriodic;
            _callBack = callBack;
            _timer = new Timer(new TimerCallback(OnCallBack), null, Timeout.Infinite, isPeriodic ? interval : Timeout.Infinite);
        }

        /// <summary>
        /// guarded callback.
        /// </summary>
        /// <param name="state"></param>
        private void OnCallBack(object state)
        {
            if(_isStarted) /*ensure still in started state*/
            {
                _callBack();
            }
        }

        public void Start()
        {
            if (_isStarted) return;
            lock(_lock)
            {
                _isStarted = true;
                /*the duetime equals one period.*/
                _timer.Change(_interval, IsPeriodic ? _interval : Timeout.Infinite);
            }
        }

        public void Stop()
        {
            if (!_isStarted) return;
            lock(_lock)
            {
                _isStarted = false;
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            lock(_lock)
            {
                SipUtil.DisposeTimer(_timer);
                _isDisposed = true;
            }
        }


        public bool IsDisposed
        {
            get { return _isDisposed; }
            
        }
    }
}