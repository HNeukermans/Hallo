using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Amib.Threading;
using FluentAssertions;
using Hallo.Sip;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.UdpSipListenerTests
{
    [TestFixture]
    public class UdpSipListenerTests
    {
        private ManualResetEvent _requestReceived;

        [Test]
        public void Test()
        {
            _requestReceived = new ManualResetEvent(false);

            /*setup + start listening*/
            var stub = new SipReceivedMessageProcessorStub(OnRequestReceived, (s,e) => { });

            var listeningPoint = new IPEndPoint(TestConstants.MyIpAddress, 33333);

            var f = new SipFactory();

            var stp = new SmartThreadPool();
            stp.Start();
            var provider = new SipContextSource(listeningPoint, stp);
            provider.AddListener(null);
            provider.Start();

            /*send a message to the listener*/
            var send = new SipRequestBuilder().Build();
            var requestBytes = SipFormatter.FormatMessage(send);

            var udpClient = new UdpClient();
            udpClient.Send(requestBytes, requestBytes.Length, listeningPoint);

            _requestReceived.WaitOne();

            var oc = ObjectComparer.Create();

            var received = stub.Requests.First();
            oc.Compare(received, send);
            Assert.True(oc.Differences.Count == 0, oc.DifferencesString);
        }

        private void OnRecievedResponse(SipResponse sipResponse, SipContext sipContext)
        {
            
        }

        private void OnRequestReceived(SipRequest request, SipContext context)
        {
           _requestReceived.Set();
        }
    }
}
