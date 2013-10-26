using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FluentAssertions;
using Hallo.Component;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public class When_in_proceeding_state : When_in_state_base
    {
        public When_in_proceeding_state()
            : base()
        {

        }

        protected override void When()
        {
            ((SipAbstractServerTransaction) Stx).Initialize();
            Stx.State.Should().Be(SipInviteServerTransaction.ProceedingState); /*required assertion*/
        }
        
        [Test]
        public void Expect_the_LatestResponse_to_be_null()
        {
            Stx.LatestResponse.Should().BeNull();
        }

        [Test]
        public void Expect_the_SendTrying_to_be_started()
        {
            Stx.SendTryingTimer.As<TxTimerStub>().IsStarted.Should().BeTrue();
        }

        [Test]
        public void Expect_the_EndCompletedTimer_not_to_be_started()
        {
            Stx.EndCompletedTimer.As<TxTimerStub>().IsStarted.Should().BeFalse();
        }

        [Test]
        public void Expect_the_EndConfirmedTimer_not_to_be_started()
        {
            Stx.EndConfirmedTimer.As<TxTimerStub>().IsStarted.Should().BeFalse();
        }

        [Test]
        public void Expect_the_RetransitTimer_not_to_be_started()
        {
            Stx.ReTransmitNonx200FinalResponseTimer.As<TxTimerStub>().IsStarted.Should().BeFalse();
        }

        
    }
}
