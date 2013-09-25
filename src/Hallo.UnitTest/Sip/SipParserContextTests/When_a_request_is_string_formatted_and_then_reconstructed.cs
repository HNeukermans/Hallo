using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FluentAssertions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.UnitTest.Builders;
using Hallo.Util;
using NUnit.Framework;
using Hallo.UnitTest.Helpers;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class When_a_request_is_formatted_and_then_parsed : SipParserContextTestBase
    {
        private string messageContent;
        private SipRequest _sipMessage;
        private SipRequest _originalRequest;
        
        protected override void When()
        {
            _originalRequest = new SipRequestBuilder().Build();
            var bytes = SipFormatter.FormatMessage(_originalRequest);
            _sipMessage = _parser.Parse(new DatagramPacketBuilder().WithDataBytes(bytes).Build()) as SipRequest;
        }

        [Test]
        public void Expect_the_reconstructed_requestline_to_be_equal_to_the_original()
        {
            var c = ObjectComparer.Create();
            c.Compare(_originalRequest.RequestLine, _sipMessage.RequestLine).Should().BeTrue();
            
        }

        [Test]
        public void Expect_the_reconstructed_viaheaders_to_be_equal_to_the_original()
        {
            var c = ObjectComparer.Create();
            c.Compare(_originalRequest.Vias, _sipMessage.Vias).Should().BeTrue();
        }

        [Test]
        public void Expect_the_reconstructed_fromheader_to_be_equal_to_the_original()
        {
            var c = ObjectComparer.Create();
            c.Compare(_originalRequest.From, _sipMessage.From).Should().BeTrue();
        }

        [Test]
        public void Expect_the_reconstructed_toheader_to_be_equal_to_the_original()
        {
            var c = ObjectComparer.Create();
            c.Compare(_originalRequest.To, _sipMessage.To).Should().BeTrue();
        }
    }

}