using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sdk.SoftPhoneStates;
using Hallo.Sdk;
using Hallo.Sip.Stack;

namespace Hallo.UnitTest.Helpers
{
    internal class SoftPhoneStateProxy : ISoftPhoneState
    {
        ISoftPhoneState _state;
        private Action<IInternalSoftPhone, SipResponseEvent> _afterProcessResponse;
        private readonly Action<IInternalSoftPhone> _afterInitialized;
        private Action<IInternalSoftPhone, SipRequestEvent> _afterProcessRequest;

        public SoftPhoneStateProxy(ISoftPhoneState state)
        {
            _state = state;
            _afterProcessRequest = delegate { };
            _afterProcessResponse = delegate { };
            _afterInitialized = delegate { };
        }

        public ISoftPhoneState State 
        {
            get { return _state; }
        }

        public SoftPhoneStateProxy(ISoftPhoneState state, Action<IInternalSoftPhone, SipRequestEvent> afterProcessRequest, Action<IInternalSoftPhone, SipResponseEvent> afterProcessResponse, Action<IInternalSoftPhone> afterInitialized)
        {
            _state = state;
            _afterProcessRequest = afterProcessRequest;
            _afterProcessResponse = afterProcessResponse;
            _afterInitialized = afterInitialized;
        }

        public int ProcessRequestCounter { get; set; }

        public int ProcessResponseCounter { get; set; }

        public void Initialize(IInternalSoftPhone softPhone)
        {
            _state.Initialize(softPhone);

            _afterInitialized(softPhone);
        }

        public void ProcessRequest(IInternalSoftPhone softPhone, SipRequestEvent requestEvent)
        {
            ProcessRequestCounter++;

            _state.ProcessRequest(softPhone, requestEvent);

            _afterProcessRequest(softPhone, requestEvent);
        }

        public void ProcessResponse(IInternalSoftPhone softPhone, SipResponseEvent responseEvent)
        {
            ProcessResponseCounter++;

            _state.ProcessResponse(softPhone, responseEvent);

            _afterProcessResponse(softPhone, responseEvent);
        }

        public SoftPhoneState StateName
        {
            get { return _state.StateName; }
        }


        public void Terminate(IInternalSoftPhone softPhone)
        {
            _state.Terminate(softPhone);
        }
    }

}
