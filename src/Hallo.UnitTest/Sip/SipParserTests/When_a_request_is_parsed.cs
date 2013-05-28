using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FluentAssertions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Util;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class When_a_request_is_parsed : SipParserTestBase
    {
        protected override void Given()
        {
            base.Given();
            var sipMessage = new SipRequestBuilder().Build();
            _bytes = SipFormatter.FormatMessage(sipMessage);
        }

        [Test]
        public void Expect_the_listener_OnRequest_to_be_invoked()
        {
            _listenerStub.OnRequestExecuteReceived.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_listener_OnResponse_not_to_be_invoked()
        {
            _listenerStub.OnResponseExecuteReceived.Should().BeNull();
        }

        [Test]
        public void Expect_the_listener_OnHeader_to_be_invoked()
        {
            _listenerStub.OnHeaderExecuteReceived.Should().NotBeEmpty();
        }

        [Test]
        public void Expect_the_listener_OnComplete_to_be_invoked()
        {
            _listenerStub.OnCompleteExecuteReceived.Should().BeTrue();
        }
    }
}
