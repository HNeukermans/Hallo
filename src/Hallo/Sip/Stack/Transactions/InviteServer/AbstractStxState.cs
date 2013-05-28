using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Hallo.Sip.Util;
using Hallo.Util;

namespace Hallo.Sip.Stack.Transactions.InviteServer
{
    internal abstract class AbstractStxState
    {
        public abstract SipTransactionStateName Name { get; }
        internal abstract void Initialize(SipInviteServerTransaction transaction);
        internal abstract StateResult ProcessRequest(SipInviteServerTransaction transaction, SipRequestEvent request);
        internal abstract StateResult HandleSendingResponse(SipInviteServerTransaction transaction, SipResponse response);

    }

}
