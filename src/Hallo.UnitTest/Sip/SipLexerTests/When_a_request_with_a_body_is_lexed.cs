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
using Hallo.UnitTest.Sip.SipParserTests;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class When_a_request_with_a_body_is_lexed : SipLexerTestBase
    {
        protected byte[] _originalBody;

        protected override void Given()
        {
            base.Given();

            _originalBody = new DataBytesBuilder().WithText("body").Build();
            var sipMessage = new SipRequestBuilder()
                .WithBody(_originalBody)
                .WithContentLength(4).Build();
            _bytes = SipFormatter.FormatMessage(sipMessage);
        }
       
        [Test]
        public void Expect_the_OnBody_to_be_executed()
        {
            OnBodyBytesExecuteReceived.Should().BeTrue();
        }

        [Test]
        public void Expect_the_received_body_bytes_to_have_the_origina_body_lenght()
        {
           OnBodyBytesReceived.Count.Should().Be(_originalBody.Count());
        }

        [Test]
        public void Expect_the_received_body_bytes_to_be_equal_to_the_original_body()
        {
            OnBodyBytesReceived.Should().BeEquivalentTo(_originalBody);
        }
        
    }
}
