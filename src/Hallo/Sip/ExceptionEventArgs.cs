using System;

namespace Hallo.Sip
{
    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
    }
}