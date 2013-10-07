using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sdk.SoftPhoneStates;
using Hallo.Sdk;
using Hallo.Sip.Stack;

namespace Hallo.UnitTest.Helpers
{
    internal class InvocationCounter : ISoftPhoneState
    {
        ISoftPhoneState _state;

        public InvocationCounter(ISoftPhoneState state)
        {
            _state = state;
        }

        public int ProcessRequestCounter { get; set; }

        public int ProcessResponseCounter { get; set; }

        public void Initialize(IInternalSoftPhone softPhone)
        {
            _state.Initialize(softPhone);
        }

        public void ProcessRequest(IInternalSoftPhone softPhone, SipRequestEvent requestEvent)
        {
            ProcessRequestCounter++;

            _state.ProcessRequest(softPhone, requestEvent);
        }

        public void ProcessResponse(IInternalSoftPhone softPhone, SipResponseEvent responseEvent)
        {
            ProcessResponseCounter++;

            _state.ProcessResponse(softPhone, responseEvent);
        }

        public SoftPhoneState StateName
        {
            get { throw new NotImplementedException(); }
        }


        public void Terminate(IInternalSoftPhone softPhone)
        {
            throw new NotImplementedException();
        }
    }

}
