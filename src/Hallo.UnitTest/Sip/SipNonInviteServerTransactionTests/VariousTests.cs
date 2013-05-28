using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using Hallo.Component;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Stubs;
using Moq;
using NUnit.Framework;


namespace Hallo.UnitTest.Sip.SipNonInviteServerTransactionTests
{
    [TestFixture]
    public class VariousTests
    {
        private object _lock = new object();
        private int counter = 0;

        private AutoResetEvent are = new AutoResetEvent(false);

        private int Number = 100;
        private SipNonInviteClientTransaction _nict;
        private SipNonInviteServerTransaction _nist;
        private SipServerTransactionTable table;
        private ITimerFactory tfs;

        [Test]
        public void ExpectStxToDisposeThemselfWhenTheirEndTimerHasFired()
        {
            var s = new Mock<ISipMessageSender>();
            var l = new Mock<ISipListener>();
            var txTable = new SipServerTransactionTable();
            var tfsBuilder = new TimerFactoryStubBuilder();
            tfsBuilder.WithNonInviteStxEndCompletedTimerInterceptor((a) => new TxTimerStub(a, 1000, false, AfterCallBack));
            tfs = tfsBuilder.Build();
            for (int i = 0; i < Number; i++)
            {
                var r = new SipRequestBuilder().WithCSeq(new SipCSeqHeaderBuilder().WithSequence(i).Build()).Build();
                var stx = new SipNonInviteServerTransaction(txTable, r, l.Object, s.Object, tfs);
                
                /*go to completed state */
                stx.SendResponse(new SipResponseBuilder()
                    .WithStatusLine(new SipStatusLineBuilder()
                    .WithStatusCode(200)
                    .WithReason("OK")
                    .Build())
                    .Build());
            }

            /*wait for Number of timers to fire*/
            are.WaitOne();

            Assert.IsTrue(!txTable.AsEnumerable().Any());
        }
        
        private void AfterCallBack()
        {
            lock(_lock)
            {
                counter++;
                if(counter == Number) are.Set();

            }
        }

        

        //[Test]
        //public void VerifyTryingCompletedRegisterRequestTransaction()
        //{
        //    /*client = trying -> completed */
        //    /*server = trying -> completed */

        //    var register =
        //        new SipRequestBuilder().WithRequestLine(
        //            new SipRequestLineBuilder().WithMethod(SipMethods.Register).Build()).Build();

        //    var table = new SipClientTransactionTable();
        //    var requestSender = new RequestSenderStub(SendRequestTryingProceedingCompleted);
        //    tfs = new TimerFactory();
            
        //    _nict =  new SipNonInviteClientTransaction(requestSender, table, tfs,  register);
           
        //    var registerResponse = new SipResponseBuilder()
        //           .WithStatusLine(new SipStatusLineBuilder()
        //           .WithStatusCode(200)
        //           .WithReason("OK")
        //           .Build())
        //           .Build();

        //    _nict.TimeInterval().Subscribe(OnNext);
           
        //    _nict.SendRequest();

        //    are.WaitOne();

        //    Thread.Sleep(500);

        //    _nist.SendResponse(registerResponse);

        //    while (table.AsEnumerable().Count() > 1)
        //    {};

        //    Assert.IsTrue(true);
        //}

        //[Test]
        //public void VerifyTryingProceedingCompletedRegisterRequestTransaction()
        //{
        //    /*client = trying -> proceeding -> completed */
        //    /*server = trying -> proceeding -> completed */

        //    var register =
        //        new SipRequestBuilder().WithRequestLine(
        //            new SipRequestLineBuilder().WithMethod(SipMethods.Register).Build()).Build();

        //    var table = new SipClientTransactionTable();
        //    var requestSender = new RequestSenderStub(SendRequest);
        //    tfs = new TimerFactory();

        //    _nict = new SipNonInviteClientTransaction(requestSender, table, new TimerFactory(), register);
            
        //    var tryingResponse = new SipResponseBuilder()
        //           .WithStatusLine(new SipStatusLineBuilder()
        //           .WithStatusCode(180)
        //           .WithReason("Trying")
        //           .Build())
        //           .Build();

        //    _nict.TimeInterval().Subscribe(OnNext);

        //    _nict.SendRequest();

        //    are.WaitOne();

        //    Thread.Sleep(500);

        //    _nist.SendResponse(tryingResponse);

        //    Thread.Sleep(1000);

        //    var okResponse = new SipResponseBuilder()
        //           .WithStatusLine(new SipStatusLineBuilder()
        //           .WithStatusCode(200)
        //           .WithReason("OK")
        //           .Build())
        //           .Build();

        //    _nist.SendResponse(okResponse);

        //    while (table.AsEnumerable().Any())
        //    { };
        //}

        //private static void OnNext(TimeInterval<SipTransactionStateInfo> tx)
        //{
        //    Console.WriteLine(string.Format("{0}, {1}, {2}, {3}", tx.Value.CurrentState,
        //                                           tx.Value.Request.RequestLine.FormatToString(), tx.Value.TransactionType, tx.Interval));
        //}


        //private void SendResponse(SipResponse sipResponse)
        //{
        //    _nict.HandleResponseContext(sipResponse);
        //}

        //private void SendRequestTryingProceedingCompleted(SipRequest sipRequest)
        //{
        //    if (_nist == null)
        //    {
        //        var responseSender = new ResponseSenderStub(SendResponse);

        //        _nist = new SipNonInviteServerTransaction(responseSender, table, tfs, sipRequest);
        //        //var co = Observer.Create<SipTransactionStateInfo>((a) => Console.WriteLine(string.Format("{0}, {1}", a.CurrentState, a.Request.RequestLine.FormatToString())));
        //        _nist.TimeInterval().Subscribe(OnNext);

        //        are.Set();
        //    }
        //    else
        //    {
        //        _nist.HandleResentRequest(sipRequest);
        //    }
        //}

        //private void SendRequest(SipRequest sipRequest)
        //{
        //    if (_nist == null)
        //    {
        //        var responseSender = new ResponseSenderStub(SendResponse);

        //        _nist = new SipNonInviteServerTransaction(responseSender, table, tfs, sipRequest);
        //        //var co = Observer.Create<SipTransactionStateInfo>((a) => Console.WriteLine(string.Format("{0}, {1}", a.CurrentState, a.Request.RequestLine.FormatToString())));
        //        _nist.TimeInterval().Subscribe(OnNext);
               
        //        are.Set();
        //    }
        //    else
        //    {
        //        _nist.HandleResentRequest(sipRequest);
        //    }
        //}
    }
}
