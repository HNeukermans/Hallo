using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Hallo.Sip.Util;
using Hallo.Util;

namespace Hallo.Sip.Stack.Transactions.InviteClient
{
    internal abstract class AbstractCtxState
    {
        public abstract SipTransactionStateName Name { get; }

        internal abstract void Initialize(SipInviteClientTransaction transaction);
        internal abstract void Retransmit(SipInviteClientTransaction transaction);
        internal abstract StateResult HandleResponse(SipInviteClientTransaction ctx, SipResponse response);
        
    }

}
