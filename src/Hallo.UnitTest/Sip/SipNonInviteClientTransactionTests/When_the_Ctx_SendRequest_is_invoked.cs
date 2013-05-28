using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FluentAssertions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using Hallo.Util;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipNonInviteClientTransactionTests
{

    [TestFixture]
    public class When_the_Ctx_SendRequest_is_invoked : TxSpecificationBase
    {

        protected override void GivenOverride()
        {
        }

        protected override void When()
        {
            Ctx.SendRequest();
        }

        [Test]
        public void Expect_the_Ctx_to_have_the_trying_state()
        {
            Ctx.State.Should().Be(SipNonInviteClientTransaction.TryingState);
        }

        [Test]
        public void Expect_the_TxTable_to_contain_the_Id()
        {
            TxTable.ContainsKey(Ctx.GetId()).Should().BeTrue();
        }

    }

}