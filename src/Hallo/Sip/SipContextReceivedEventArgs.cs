using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Hallo.Sip
{
    public class SipContextReceivedEventArgs : EventArgs
    {
        public SipContext Context { get; set; }
    }

    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
    }
}
