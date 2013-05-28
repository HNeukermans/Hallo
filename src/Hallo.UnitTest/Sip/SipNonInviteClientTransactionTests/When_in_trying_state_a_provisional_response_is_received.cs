using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FluentAssertions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using Hallo.UnitTest.Builders;
using Hallo.Util;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteClientTransactionTests
{
    [TestFixture]
    public class When_in_trying_state_a_provisional_response_is_received : TxSpecificationBase
    {
        protected override void GivenOverride()
        {
            Ctx.SendRequest();
        }

        protected override void When()
        {
            var r = CreateProvisionalResponseEvent();

            Ctx.ProcessResponse(r);
        }

        [Test]
        public void Expect_the_Ctx_to_transition_to_the_ProceedingState()
        {
             Ctx.State.Should().Be(SipNonInviteClientTransaction.ProceedingState);
        }
    }
}