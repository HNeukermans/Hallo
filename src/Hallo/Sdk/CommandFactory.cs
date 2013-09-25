using Hallo.Sdk.Commands;
using Hallo.Sdk.SoftPhoneStates;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions.InviteServer;

namespace Hallo.Sdk
{
    internal class CommandFactory : ICommandFactory
    {
        public ICommand CreateIdleSendCommand(SipResponse ringingResponse, ISoftPhoneState transitionTo, SipRequestEvent requestEvent)
        {
            return new IdleSendCommand(ringingResponse, transitionTo, requestEvent, SoftPhone);
        }

        public IInternalSoftPhone SoftPhone { get; set; }
        

        public ICommand CreateInitialInviteRequestSendCommand(SipRequest request)
        {
            return null; new InitialInviteSendCommand(request, new RingingState(SoftPhone, true), SoftPhone);
        }
    }
}