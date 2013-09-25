using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.UnitTest.Sip.SipParserTests;
using Hallo.Util;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class When_a_request_with_2_via_headers_is_lexed : SipLexerTestBase
    {
        protected SipHeaderList<SipViaHeader> _vias;
        
        protected override void Given()
        {
            base.Given();

            _vias = new SipViaHeaderListBuilder()
                .Add(new SipViaHeaderBuilder().WithSentBy(TestConstants.IpEndPoint1).Build())
                .Add(new SipViaHeaderBuilder().WithSentBy(TestConstants.IpEndPoint2).Build()).Build();
            var sipMessage = new SipRequestBuilder()
                .WithNoHeaders()
                .WithVias(_vias).Build();

            _bytes = SipFormatter.FormatMessage(sipMessage);
        }

        [Test]
        public void Expect_the_listener_OnHeader_to_be_invoked_with_only_the_via_header_name()
        {
            OnHeaderExecuteReceived.Should().HaveCount(1);
        }

        [Test]
        public void Expect_the_listener_OnHeader_to_be_invoked_with_the_via_header_name()
        {
            OnHeaderExecuteReceived[SipHeaderNames.Via].Should().NotBeNull();
        }

        [Test]
        public void Expect_the_listener_OnHeader_to_be_invoked_2_times_with_via_name_arg()
        {
            OnHeaderExecuteReceived[SipHeaderNames.Via].Should().HaveCount(2);
        }
    }

}