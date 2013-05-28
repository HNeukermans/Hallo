using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FluentAssertions;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteClientTransactionTests
{
    public class When_in_completed_state : When_in_state_base
    {
        private AutoResetEvent _timeOutWaitHandle = new AutoResetEvent(false);
        private string _txId;

       public When_in_completed_state():base()
       {
           
       }


        protected override void GivenOverride()
        {
            Ctx.SendRequest();
            var r = CreateFinalResponseEvent();
            Ctx.ProcessResponse(r); /*force it to go into completed state*/
            Ctx.State.Should().Be(SipNonInviteClientTransaction.CompletedState); /*required assertion*/
        }

        protected override void When()
        {
            var r = CreateFinalResponseEvent();
            Ctx.ProcessResponse(r);
        }

        [Test]
        public void Expect_the_Retransmit_timer_to_be_disposed()
        {
            RetransitTimer.IsDisposed.Should().BeTrue();
        }

        [Test]
        public void Expect_the_TimeOut_timer_to_be_disposed()
        {
            TimeOutTimer.IsDisposed.Should().BeTrue();
        }

        [Test]
        public void Expect_the_CompletedEndTimer_to_be_started()
        {
            CompletedEndTimer.IsStarted.Should().BeTrue();
        }
    }
}
