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
        private Action<ISoftPhoneState> _afterProcessResponse;
        private readonly Action<ISoftPhoneState> _afterInitialized;
        private Action<ISoftPhoneState> _afterProcessRequest;

        public SoftPhoneStateProxy(ISoftPhoneState state)
        {
            _state = state;
            _afterProcessRequest = delegate { };
            _afterProcessResponse = delegate { };
            _afterInitialized = delegate { };
        }

        public SoftPhoneStateProxy(ISoftPhoneState state, Action<ISoftPhoneState> afterProcessRequest, Action<ISoftPhoneState> afterProcessResponse, Action<ISoftPhoneState> afterInitialized)
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

            _afterInitialized(_state);
        }

        public void ProcessRequest(IInternalSoftPhone softPhone, SipRequestEvent requestEvent)
        {
            ProcessRequestCounter++;

            _state.ProcessRequest(softPhone, requestEvent);

            _afterProcessRequest(_state);
        }

        public void ProcessResponse(IInternalSoftPhone softPhone, SipResponseEvent responseEvent)
        {
            ProcessResponseCounter++;

            _state.ProcessResponse(softPhone, responseEvent);

            _afterProcessResponse(_state);
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
