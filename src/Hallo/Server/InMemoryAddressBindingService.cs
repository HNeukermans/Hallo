using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace Hallo.Server
{
    public class InMemoryAddressBindingService : ISipAdressBindingService
    {
        private readonly Dictionary<Tuple<string, IPEndPoint>, SipAddressBinding> _table = new Dictionary<Tuple<string, IPEndPoint>, SipAddressBinding>();
        private readonly object _lock = new object();
        private IObserver<SipAddressBindingDiagnosticsInfo> _observer;
        
        public void AddOrUpdate(SipAddressBinding addressBinding)
        {
            var key = Tuple.Create(addressBinding.AddressOfRecord, addressBinding.Host);
            

            lock(_lock)
            {
                SipAddressBinding ab;
                if(_table.TryGetValue(key, out ab))
                {
                    if(RemoveExpired(key, ab))
                    {
                        _table.Add(key, addressBinding);
                        PushDiagnostics(AddressBindingServiceOperation.Add, addressBinding);
                    }
                    else
                    {
                        /*update*/
                        _table.Remove(key);
                        _table.Add(key, addressBinding);
                        PushDiagnostics(AddressBindingServiceOperation.Update, addressBinding);
                  
                    }
                }
                else
                {
                    _table.Add(key, addressBinding);
                    PushDiagnostics(AddressBindingServiceOperation.Add, addressBinding);
                }
            }
        }

        public void Remove(SipAddressBinding addressBinding)
        {
            var key = Tuple.Create(addressBinding.AddressOfRecord, addressBinding.Host);
            SipAddressBinding ab;

            lock (_lock)
            {
                if (_table.TryGetValue(key, out ab))
                {
                    if (!RemoveExpired(key, ab))
                    {
                        _table.Remove(key);
                        PushDiagnostics(AddressBindingServiceOperation.Remove, ab);
                    }
                }
            }
        }

        public List<SipAddressBinding> GetByAddressOfRecord(string addressOfRecord)
        {
            List<Tuple<string,IPEndPoint>> keys = null;
            var abs = new List<SipAddressBinding>();

            keys = _table.Keys.Where(k => k.Item1 == addressOfRecord).ToList();
            lock (_lock)
            {
                foreach (var key in keys)
                {
                    SipAddressBinding ab;
                    if (_table.TryGetValue(key, out ab))
                    {
                        if(!RemoveExpired(key, ab))
                        {
                            abs.Add(ab);
                        }
                    }
                }
            }
            
            return abs;
        }

        private bool RemoveExpired(Tuple<string, IPEndPoint> key, SipAddressBinding ab)
        {
             if (ab.EndTime < DateTime.Now)
             {
                 _table.Remove(key);
                 PushDiagnostics(AddressBindingServiceOperation.CleanUp, ab);
                 return true;
             }
             return false;
        }

        private void PushDiagnostics(AddressBindingServiceOperation operation, SipAddressBinding addressBinding)
        {
            if (_observer != null)
            {
                _observer.OnNext(
                    new SipAddressBindingDiagnosticsInfo()
                    {
                        Operation = operation,
                        AddressBinding = addressBinding
                    });
            }
        }

        public IObservable<SipAddressBindingDiagnosticsInfo> ObserveDiagnostics()
        {
            return Observable.Create<SipAddressBindingDiagnosticsInfo>((o) =>
            {
                _observer = o;
                return Disposable.Empty;
            });
        }

    }
}