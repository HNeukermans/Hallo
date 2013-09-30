using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sdk.Commands;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.Sdk.SoftPhoneStates
{
    internal interface ISoftPhoneState
    {
        void Initialize(IInternalSoftPhone softPhone);

        void ProcessRequest(IInternalSoftPhone softPhone, SipRequestEvent requestEvent);

        void ProcessResponse(IInternalSoftPhone softPhone, SipResponseEvent responseEvent);

        SoftPhoneState StateName { get; }
    }
}
