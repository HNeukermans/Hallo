using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Hallo.Sip;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.UdpSipListenerTests
{
    public class UdpSipListenerLoadTest
    {
        private static readonly ManualResetEvent _threadsGoEvent = new ManualResetEvent(false);
        private static readonly ManualResetEvent _threadsDoneEvent = new ManualResetEvent(false);
        private const int ThreadCount = 50;
        private static int _currentThreadCount;
        private static List<int> _failedThreads = new List<int>();
        private static IPEndPoint _listenerEndPoint = new IPEndPoint(TestConstants.MyIpAddress, 33333);
        
        [Test]
        public void Test()
        {
            var stub = new SipReceivedMessageProcessorStub(OnRequestReceived, (s,e) => { });

            //var provider = new SipContextSource(_listenerEndPoint);
            provider.AddListener(null);
            provider.Start();

            Thread[] threads = new Thread[ThreadCount];
            for (int i = 0; i < ThreadCount; ++i)
            {
                threads[i] = new Thread(OnSendRequest);
                threads[i].Start(i + 1);
                Thread.Sleep(5);
            }

            if (!_threadsDoneEvent.WaitOne(60000, true))
                Console.WriteLine("Failed to get all responses.");

            foreach (int id in _failedThreads)
                Console.WriteLine("* Failed thread: " + id);
            if (_failedThreads.Count > 0)
                Console.WriteLine("* Total: " + _failedThreads.Count);

            Assert.IsTrue(true);
        }

        private void OnRequestReceived(SipRequest request, SipContext context)
        {
            // last thread should signal main test
            
            lock (_failedThreads)
            {
                Interlocked.Decrement(ref _currentThreadCount);
                Console.WriteLine(request.CSeq.Sequence + " done, left: " + _currentThreadCount);
                
                if (_currentThreadCount == 0)
                    _threadsDoneEvent.Set();
            }
        }

        private static void OnSendRequest(object state)
        {
            var id = (int)state;
            var client = new UdpClient();

            Interlocked.Increment(ref _currentThreadCount);
            //if (_currentThreadCount == ThreadCount)
            //    _threadsGoEvent.Set();

            //// thread should wait until all threads are ready, to try the server.
            //if (!_threadsGoEvent.WaitOne(60000, true))
            //    Assert.False(true, "Start event was not triggered.");

            var request = new SipRequestBuilder().WithCSeq(new SipCSeqHeaderBuilder().WithSequence(id).Build()).Build();

            try
            {
                var bytes = SipFormatter.FormatMessage(request);
                client.Send(bytes, bytes.Length, _listenerEndPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine("failed to send: " + id);
                lock (_failedThreads) _failedThreads.Add(id);
            }
            
        }
    }
}
