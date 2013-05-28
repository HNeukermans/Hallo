using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

namespace Hannes.Net.Test.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.Console.WriteLine(DateTime.Now);
            ////var s = new MyScheduler();
            //var numbers = new List<long> {1, 3, 7, 15, 23, 31, 39, 47, 55, 63};
            ////Observable.T
            //var r = Observable.Interval(TimeSpan.FromMilliseconds(500), MyScheduler.Instance).Where(i => numbers.Contains(i+1)).Timestamp(MyScheduler.Instance);
            ////var r = Observable.Timer(TimeSpan.FromSeconds(3), new Scheduler()).Select(i => DateTime.Now);
            ////var r = Observable.Timer(TimeSpan.FromSeconds(3)).Select(i => DateTime.Now);
            //var subscription = r.Subscribe(d => System.Console.WriteLine(d.Timestamp.DateTime.ToString() + " " + d.Timestamp.DateTime.Millisecond));
            ////subscription.Dispose();
            //System.Console.ReadLine();
        }

        static private List<long> CreateNumbers()
        {
            var result = new List<long>();

            for(int i=1;i<int.MaxValue;i++)
            {
                var p = (long) Math.Pow(2, i) - 1;
                if(p > 64) break;
                result.Add(p);
            }
            return result;
        }

        //public class MyScheduler : IScheduler
        //{
        //    private static readonly MyScheduler _instance;

        //    private MyScheduler()
        //    {
        //    }

        //    static MyScheduler()
        //    {
        //        _instance = new MyScheduler();
        //    }

        //    public static MyScheduler Instance
        //    {
        //        get { return _instance; }
        //    }


        //    public DateTimeOffset Now
        //    {
        //        get { return DateTime.Now; }
        //    }

        //    public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        //    {
        //        var t = new Timer(new TimerCallback(o => action(this, state)), null, dueTime, TimeSpan.FromMilliseconds(-1));
        //        //t.Change() 
        //        //System.Console.WriteLine(state);
        //        //System.Console.WriteLine(dueTime);
        //        //return action(this, state);
        //    }

        //    public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}
