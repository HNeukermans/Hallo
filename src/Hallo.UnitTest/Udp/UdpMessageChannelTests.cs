using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Udp;
using Hallo.UnitTest.Builders;
using Moq;
using NUnit.Framework;
using Hallo.UnitTest.Helpers;

namespace Hallo.UnitTest.Udp
{
    [TestFixture]
    public class UdpMessageChannelTests
    {
        private UdpMessageChannelReadResult _result;

        //[Test]
        //[ExpectedException(typeof(SipParseException))]
        //public void Start_RequestWithoutFrom_ExpectToThrowException()
        //{
        //    var parser = new Mock<ISipMessageParser>();
        //    parser.Setup(p => p.ParseSipMessage(It.IsAny<byte[]>(), It.IsAny<string>())).Throws(new SipParseException(""));

        //    var validator = new Mock<ISipValidator>();
        //    validator.Setup(v => v.ValidateRequest(It.IsAny<SipRequest>())).Returns(new ValidateRequestResult()
        //                                                                                {HasInvalidSCeqMethod = true});
        //    var packet = new UdpPacketBuilder().Build();
        //    var channel = new UdpMessageChannel(packet, null, new SipStack(), validator.Object);
        //    _result = channel.Start();
        //}

        //[Test]
        //[ExpectedException(typeof(SipParseException))]
        //public void EnsureRemoteEndPointInformationIsStoredInRequest_Request_With_SentBy_Different_From_RemoteEndPoint_Expect_To_Send_Response_With_The_Received_Parameter()
        //{
        //    var vias = new SipViaHeaderListBuilder().WithTopMost(new SipViaHeaderBuilder().WithSentBy(TestConstants.IpEndPoint1).Build()).Build();
        //    var sipRequest = new SipRequestBuilder().WithFrom(null).WithVias(vias).Build();
        //    var bytes = new DataBytesBuilder().WithText(sipRequest.FormatToString()).Build();
        //    var packet = new UdpPacketBuilder().WithDataBytes(bytes).WithRemoteEp(TestConstants.IpEndPoint2).Build();

        //    var channel = new UdpMessageChannel(packet, null, new SipStack());
        //    channel.EnsureRemoteEndPointInformationIsStoredInRequest(sipRequest);
        //}
    }
}
