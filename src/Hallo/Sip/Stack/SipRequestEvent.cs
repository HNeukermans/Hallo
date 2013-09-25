using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.NonInviteServer;

namespace Hallo.Sip.Stack
{
    public class SipRequestEvent
    {
        public IPEndPoint LocalEndPoint { get; set; }

        public IPEndPoint RemoteEndPoint { get; set; }

        public SipRequest Request { get; set; }

        public SipResponse Response { get; set; }

        public bool IsHandled { get; set; }

        public bool IsSent { get; set; }

        public SipAbstractDialog Dialog { get; set; }
        
        public SipRequestEvent(SipContext context)
        {
            this.Request = context.Request;
            this.LocalEndPoint = context.LocalEndPoint;
            this.RemoteEndPoint = context.RemoteEndPoint;
            this.IsSent = context.IsSent;
        }

        public ISipServerTransaction ServerTransaction { get; set; }
        
    }
}
