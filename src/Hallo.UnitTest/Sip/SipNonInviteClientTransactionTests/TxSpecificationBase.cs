using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using Hallo.UnitTest.Builders;
using Moq;

namespace Hallo.UnitTest.Sip.SipNonInviteClientTransactionTests
{
    public class TxSpecificationBase : Specification
    {
        private SipNonInviteClientTransaction _ctx;
        private SipClientTransactionTable _txTable;
        private SipRequest _request;
        private ITimerFactory _timerFactory;
        private Mock<ISipMessageSender> _sender;

        internal SipNonInviteClientTransaction Ctx
        {
            get { return _ctx; }
            set { _ctx = value; }
        }

        protected internal Mock<ISipMessageSender> Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        protected internal SipClientTransactionTable TxTable
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
            TxTable = new SipClientTransactionTable();
            Request = new SipRequestBuilder().Build();
            TimerFactory = new TimerFactoryMockBuilder().Build();
        }

        protected override void Given()
        {
            _ctx = new SipNonInviteClientTransaction(TxTable, Sender.Object, Listener.Object, Request, TimerFactory);
            GivenOverride();
        }

        protected virtual void GivenOverride()
        {
            
        }

        protected SipResponseEvent CreateFinalResponseEvent()
        {
            var statusLine = new SipStatusLineBuilder().WithStatusCode(200).WithReason("OK").Build();
            var r = new SipResponseBuilder().WithStatusLine(statusLine).Build();
            var c = new SipContext();
            c.Response = r;
            return new SipResponseEvent(c);
        }

        //protected SipResponseEvent CreateFinalResponseEvent()
        //{
        //    var statusLine = new SipStatusLineBuilder().WithStatusCode(200).WithReason("OK").Build();
        //    var r = new SipResponseBuilder().WithStatusLine(statusLine).Build();
        //    var c = new SipContext();
        //    c.Response = r;
        //    return new SipResponseEvent(c);
        //}

        protected SipResponseEvent CreateProvisionalResponseEvent()
        {
            var statusLine = new SipStatusLineBuilder().WithStatusCode(180).WithReason("Ringing").Build();
            var r = new SipResponseBuilder().WithStatusLine(statusLine).Build();

            var c = new SipContext();
            c.Response = r;
            return new SipResponseEvent(c);
        }
    }
}
