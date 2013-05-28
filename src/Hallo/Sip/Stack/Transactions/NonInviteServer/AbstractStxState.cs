using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Hallo.Util;

namespace Hallo.Sip.Stack.Transactions.NonInviteServer
{
    internal abstract class AbstractStxState
    {
        public abstract SipTransactionStateName Name { get; }
        internal abstract void Initialize(SipNonInviteServerTransaction transaction);
        internal abstract StateResult ProcessRequest(SipNonInviteServerTransaction ctx, SipRequestEvent request);
        internal abstract void HandleSendingResponse(SipNonInviteServerTransaction transaction, SipResponse response);
   
    }

}
