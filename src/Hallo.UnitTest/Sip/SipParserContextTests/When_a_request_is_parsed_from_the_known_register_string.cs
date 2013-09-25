using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class When_a_request_is_parsed_from_the_known_register_string : SipParserContextTestBase
    {
        private string _knownRegisterRequestString =
            "INVITE sip:joe@1.2.3.4 SIP/2.0\r\n" +
            "To: <sip:joe@4.3.2.1:8870>\r\n" +
            "Max-Forwards: 70\r\n" +
            "From: <sip:caller@4.4.2.1:8870>;tag=1234\r\n" +
            "Call-ID: 0ha0isnda977644900765@10.0.0.1\r\n" +
            "CSeq: 9 INVITE\r\n" +
            "Via: SIP/2.0/UDP 135.180.130.133;branch=z9hG4bKfae8cb69f547b8cb\r\n" +
            "Via: SIP/2.0/UDP 136.180.130.133;branch=z9hG4bKfa18cb69f547b8cb\r\n" +
            "Content-Type: application/sdp\r\n" +
            "Content-Length: 0\r\n" +
            "\r\n";

        protected override void When()
        {
            var bytes = SipFormatter.FormatToBytes(_knownRegisterRequestString);
            _sipRequest = _parser.Parse(new DatagramPacketBuilder().WithDataBytes(bytes).Build()) as SipRequest;
        }

        [Test]
        public void Expect_the_RequestLine_method_to_be_INVITE()
        {
            _sipRequest.RequestLine.Method.Should().Be(SipMethods.Invite);
        }

        [Test]
        public void Expect_the_Second_Via_header_Addrress_to_be_136_180_130_133()
        {
            _sipRequest.Vias.Count().Should().Be(2);
            _sipRequest.Vias[1].SentBy.Address.Should().Be(IPAddress.Parse("136.180.130.133"));
        }
    }
}
