using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.UnitTest.Stubs
{
    public class SipReceivedMessageProcessorStub : ISipListener
    {
        private readonly Action<SipRequest, SipContext> _onRecievedRequest;
        private readonly Action<SipResponse, SipContext> _onRecievedResponse;
        
        public List<SipRequest> Requests { get; set; }
        public List<SipResponse> Responses { get; set; }

        private object _locker = new object();

        public SipReceivedMessageProcessorStub(Action<SipRequest, SipContext> onRecievedRequest, Action<SipResponse, SipContext> onRecievedResponse)
        {
            _onRecievedRequest = onRecievedRequest;
            _onRecievedResponse = onRecievedResponse;
            Requests = new List<SipRequest>();
            Responses = new List<SipResponse>();
        }

        public void ProcessRequest(SipRequest sipRequest, SipContext context)
        {
            lock (_locker) Requests.Add(sipRequest);
            _onRecievedRequest(sipRequest, context);
            
        }

        public void ProcessResponse(SipResponse sipResponse, SipContext context)
        {
            lock (_locker) Responses.Add(sipResponse);
            _onRecievedResponse(sipResponse, context);
            
        }

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            throw new NotImplementedException();
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
            throw new NotImplementedException();
        }

        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {
            throw new NotImplementedException();
        }
    }
}
