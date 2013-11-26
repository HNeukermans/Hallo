using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using Hallo.Util;
using NLog;

namespace Hallo.Sip.Stack.Transactions
{
    public abstract class SipAbstractClientTransaction : ISipClientTransaction, ISipResponseProcessor
    {
        protected Logger _logger;

        protected readonly SipClientTransactionTable _table;
        protected readonly ISipMessageSender _messageSender;
        protected readonly ISipListener _listener;
        protected readonly object _rtxLock = new object();
        protected readonly object _lock = new object();
        protected bool _isDisposed;
        protected SipRequest _request;
        protected ITimerFactory _timerFactory;

        protected IObserver<SipTransactionStateInfo> _stateObserver;
        protected SipAbstractDialog _dialog;

        protected SipAbstractClientTransaction(
            SipClientTransactionTable table,
            SipRequest request, 
            ISipMessageSender messageSender,
            ISipListener listener,
            ITimerFactory timerFactory)
        {
            Check.Require(table, "table");
            Check.Require(messageSender, "messageSender");
            Check.Require(listener, "listener");
            Check.Require(request, "request");
            Check.Require(timerFactory, "timerFactory");
            
            _table = table;
            _messageSender = messageSender;
            _listener = listener;
            _request = request;
            _timerFactory = timerFactory;
        }

       
        /// <summary>
        /// the request that created the transaction
        /// </summary>
        public SipRequest Request
        {
            get { return _request; }
        }

        public abstract void Dispose();

        public string GetId()
        {
            return SipProvider.GetClientTransactionId(Request);
        }

        public abstract void ProcessResponse(SipResponseEvent responseEvent);
        
        public abstract void SendRequest();

        public IObservable<SipTransactionStateInfo> Observe()
        {
            return Observable.Create<SipTransactionStateInfo>((o) =>
            {
                _stateObserver = o;
                return Disposable.Empty;
            });
        }

        internal void SendRequestInternal()
        {
            lock (_rtxLock)
            {
                _messageSender.SendRequest(Request);
            }
        }

        protected SipTransactionStateInfo CreateStateInfo(SipTransactionStateName name)
        {
            var si = new SipTransactionStateInfo();
            si.CurrentState = name;
            si.Request = Request;
            si.TransactionType = Type;
            si.Id = GetId();
            return si;
        }

        public abstract SipTransactionType Type { get;  }
        
        public SipAbstractDialog GetDialog()
        {
            return _dialog;
        }

        public void SetDialog(SipAbstractDialog dialog)
        {
            _dialog = dialog;
        }
    }
}
