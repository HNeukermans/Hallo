using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Hallo.Sip.Stack;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.TxTimerTests
{
    [TestFixture]
    public class TxTimerTest
    {
        private AutoResetEvent _wait;

        [Test]
        public void TestStartStop()
        {
            _wait = new AutoResetEvent(false);

            var t = new TxTimer(CallBack, 2000, true);
           
            Console.WriteLine("Start time: " + DateTime.Now); 
            t.Start();
            
            _wait.WaitOne();
            Console.WriteLine(DateTime.Now);
            _wait.WaitOne();
            t.Stop();
            Console.WriteLine("Stop time: "+ DateTime.Now);
            _wait.WaitOne(TimeSpan.FromSeconds(5));
            Console.WriteLine("End time: equal to period or to timeout ? " + DateTime.Now);

            t.Start();
            _wait.WaitOne();
            Console.WriteLine(DateTime.Now);

            
        }

        [Test]
        public void TestChangeInterval()
        {
            _wait = new AutoResetEvent(false);

            var interval = 2000;

            Console.WriteLine("Interval time: " + interval);

            var t = new TxTimer(CallBack, interval, true);

            Console.WriteLine("Start time: " + DateTime.Now);
            t.Start();

            _wait.WaitOne();
            Console.WriteLine("Must be an interval later : "+ DateTime.Now);
            
            interval = 3000;

            Console.WriteLine("Changing Interval to : " + interval);
            t.Interval = interval;
            _wait.WaitOne(TimeSpan.FromSeconds(5));

            Console.WriteLine("Must be an interval later : "+ DateTime.Now);

        }

        private void CallBack()
        {
            _wait.Set();
        }
    }
}
