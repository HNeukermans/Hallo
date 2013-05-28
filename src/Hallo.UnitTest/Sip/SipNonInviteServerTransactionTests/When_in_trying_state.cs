using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FluentAssertions;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteServerTransactionTests
{
    public class When_in_trying_state : TxSpecificationBase
    {
        public When_in_trying_state()
            : base()
        {

        }

        protected override void GivenOverride()
        {
            Stx.State.Should().Be(SipNonInviteServerTransaction.TryingState); /*required assertion*/
        }

        [Test]
        public void Expect_the_LatestResponse_to_be_null()
        {
            Stx.LatestResponse.Should().BeNull();
        }

        [Test]
        public void Expect_the_EndCompletedTimer_not_to_be_started()
        {
            EndCompletedTimer.IsStarted.Should().BeFalse();
        }
    }
}
