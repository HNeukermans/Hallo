using Hallo.Sip.Stack;
using System;

namespace Hallo.Component
{
    public class TxTimerBuilder
    {
        Action _callback;

        public TxTimerBuilder()
        {
            _interval = int.MaxValue;
            _isPeriodic = false;
        }

        public TxTimerBuilder WithCallback(Action value)
        {
            _callback = value;
            return this;
        }

        int _interval;

        public TxTimerBuilder WithInterval(int value)
        {
            _interval = value;
            return this;
        }


        bool _isPeriodic;

        public TxTimerBuilder WithisPeriodic(bool value)
        {
            _isPeriodic = value;
            return this;
        }

        public TxTimer Build()
        {
            return new TxTimer(_callback, _interval, _isPeriodic);
        }
    }
    
}