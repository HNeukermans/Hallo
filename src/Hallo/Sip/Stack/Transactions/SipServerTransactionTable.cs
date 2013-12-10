using System;
using System.Collections.Concurrent;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Hallo.Sip.Stack.Transactions
{
    public class SipServerTransactionTable : ConcurrentDictionary<string, SipAbstractServerTransaction>
    {
        private IObserver<IObservable<Timestamped<SipTransactionStateInfo>>> _observer;

        public new bool TryAdd(string key, SipAbstractServerTransaction stx)
        {
            bool result = base.TryAdd(key, stx);

            if (result && _observer != null) _observer.OnNext(stx.Observe().Timestamp());

            return result;
        }
        
        public IObservable<IObservable<Timestamped<SipTransactionStateInfo>>> Observe()
        {
            return Observable.Create<IObservable<Timestamped<SipTransactionStateInfo>>>((o) =>
            {
                _observer = o;
                return Disposable.Empty;
            });
        }
        
    }
    
}