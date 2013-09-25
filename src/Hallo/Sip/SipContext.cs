using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.Sip
{
    public class SipContext
    {
        internal SipContext()
        {
            
        }
        
        public IPEndPoint RemoteEndPoint { get; set; }
        public IPEndPoint LocalEndPoint  { get; set; }
     
        public SipRequest Request { get; set; }
        public SipResponse Response { get; set; }

        public bool IsHandled { get; set; }
        
        public bool IsSent { get; set; }

        internal void Update(SipRequestEvent requestEvent)
        {
            Response = requestEvent.Response;
            IsHandled = requestEvent.IsHandled;
            IsSent = requestEvent.IsSent;
        }

        internal void Update(SipResponseEvent responseEvent)
        {
            
        }
    }
}
