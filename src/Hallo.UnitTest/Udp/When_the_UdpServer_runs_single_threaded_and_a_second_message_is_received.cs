using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amib.Threading;
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
    //[TestFixture]
    //public class When_the_UdpServer_runs_single_threaded_and_a_second_message_is_received : Specification
    //{
    //    public static IPEndPoint DefaultRemoteEp = ParseUtil.ParseIpEndPoint("127.0.0.1:1111");

    //    readonly IPEndPoint _listeningPoint = new IPEndPoint(IPAddress.Loopback, 22200);
    //    UdpServer _udpServer = null;
    //    private FakeUdpPacket _packet1;
    //    private FakeUdpPacket _packet2;
    //    private SmartThreadPool _threadPool;

    //    protected override void Given()
    //    {
    //        var mock = new Mock<IUdpMessageChannelFactory>();

    //        var heavyProcessingChannel = new FakeUdpMessageChannel(Timeout.Infinite);
    //        mock.Setup(m => m.CreateChannel(null, null, null)).Returns(heavyProcessingChannel);

    //        _packet1 = new FakeUdpPacket("1", DefaultRemoteEp);
    //        _packet2 = new FakeUdpPacket("1", DefaultRemoteEp);

    //        var fakePackets = new Stack<FakeUdpPacket>();
    //        fakePackets.Push(_packet1);
    //        fakePackets.Push(_packet2);
    //        var fakeSocketImpl = new FakeSocketImpl(fakePackets);

    //        var sipStack = new SipStack();
    //        sipStack.MaxWorkerThreads = 1;

    //        _threadPool = sipStack.CreateThreadPool();

    //        _udpServer = new UdpServer(sipStack, _listeningPoint, fakeSocketImpl, null);
    //    }

    //    protected override void When()
    //    {
    //        _udpServer.Start();
    //    }

    //    [Test]
    //    public void Expect_the_second_message_to_be_queued()
    //    {
    //        _threadPool.WaitForIdle();
    //        var q = _threadPool.PerformanceCountersReader.WorkItemsQueued;
    //        //Assert.IsTrue(_threadPool.PerformanceCountersReader.WorkItemsQueued == 1);
    //    }


    //    [Test]
    //    public void Expect_the_processed_messages_to_be_zero()
    //    {
    //        //Assert.IsTrue(_threadPool.PerformanceCountersReader.WorkItemsProcessed == 0);
    //    }

    //    [Test]
    //    public void Expect_the_ActiveThreads_to_be_one()
    //    {
    //        //Assert.IsTrue(_udpServer.ThreadPool.ActiveThreads == 1);
    //    }
    //    public override void CleanUp()
    //    {
    //        base.CleanUp();
    //        _udpServer.Stop();
    //    }
    //}
}