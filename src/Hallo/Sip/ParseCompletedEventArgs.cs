using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip
{
    public class ParseCompletedEventArgs : EventArgs
    {
        public SipMessage Message { get; set; }
    }
}
