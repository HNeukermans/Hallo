using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public SipMessage Message { get; set; }

        public MessageReceivedEventArgs(SipMessage message)
        {
            Message = message;
        }
    }
}
