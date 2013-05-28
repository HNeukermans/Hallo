using System.Net;
using FluentAssertions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Udp;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Udp.MessageChannel
{
    [TestFixture]
    public class When_reading_a_response : UdpMessageBaseSpecification
    {
        private IPEndPoint _remoteEndPoint;

        protected override void Given()
        {
            base.Given();

            _remoteEndPoint = TestConstants.IpEndPoint1;
            var response = new SipResponseBuilder().Build();
            var bytes = new DataBytesBuilder().WithText(response.FormatEnvelopeToString()).Build();
            _packet = new UdpPacketBuilder().WithDataBytes(bytes).WithRemoteEp(_remoteEndPoint).Build();
            _channel = new UdpMessageChannel(new SipStack(), new SipValidator(), new SipMessageParser());
        }

        protected override void When()
        {
            _result = _channel.Read(_packet, _server.Object);
        }

        public SipMessageReadResult Result { get { return _result as SipMessageReadResult; } }

        [Test]
        public void Expect_the_result_is_of_type_SipMessageReadResult()
        {
            Result.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_Message_is_of_type_response()
        {
            Result.Message.Should().BeOfType<SipResponse>();
        }

        [Test]
        public void Expect_the_UdpPacket_Remote_EndPoint_to_be_RemoteEndPoint()
        {
            Result.UdpPacket.RemoteEndPoint.Should().Be(_remoteEndPoint);
        }
    }
}