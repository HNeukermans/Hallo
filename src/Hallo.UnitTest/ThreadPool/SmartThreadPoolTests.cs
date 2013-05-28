using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Amib.Threading;
using NUnit.Framework;
using Hallo.UnitTest.ThreadPool;

namespace Hallo.UnitTest.Udp
{
    [TestFixture]
    public class SmartThreadPoolTests
    {
        private SmartThreadPool _threadPool;

        private const int MaxWorkerThreads = 2;

        [SetUp]
        public void Setup()
        {
            _threadPool = new SmartThreadPool(10 * 1000, MaxWorkerThreads, 0);

            //startInfo. 
            //_threadPool = new SmartThreadPool();
            //_threadPool
        }

        [Test]
        //copied from ThreadPool solution
        public void WaitForIdle()
        {
            ManualResetEvent isRunning = new ManualResetEvent(false);

            for (int i = 0; i < MaxWorkerThreads+1; ++i)
            {
                _threadPool.QueueWorkItem(delegate { isRunning.WaitOne(); });
            }

            bool success = !_threadPool.WaitForIdle(1000);

            isRunning.Set();

            success = success && _threadPool.WaitForIdle(1000);

            Assert.IsTrue(success);
        }

        [Test]
        public void QueueMoreNeverStoppingWorkItemsThanMaxThread_ExpectWaitingCallBacksToBeGreaterThanZero()
        {
            ManualResetEvent isRunning = new ManualResetEvent(false);

            for (int i = 0; i < MaxWorkerThreads + 2; ++i)
            {
                _threadPool.QueueWorkItem(delegate { isRunning.WaitOne(); });
            }

            bool success = !_threadPool.WaitForIdle(1000);

            int inUseThreads = _threadPool.InUseThreads;
            int waitingCallBacks = _threadPool.WaitingCallbacks;

            isRunning.Set();

            success = success && _threadPool.WaitForIdle(1000);

            Assert.IsTrue(inUseThreads == MaxWorkerThreads);
            Assert.IsTrue(waitingCallBacks == 2);
        } 

        [TearDown]
        public void TearDown()
        {
            _threadPool.Shutdown();
        }
    }
}
