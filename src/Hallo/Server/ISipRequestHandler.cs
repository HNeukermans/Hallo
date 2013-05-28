using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip.Stack;

namespace Hallo.Server
{
    public interface ISipRequestHandler
    {
        void ProcessRequest(SipRequestEvent requestEvent);
    }
}
