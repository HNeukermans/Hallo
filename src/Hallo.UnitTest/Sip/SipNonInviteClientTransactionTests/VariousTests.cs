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
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteClientTransactionTests
{
    [TestFixture]
    public class VariousTests
    {
        [Test]
        public void When_1000_transactions_are_in_trying_state()
        {
            var m = new Mock<ISipMessageSender>();
            var l = new Mock<ISipListener>();
            var txTable = new SipClientTransactionTable();
           
            SipNonInviteClientTransaction ctx;
            for(int i=0;i <1000;i++)
            {
                /*provide a separate sequence so that each transaction has a unique ID*/
                var r = new SipRequestBuilder().WithCSeq(new SipCSeqHeaderBuilder().WithSequence(i).Build()).Build();
                ctx = new SipNonInviteClientTransaction(txTable, m.Object, l.Object, r, new TimerFactory());
                ctx.SendRequest(); /*go to state*/
            }
            
            Assert.IsTrue(txTable.Keys.Count == 1000);

            /*extra race condition check. Wait T1 time, so while disposing the transactions, their Retransmit is firing.*/
            var dateTime = DateTime.Now.AddMilliseconds(SipConstants.T1);
            while (dateTime > DateTime.Now) { };

            txTable.Values.ToList().ForEach(a => a.Dispose());

            Assert.IsTrue(txTable.Keys.Count == 0);
        }

        [Test]
        public void Test_Retransmit()
        {
            var s = new Mock<ISipMessageSender>();
            var l = new Mock<ISipListener>();
            var txTable = new SipClientTransactionTable();
            var request = new SipRequestBuilder().Build();
            SipNonInviteClientTransaction ctx = new SipNonInviteClientTransaction(txTable, s.Object, l.Object, request, new TimerFactoryMockBuilder().Build());
            ctx.SendRequest();

            var dateTime = DateTime.Now.AddMilliseconds(SipConstants.T1);
            while (dateTime > DateTime.Now) {};

            s.Verify(ss => ss.SendRequest(request), Times.Exactly(2));

            ctx.Dispose();
        }
    }
}
