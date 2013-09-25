using System.Net;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions;

namespace Hallo.Sip.Stack
{
    public class SipResponseEvent
    {
        public SipResponse Response { get; set; }

        public IPEndPoint LocalEndPoint { get; set; }

        public IPEndPoint RemoteEndPoint { get; set; }

        public bool IsSent { get; set; }

        public ISipClientTransaction ClientTransaction { get; set; }

        public SipAbstractDialog Dialog { get; set; }

        public SipResponseEvent(SipContext context)
        {
            this.Response = context.Response;
            this.LocalEndPoint = context.LocalEndPoint;
            this.RemoteEndPoint = context.RemoteEndPoint;
            this.IsSent = context.IsSent;
        }
    }
}