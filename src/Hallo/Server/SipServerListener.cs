using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip.Stack;

namespace Hallo.Server
{
    public class SipServerListener : ISipListener
    {
        List<ISipRequestHandler> _requestHandlers = new List<ISipRequestHandler>();

        public void AddRequestHandler(ISipRequestHandler handler)
        {
            _requestHandlers.Add(handler);
        }

        public SipServerListener()
        {
            
        }

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            var enumerator = _requestHandlers.GetEnumerator();

            while(enumerator.MoveNext())
            {
                enumerator.Current.ProcessRequest(requestEvent);
                if (requestEvent.IsHandled) break;
            }
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
            
        }


        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {
            throw new NotImplementedException();
        }
    }
}
