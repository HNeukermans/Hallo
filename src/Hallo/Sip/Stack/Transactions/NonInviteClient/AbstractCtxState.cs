using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Hallo.Util;

namespace Hallo.Sip.Stack.Transactions.NonInviteClient
{
    internal abstract class AbstractCtxState
    {
        public abstract SipTransactionStateName Name { get; }

        internal abstract void Initialize(SipNonInviteClientTransaction transaction);
        internal abstract void Retransmit(SipNonInviteClientTransaction transaction);
        internal abstract StateResult HandleResponse(SipNonInviteClientTransaction ctx, SipResponse response);
 
    }

}
