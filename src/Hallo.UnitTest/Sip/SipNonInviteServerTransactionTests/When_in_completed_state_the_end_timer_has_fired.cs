using System;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteServerTransactionTests
{
    [TestFixture]
    public class When_in_completed_state_the_end_timer_has_fired : TxSpecificationBase
    {
        private AutoResetEvent _timeOutWaitHandle = new AutoResetEvent(false);
        private string _txId;
        public TxTimerStub TimeOutTimer { get; set; }

        public When_in_completed_state_the_end_timer_has_fired()
        {
            var tf = new TimerFactoryStub();
            tf.CreateNonInviteStxEndCompletedTimerInterceptor = (a) => new TxTimerStub(a, 2000, false, AfterCallBack);
            TimerFactory = tf;
        }

        private void AfterCallBack()
        {
            _timeOutWaitHandle.Set();
        }

        protected override void GivenOverride()
        {
            _txId = Stx.GetId();
            Stx.SendResponse(CreateFinalResponse()); /*go into state*/
        }

        protected override void When()
        {
            /*wait for the timeout to happen*/
            _timeOutWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
        }

        [Test]
        public void Expect_the_TransactionTable_to_be_empty()
        {
            TxTable.Keys.Contains(_txId).Should().BeFalse();
        }
    }
}