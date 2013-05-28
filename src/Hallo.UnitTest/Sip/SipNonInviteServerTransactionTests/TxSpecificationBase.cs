using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Component;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Stubs;
using Moq;

namespace Hallo.UnitTest.Sip.SipNonInviteServerTransactionTests
{
    public class TxSpecificationBase : Specification
    {
        private SipNonInviteServerTransaction _ctx;
        private SipServerTransactionTable _txTable;
        private SipRequest _request;
        private ITimerFactory _timerFactory;
        private Mock<ISipMessageSender> _sender;
        
        protected internal SipNonInviteServerTransaction Stx
        {
            get { return _ctx; }
            set { _ctx = value; }
        }

        protected internal Mock<ISipMessageSender> Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        protected internal SipServerTransactionTable TxTable
        {
            get { return _txTable; }
            set { _txTable = value; }
        }

        public SipRequest Request
        {
            get { return _request; }
            set { _request = value; }
        }

        public ITimerFactory TimerFactory
        {
            get { return _timerFactory; }
            set { _timerFactory = value; }
        }

        public Mock<ISipListener> Listener { get; set; }

        public TxSpecificationBase()
        {
            Sender = new Mock<ISipMessageSender>();
            Listener = new Mock<ISipListener>();
            TxTable = new SipServerTransactionTable();
            Request = new SipRequestBuilder().Build();
            var tf = new TimerFactoryStub();
            tf.CreateNonInviteStxEndCompletedTimerInterceptor = CreateDefaultNonInviteStxEndCompletedTimer;
            TimerFactory = tf;
        }

        private ITimer CreateDefaultNonInviteStxEndCompletedTimer(Action action)
        {
            EndCompletedTimer = new TxTimerStub(action, int.MaxValue, false, () => { });
            return EndCompletedTimer;
        }

        protected TxTimerStub EndCompletedTimer { get; set; }
    

        protected override void Given()
        {
            _ctx = new SipNonInviteServerTransaction(TxTable, Request, Listener.Object, Sender.Object, TimerFactory);
            GivenOverride();
        }

        protected virtual void GivenOverride()
        {
            
        }

        protected SipResponse CreateFinalResponse(int statusCode = 200, string reason = "OK")
        {
            var statusLine = new SipStatusLineBuilder().WithStatusCode(statusCode).WithReason(reason).Build();
            return new SipResponseBuilder().WithStatusLine(statusLine).Build();
        }

        protected SipResponse CreateProvisionalResponse(int statusCode = 180, string reason = "Ringing")
        {
            var statusLine = new SipStatusLineBuilder().WithStatusCode(statusCode).WithReason(reason).Build();
            return new SipResponseBuilder().WithStatusLine(statusLine).Build();
        }
    }
}
