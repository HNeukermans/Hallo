using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Stubs;
using Moq;

namespace Hallo.UnitTest.Sip.SipInviteClientTransactionTests
{
    public class TxSpecificationBase : Specification
    {
        protected internal SipInviteClientTransaction Ctx { get; set; }

        protected internal Mock<ISipMessageSender> MessageSenderMock { get; set; }

        protected internal SipClientTransactionTable TxTable { get; set; }

        public SipRequest Request { get; set; }

        public ITimerFactory TimerFactory { get; set; }

        public Mock<ISipListener> ListenerMock { get; set; }

        public SipHeaderFactory HeaderFactory { get; set; }

        public SipMessageFactory MessageFactory { get; set; }

        public TxSpecificationBase()
        {
            MessageSenderMock = new Mock<ISipMessageSender>();
            TxTable = new SipClientTransactionTable();
            Request = new SipRequestBuilder()
                .WithRequestLine
                (
                    new SipRequestLineBuilder()
                    .WithMethod(SipMethods.Invite)
                    .Build()
                ).Build();
            TimerFactory = new TimerFactoryMockBuilder().Build();
            ListenerMock = new Mock<ISipListener>();
            HeaderFactory = new SipHeaderFactory();
            MessageFactory = new SipMessageFactory();
        }
        
        protected override void Given()
        {
            Ctx = new SipInviteClientTransaction(TxTable, MessageSenderMock.Object, ListenerMock.Object, Request, TimerFactory, HeaderFactory, MessageFactory);
            GivenOverride();
        }

        protected virtual void GivenOverride()
        {
            
        }

        protected SipResponseEvent CreateFinalResponseEvent(int statusCode = 200, string reason = "OK")
        {
            var statusLine = new SipStatusLineBuilder().WithStatusCode(statusCode).WithReason(reason).Build();
            var r = new SipResponseBuilder().WithStatusLine(statusLine).Build();
            var c = new SipContext();
            c.Response = r;
            return new SipResponseEvent(c);
        }

        protected SipResponseEvent CreateProvisionalResponseEvent(int statusCode = 180, string reason = "Ringing")
        {
            var statusLine = new SipStatusLineBuilder().WithStatusCode(statusCode).WithReason(reason).Build();
            var r = new SipResponseBuilder().WithStatusLine(statusLine).Build();
            var c = new SipContext();
            c.Response = r;
            return new SipResponseEvent(c);
        }
    }
}
