using FluentAssertions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Udp;
using Hallo.UnitTest.Builders;
using NUnit.Framework;

namespace Hallo.UnitTest.Udp.MessageChannel
{
    [TestFixture]
    public class When_reading_a_response_that_has_a_required_header_missing : UdpMessageBaseSpecification
    {
        protected override void Given()
        {
            base.Given();
            var response = new SipResponseBuilder().WithCallId(null).Build();
            _packet = new UdpPacketBuilder().WithDataBytes(new DataBytesBuilder().WithText(response.FormatEnvelopeToString()).Build()).Build();
            _channel = new UdpMessageChannel(new SipStack(), new SipValidator(), new SipMessageParser());
        }

        protected override void When()
        {
            _result = _channel.Read(_packet, _server.Object);
        }

        public InvalidMessageReadResult Result { get { return _result as InvalidMessageReadResult; } }

        [Test]
        public void Expect_the_result_is_an_InvalidMessageReadResult()
        {
            Result.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_BadRequestResponse_to_be_null()
        {
            Result.BadRequestResponse.Should().BeNull();
        }

        [Test]
        public void Expect_the_Message_not_to_be_null()
        {
            Result.Message.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_Message_to_be_of_type_response()
        {
            (Result.Message as SipResponse).Should().NotBeNull();
        }
    }
    
}