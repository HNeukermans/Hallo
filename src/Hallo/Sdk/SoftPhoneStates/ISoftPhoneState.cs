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
        //IInternalSoftPhone SoftPhone { get; }

        void Initialize();

        ICommand ProcessRequest(SipRequestEvent requestEvent);

        void ProcessResponse(SipResponseEvent responseEvent);
        SoftPhoneState StateName { get; }
    }

   
    //internal class TransitionResult : ISendCommand
    //{
    //    public ISoftPhoneState TransitionTo { get; set; }
    //}

    //internal class EmptyResult : ISendCommand
    //{

    //}
}
