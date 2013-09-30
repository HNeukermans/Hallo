using System;
using System.Collections.Concurrent;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Hallo.Sip.Stack.Transactions;
using System.Reactive.Disposables;

namespace Hallo.Sip
{
    public class SipServerTransactionTable : ConcurrentDictionary<string, SipAbstractServerTransaction>
    {
        private IObserver<IObservable<Timestamped<SipTransactionStateInfo>>> _observer;
       
        public new SipAbstractServerTransaction GetOrAdd(string key, Func<string, SipAbstractServerTransaction> factory)
        {
            throw new InvalidOperationException("This method is not supported.");
        }

        public SipAbstractServerTransaction GetOrAdd(string key, Func<SipAbstractServerTransaction> factoryMethod)
        {
            var tx = base.GetOrAdd(key, s => BeforeAddTx(factoryMethod()));
            return tx;
           
        }

        /// <summary>
        /// Initializes the Tx before adding it to the table.
        /// </summary>
        private SipAbstractServerTransaction BeforeAddTx(SipAbstractServerTransaction tx)
        {
            if (_observer != null) _observer.OnNext(tx.Observe().Timestamp());
            tx.Start();
            return tx;
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