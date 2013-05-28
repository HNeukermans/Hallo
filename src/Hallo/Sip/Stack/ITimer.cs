using System;

namespace Hallo.Sip.Stack
{
    public interface ITimer : IDisposable
    {
        void Start();
        void Stop();
        int Interval { get; set; }
        bool IsDisposed { get; }
    }
}