using FluentAssertions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Udp;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Udp.MessageChannel
{
    [TestFixture]
    public class When_reading_a_request_with_an_invalid_cseq_method_message : UdpMessageBaseSpecification
    {
        protected override void Given()
        {
            base.Given();

            var request = new SipRequestBuilder()
               .WithRequestLine(new SipRequestLineBuilder().WithMethod(SipMethods.Register).Build())
               .WithCSeq(new SipCSeqHeaderBuilder().WithCommand(SipMethods.Invite).Build())
               .Build();
            _packet = new UdpPacketBuilder().WithDataBytes(new DataBytesBuilder().WithText(request.FormatEnvelopeToString()).Build()).Build();
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
        public void Expect_the_BadRequestResponse_not_to_be_null()
        {
            Result.BadRequestResponse.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_Message_not_to_be_null()
        {
            Result.Message.Should().NotBeNull();
        }
        
        [Test]
        public void Expect_the_ValidationErrorResponse_code_to_be_x400BadRequest()
        {
            Result.BadRequestResponse.GetResponseCode().Should().Be(SipResponseCodes.x400_Bad_Request_InvalidCseqMethod);
        }
    }
}