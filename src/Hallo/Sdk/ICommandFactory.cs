using Hallo.Sdk.Commands;
using Hallo.Sdk.SoftPhoneStates;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.Sdk
{
    internal interface ICommandFactory
    {
        ICommand CreateIdleSendCommand(SipResponse ringingResponse, ISoftPhoneState transitionTo,  SipRequestEvent requestEvent);
        IInternalSoftPhone SoftPhone { get; set; }
        ICommand CreateInitialInviteRequestSendCommand(SipRequest request);
    }
}