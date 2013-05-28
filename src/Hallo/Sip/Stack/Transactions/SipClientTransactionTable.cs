using System;
using System.Collections.Concurrent;
using System.Reactive;
using System.Reactive.Disposables;
using Hallo.Sip.Stack.Transactions;
using System.Reactive.Linq;

namespace Hallo.Sip
{
    public class SipClientTransactionTable : ConcurrentDictionary<string, AbstractClientTransaction>
    {
        private IObserver<IObservable<Timestamped<SipTransactionStateInfo>>> _observer;
        
        public new bool TryAdd(string key, AbstractClientTransaction ctx)
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