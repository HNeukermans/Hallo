using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sdk.SoftPhoneStates
{
    internal class EstablishedState : ISoftPhoneState
    {
        public void Initialize(IInternalSoftPhone softPhone)
        {
            
        }

        public void ProcessRequest(IInternalSoftPhone softPhone, Sip.Stack.SipRequestEvent requestEvent)
        {
            
        }

        public void ProcessResponse(IInternalSoftPhone softPhone, Sip.Stack.SipResponseEvent responseEvent)
        {
            
        }

        public void Terminate(IInternalSoftPhone softPhone)
        {
            
        }

        public SoftPhoneState StateName
        {
            get { return SoftPhoneState.Established; }
        }
    }
}
