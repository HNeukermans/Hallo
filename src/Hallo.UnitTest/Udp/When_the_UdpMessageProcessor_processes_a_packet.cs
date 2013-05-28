using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Amib.Threading;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack;
using Hallo.Udp;
using Hallo.Util;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Udp
{

    [TestFixture]
    public class When_the_UdpMessageProcessor_processes_a_packet : Specification
    {
        private SmartThreadPool _threadPool;
        private Mock<IUdpMessageChannelStarter> _messageChannelStarterMock;
        private UdpMessageProcessor _messageProcessor;

        protected override void Given()
        {
            _threadPool = new SmartThreadPool();

            _messageChannelStarterMock = new Mock<IUdpMessageChannelStarter>();

            _messageProcessor = new UdpMessageProcessor(_threadPool, _messageChannelStarterMock.Object);
            _messageProcessor.Start();
        }

        protected override void When()
        {
            _messageProcessor.ProcessMessage(new UdpPacket(new byte[]{}));
        }

        [Test]
        public void Expect_the_messageChannnelStarter_StartChannel_method_to_be_invoked()
        {
            _messageChannelStarterMock.Verify((c) => c.StartChannel(It.IsAny<UdpPacket>(), _messageProcessor), Times.Once());
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _messageProcessor.Stop();
        }

    }

}