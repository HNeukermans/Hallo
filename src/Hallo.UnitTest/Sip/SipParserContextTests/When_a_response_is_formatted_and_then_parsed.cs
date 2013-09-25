using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class When_a_response_is_formatted_and_then_parsed : SipParserContextTestBase
    {
        private SipResponse _originalResponse;

        protected override void When()
        {
            _originalResponse = new SipRequestBuilder().Build().CreateResponse(SipResponseCodes.x200_Ok);
            var bytes = SipFormatter.FormatMessage(_originalResponse);
            _sipResponse = _parser.Parse(new DatagramPacketBuilder().WithDataBytes(bytes).Build()) as SipResponse;
        }
        
        [Test]
        public void Expect_the_reconstructed_statusline_to_be_equal_to_the_original()
        {
            var c = ObjectComparer.Create();
            c.Compare(_originalResponse.StatusLine, _sipResponse.StatusLine).Should().BeTrue();

        }

        [Test]
        public void Expect_the_reconstructed_vias_to_be_equal_to_the_original()
        {
            var c = ObjectComparer.Create();
            c.Compare(_originalResponse.Vias, _sipResponse.Vias).Should().BeTrue();
        }

        [Test]
        public void Expect_the_reconstructed_from_to_be_equal_to_the_original()
        {
            var c = ObjectComparer.Create();
            c.Compare(_originalResponse.From, _sipResponse.From).Should().BeTrue();
        }

        [Test]
        public void Expect_the_reconstructed_to_to_be_equal_to_the_original()
        {
            var c = ObjectComparer.Create();
            c.Compare(_originalResponse.To, _sipResponse.To).Should().BeTrue();
        }
    }
}
