using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Hallo.Sip.Stack.Transactions.NonInviteServer;

namespace Hallo.Sip.Stack
{
    public interface ISipListener : IRequestProcessor, IResponseProcessor
    {
        void ProcessTimeOut(SipTimeOutEvent timeOutEvent);
        
    }
}
