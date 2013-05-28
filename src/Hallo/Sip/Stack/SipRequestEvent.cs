using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
        
        public SipRequestEvent(SipContext context)
        {
            this.Request = context.Request;
            this.LocalEndPoint = context.LocalEndPoint;
            this.RemoteEndPoint = context.RemoteEndPoint;
            this.IsSent = context.IsSent;
        }

        public ISipServerTransaction ServerTransaction { get; set; }
    }

    public class SipResponseEvent
    {
        public SipResponse Response { get; set; }

        public IPEndPoint LocalEndPoint { get; set; }

        public IPEndPoint RemoteEndPoint { get; set; }

        public bool IsSent { get; set; }

        public ISipClientTransaction ClientTransaction { get; set; }

        public SipResponseEvent(SipContext context)
        {
            this.Response = context.Response;
            this.LocalEndPoint = context.LocalEndPoint;
            this.RemoteEndPoint = context.RemoteEndPoint;
            this.IsSent = context.IsSent;
        }
    }
    
    public abstract class SipTimeOutEvent
    {
        public SipRequest Request { get; set; }
    }

    public class SipClientTxTimeOutEvent : SipTimeOutEvent
    {
        public ISipClientTransaction ClientTransaction { get; set; }
    }

    public class SipServerTxTimeOutEvent : SipTimeOutEvent
    {
        public ISipServerTransaction ServerTransaction { get; set; }
    }
    
}
