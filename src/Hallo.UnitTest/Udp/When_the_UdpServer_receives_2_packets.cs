using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amib.Threading;
using Hallo.Sip.Stack;
using NUnit.Framework;
using System.Net.Sockets;
using Hallo.Udp;
using System.Net;
using Moq;
using System.Linq.Expressions;
using Hallo.Sip;
using Hallo.Util;
using System.Threading;
using Ninject;

namespace Hallo.UnitTest.Udp
{
    [TestFixture]
    public class When_the_UdpServer_receives_2_packets : Specification
    {
        public static IPEndPoint DefaultRemoteEp = ParseUtil.ParseIpEndPoint("127.0.0.1:1111");

        readonly IPEndPoint _listeningPoint = new IPEndPoint(IPAddress.Loopback, 22200);
        UdpServer _udpServer = null;
        private FakeUdpPacket _packet1;
        private FakeUdpPacket _packet2;
        private SipStack _sipStack;
        private FakeSocketImpl _fakeSocketImpl;
        private Mock<IUdpMessageProcessor> _messageprocessor;

        protected override void Given()
        {
            _packet1 = new FakeUdpPacket("1", DefaultRemoteEp);
            _packet2 = new FakeUdpPacket("1", DefaultRemoteEp);

            var fakePackets = new Stack<FakeUdpPacket>();
            fakePackets.Push(_packet1);
            fakePackets.Push(_packet2);
            _fakeSocketImpl = new FakeSocketImpl(fakePackets);

            _messageprocessor = new Mock<IUdpMessageProcessor>();

            _sipStack = new SipStack();
            _udpServer = new UdpServer(_sipStack, _listeningPoint, _fakeSocketImpl, _messageprocessor.Object);
        }

        protected override void When()
        {
            _udpServer.Start();
        }

        [Test]
        public void Expect_the_PacketsReceived_to_be_equal_to_2()
        {
            _fakeSocketImpl.WaitForAllDataReceived();
            Assert.IsTrue(_udpServer.PacketsReceived == 2);
        }

        [Test]
        public void Expect_the_BytesReceived_to_be_equal_to_the_sum_of_the_fakePackets_received()
        {
            _fakeSocketImpl.WaitForAllDataReceived();
            var expectedNrOfBytes = _packet1.DataBytes.Length +
                                    _packet2.DataBytes.Length;
            
            Assert.IsTrue(_udpServer.BytesReceived == expectedNrOfBytes);
        }

        //[Test]
        //public void Expect_the_MessageProcessor_ProcessMessage_to_be_invoked_2_times()
        //{
        //    _fakeSocketImpl.WaitForAllDataReceived();
        //    _messageprocessor.Verify((m) => m.ProcessMessage(It.IsAny<UdpPacket>()), Times.Exactly(2));
        //}

        public override void CleanUp()
        {
            base.CleanUp();
            _udpServer.Stop();
        }
    }
}