using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FluentAssertions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Udp;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.Util;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Udp.MessageChannel
{
    public class UdpMessageBaseSpecification : Specification
    {
        protected UdpMessageChannelReadResult _result;
        protected UdpMessageChannel _channel;
        protected Mock<ISipValidator> _validator;
        protected UdpPacket _packet;
        protected Mock<IUdpServer> _server;

        protected override void Given()
        {
            _server = new Mock<IUdpServer>();
            _server.Setup(s => s.ListeningPoint).Returns(TestConstants.IpEndPoint1);
        }
    }

    [TestFixture]
    public class When_reading_an_unparseable_message : UdpMessageBaseSpecification
    {
        protected override void Given()
        {
            base.Given();
            var parser = new Mock<ISipMessageParser>();
            parser.Setup(p => p.ParseSipMessage(It.IsAny<byte[]>(), It.IsAny<string>())).Throws(new SipParseException(""));

            _validator = new Mock<ISipValidator>();
            _packet = new UdpPacketBuilder().WithDataBytes(new DataBytesBuilder().WithText("nonsens").Build()).Build();
            _channel = new UdpMessageChannel( new SipStack(), _validator.Object, parser.Object);
        }

        protected override void When()
        {
            _result = _channel.Read(_packet, _server.Object);
        }

        public ParseExceptionReadResult Result { get { return _result as ParseExceptionReadResult; } }

        [Test]
        public void Expect_the_result_is_an_ParseExceptionReadResult()
        {
            Result.Should().NotBeNull();
        }
        [Test]
        public void Expect_the_ParseException_to_be_not_null()
        {
            Result.ParseException.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_ValidateMessage_is_never_invoked()
        {
            _validator.Verify(s => s.ValidateMessage(It.IsAny<SipRequest>()), Times.Never());
        }

        //[Test]
        //public void Expect_the_ValidateRequest_is_never_invoked()
        //{
        //    _result.
        //    _validator.Verify(s => s.ValidateRequest(It.IsAny<SipRequest>()), Times.Never());
        //}
    }
}