using System;
using System.Collections.Concurrent;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Hallo.Sip.Stack.Transactions
{
    public class SipClientTransactionTable : ConcurrentDictionary<string, SipAbstractClientTransaction>
    {
        private IObserver<IObservable<Timestamped<SipTransactionStateInfo>>> _observer;
        
        public new bool TryAdd(string key, SipAbstractClientTransaction ctx)
        {
            bool result = base.TryAdd(key, ctx);

            if(result && _observer != null) _observer.OnNext(ctx.Observe().Timestamp());

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