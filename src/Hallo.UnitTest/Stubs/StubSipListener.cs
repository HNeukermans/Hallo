using System;
using Hallo.Sip.Stack;

namespace Hallo.UnitTest.Stubs
{
    public class StubSipListener : ISipListener
    {
        public Action<SipRequestEvent> OnProcessRequest { get; set; }
        public Action<SipResponseEvent> OnProcessResponse { get; set; }
        public Action<SipTimeOutEvent> OnTimeOutEvent { get; set; }

        public StubSipListener(Action<SipRequestEvent> onProcessRequest = null, Action<SipResponseEvent> onProcessResponse=null, Action<SipTimeOutEvent> onTimeOutEvent=null)
        {
            OnProcessRequest = onProcessRequest ?? delegate { };
            OnProcessResponse = onProcessResponse ?? delegate { };
            OnTimeOutEvent = onTimeOutEvent ?? delegate { };
        }


        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            OnProcessRequest(requestEvent);
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
            OnProcessResponse(responseEvent);
        }

        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {
            OnTimeOutEvent(timeOutEvent);
        }
    }
}
