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
using NUnit.Framework;

namespace Hallo.UnitTest.Udp.MessageChannel
{

    [TestFixture]
    public class When_reading_a_request_with_sentby_equal_to_the_sender_endpoint : UdpMessageBaseSpecification
    {
        private IPEndPoint _senderEndPoint;

        protected override void Given()
        {
            base.Given();
            _senderEndPoint = TestConstants.IpEndPoint1;
            var via = new SipViaHeaderBuilder().WithSentBy(_senderEndPoint).Build();
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
        public void Expect_the_via_sentBy_is_equal_to_UdpPacket_RemoteEndPoint()
        {
            Result.Message.Vias[0].SentBy.Should().Be(_packet.RemoteEndPoint);
        }

        [Test]
        public void Expect_the_via_received_to_be_null()
        {
            Result.Message.Vias[0].Received.Should().BeNull();
        }
    }
}