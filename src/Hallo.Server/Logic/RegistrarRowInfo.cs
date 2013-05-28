using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Threading;
using Hallo.Component.Logic;

namespace Hallo.Server.Logic
{
    public class RegistrarRowInfo : PropertyChangedBase
    {
        public SipAddressBinding Item { get; set; }
       
        #region props
        
        private string _host;

        public string Host
        {
            get { return _host; }
            set
            {
                _host = value;
                RaisePropertyChanged(() =>Host);
            }
        }

        private string _startTime;

        public string StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                RaisePropertyChanged(() => StartTime);
            }
        }

        private string _addressOfRecord;

        public string AddressOfRecord
        {
            get { return _addressOfRecord; }
            set
            {
                _addressOfRecord = value;
                RaisePropertyChanged(() => AddressOfRecord);
            }
        }

        private string _displayId;

        public string DisplayId
        {
            get { return _displayId; }
            set
            {
                _displayId = value;
                RaisePropertyChanged(() => DisplayId);
            }
        }

        private int _expires;

        public int Expires
        {
            get { return _expires; }
            set
            {
                _expires = value;
                RaisePropertyChanged(() => Expires);
            }
        }

        private string _endTime;

        public string EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                RaisePropertyChanged(() => EndTime);
            }
        } 

        #endregion

        public RegistrarRowInfo(string host, string startTime, string addressOfRecord, int expires, string endTime, SipAddressBinding item)
        {
            _host = host;
            _startTime = startTime;
            _addressOfRecord = addressOfRecord;
            _expires = expires;
            _endTime = endTime;
            Item = item;

            Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).ObserveOn(Dispatcher.CurrentDispatcher).Subscribe(DecrementExpires);
        }

        private void DecrementExpires(long l)
        {
            Expires = (int)(Item.EndTime - DateTime.Now).TotalSeconds;
        }

        public void Update(RegistrarRowInfo rtInfo)
        {
            Expires = rtInfo.Expires;
            StartTime = rtInfo.StartTime;
            EndTime = rtInfo.EndTime;
        }

        
    }
}
