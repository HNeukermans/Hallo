using System.Reactive;
using Hallo.Sip.Stack.Transactions;

namespace Hallo.Component.Logic
{
    public class TxRowInfo : PropertyChangedBase
    {
        public TxRowInfo(string state)
        {
            State = state;
        }

        internal void Update(TxRowInfo newInfo)
        {
            State = newInfo.State;

            var duration = newInfo.Item.Timestamp - Item.Timestamp;

            var durationString = duration.TotalMilliseconds.ToString();

            switch (Item.Value.CurrentState)
            {
                case SipTransactionStateName.Trying: TryingTime = durationString;
                    break;
                case SipTransactionStateName.Proceeding: ProceedingTime = durationString;
                    break;
                case SipTransactionStateName.Completed: CompletedTime = durationString;
                    break;
            }

            Item = newInfo.Item;

        }

        #region properties

        public Timestamped<SipTransactionStateInfo> Item { get; set; }

        private string _dateTime;

        public string TimeOffset
        {
            get { return _dateTime; }
            set
            {
                _dateTime = value;
                RaisePropertyChanged("TimeOffset");
            }
        }

        private string _state;

        public string State
        {
            get { return _state; }
            set
            {
                _state = value;
                RaisePropertyChanged("State");
            }
        }

        private string _method;
        public string Method
        {
            get { return _method; }
            set
            {
                _method = value;
                RaisePropertyChanged("Method");
            }
        }

        private string _displayId;

        public string DisplayId
        {
            get { return _displayId; }
            set
            {
                _displayId = value;
                RaisePropertyChanged("DisplayId");
            }
        }

        private string _txType;

        public string TxType
        {
            get { return _txType; }
            set
            {
                _txType = value;
                RaisePropertyChanged("TxType");
            }
        }

        private string _ProceedingTime;

        public string ProceedingTime
        {
            get { return _ProceedingTime; }
            set
            {
                _ProceedingTime = value;
                RaisePropertyChanged("ProceedingTime");
            }
        }

        private string _CompletedTime;

        public string CompletedTime
        {
            get { return _CompletedTime; }
            set
            {
                _CompletedTime = value;
                RaisePropertyChanged("CompletedTime");
            }
        }

        private string _TryingTime;

        public string TryingTime
        {
            get { return _TryingTime; }
            set
            {
                _TryingTime = value;
                RaisePropertyChanged("TryingTime");
            }
        }

        #endregion

       

    }
}