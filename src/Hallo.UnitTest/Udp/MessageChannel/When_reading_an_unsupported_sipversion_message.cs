using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FluentAssertions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Udp;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.Util;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Udp.MessageChannel
{

    [TestFixture]
    public class When_reading_an_unsupported_sipversion_message : UdpMessageBaseSpecification
    {
        protected override void Given()
        {
            base.Given();
            var parser = new Mock<ISipMessageParser>();
            parser.Setup(p => p.ParseSipMessage(It.IsAny<byte[]>(), It.IsAny<string>()))
               .Returns(new SipRequestBuilder().Build());
            _validator = new Mock<ISipValidator>();
            _validator.Setup(v => v.ValidateMessage(It.IsAny<SipRequest>()))
                .Returns(new ValidateRequestResult() { HasUnSupportedSipVersion = true });
            _packet = new UdpPacketBuilder().Build();

            _channel = new UdpMessageChannel(new SipStack(), _validator.Object, parser.Object);
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
            Result.BadRequestResponse.GetResponseCode().Should().Be(SipResponseCodes.x400_Bad_Request_UnsupportedSipVersion);
        }
    }
}