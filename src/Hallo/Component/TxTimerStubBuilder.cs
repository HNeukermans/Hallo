using System;

namespace Hallo.Component
{
    public class TxTimerStubBuilder 
    {
        Action _callback;

        /// <summary>
        /// creates by default a timer that is interval of int.MaxValue and
        /// is not periodic.
        /// </summary>
        public TxTimerStubBuilder()
        {
            _interval = int.MaxValue;
            _isPeriodic = false;
            _afterCallBack = () => { };
        }

        public TxTimerStubBuilder(Action afterCallBack = null)
            : this()
        {
            _afterCallBack = afterCallBack;
        }

        public TxTimerStubBuilder WithCallback(Action value)
        {
            _callback = value;
            return this;
        }

        int _interval;

        public TxTimerStubBuilder WithInterval(int value)
        {
            _interval = value;
            return this;
        }


        bool _isPeriodic;

        public TxTimerStubBuilder WithisPeriodic(bool value)
        {
            _isPeriodic = value;
            return this;
        }


        Action _afterCallBack;

        public TxTimerStubBuilder WithAfterCallBack(Action value)
        {
            _afterCallBack = value;
            return this;
        }

        public TxTimerStub Build()
        {
            return new TxTimerStub(_callback, _interval, _isPeriodic, _afterCallBack);
        }
    }

}