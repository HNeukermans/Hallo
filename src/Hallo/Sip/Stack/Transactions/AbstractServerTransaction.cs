using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using Hallo.Util;

namespace Hallo.Sip.Stack.Transactions
{
    public abstract class AbstractServerTransaction : ISipServerTransaction
    {
        protected IObserver<SipTransactionStateInfo> _stateObserver;
        protected readonly object _lock = new object();
        protected readonly SipServerTransactionTable _table;
        protected readonly ISipMessageSender _messageSender;
        protected readonly ITimerFactory _timerFactory;
        protected readonly ISipListener _listener;
        protected readonly SipRequest _request;
        protected bool _isDisposed;
        protected SipTransactionType _type;

        protected AbstractServerTransaction(SipServerTransactionTable table, SipRequest request, ISipListener listener, ISipMessageSender messageSender, ITimerFactory timerFactory)
        {
            Check.Require(table, "table");
            Check.Require(request, "request");
            Check.Require(listener, "listener");
            Check.Require(messageSender, "messageSender");
            Check.Require(timerFactory, "timerFactory");

            _table = table;
            _request = request;
            _listener = listener;
            _messageSender = messageSender;
            _timerFactory = timerFactory;
        }

        /// <summary>
        /// the request that created the transaction
        /// </summary>
        public SipRequest Request
        {
            get { return _request; }
        }

        public abstract SipTransactionType Type { get;  }

        public abstract void Dispose();
        
        public string GetId()
        {
            return SipProvider.GetServerTransactionId(Request);
        }
 
        public abstract void SendResponse(SipResponse response);

        public abstract void Start();

        protected SipTransactionStateInfo CreateStateInfo(SipTransactionStateName name)
        {
            var si = new SipTransactionStateInfo();
            si.CurrentState = name;
            si.Request = Request;
            si.TransactionType = Type;
            si.Id = GetId();
            return si;
        }

        internal SipResponse LatestResponse { get; set; }

        protected void SendResponseInternal(SipResponse response = null)
        {
            if (response != null)
            {
                if (LatestResponse != null)
                {
                    //update the latestresponse only when it is a provisional response
                    if (LatestResponse.StatusLine.StatusCode < 200)
                    LatestResponse = response;
                }
                else
                {
                    LatestResponse = response;
                }
                
            }
            _messageSender.SendResponse(LatestResponse);
        }

        public IObservable<SipTransactionStateInfo> Observe()
        {
            return Observable.Create<SipTransactionStateInfo>((o) =>
            {
                _stateObserver = o;
                return Disposable.Empty;
            });
        }

        public abstract void ProcessRequest(SipRequestEvent requestEvent);

    }
}