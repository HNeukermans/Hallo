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
    public class When_reading_a_request_with_sentby_different_from_the_sender_endpoint : UdpMessageBaseSpecification
    {
        private IPEndPoint _senderEndPoint;
        private IPEndPoint _sentByEndPoint;

        protected override void Given()
        {
            base.Given();
            _senderEndPoint = TestConstants.IpEndPoint1;
            _sentByEndPoint = TestConstants.IpEndPoint2;
            var via = new SipViaHeaderBuilder().WithSentBy(_sentByEndPoint).Build();
            var request = new SipRequestBuilder().WithVias(new SipViaHeaderListBuilder().Add(via).Build()).Build();
            var bytes = new DataBytesBuilder().WithText(request.FormatEnvelopeToString()).Build();
            _packet = new UdpPacketBuilder().WithDataBytes(bytes).WithRemoteEp(_senderEndPoint).Build();
            _channel = new UdpMessageChannel(new SipStack(), new SipValidator(), new SipMessageParser());
        }

        protected override void When()
        {
            _result = _channel.Read(_packet, _server.Object);
        }

        public SipMessageReadResult Result { get { return _result as SipMessageReadResult; } }

        [Test]
        public void Expect_the_via_sentby_to_be_different_from_sender_endpoint()
        {
            Result.Message.Vias[0].SentBy.Should().NotBe(_packet.RemoteEndPoint);
        }

        [Test]
        public void Expect_the_via_received_to_be_equal_to_udppacket_remote_ipadress()
        {
            Result.Message.Vias[0].Received.Should().Be(_packet.RemoteEndPoint.Address);
        }
    }
}