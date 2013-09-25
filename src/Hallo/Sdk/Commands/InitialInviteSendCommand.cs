using Hallo.Sdk.Commands;
using Hallo.Sdk.SoftPhoneStates;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.InviteClient;

namespace Hallo.Sdk
{
    internal class InitialInviteSendCommand : ICommand
    {
        private readonly SipRequest _request;
        private readonly ISoftPhoneState _transitionTo;
        private readonly IInternalSoftPhone _softPhone;

        public InitialInviteSendCommand(SipRequest request, ISoftPhoneState transitionTo, IInternalSoftPhone softPhone)
        {
            _request = request;
            _transitionTo = transitionTo;
            _softPhone = softPhone;
        }

        public void Execute()
        {
            var clientTransaction = _softPhone.SipProvider.CreateClientTransaction(_request);
            var dialog = _softPhone.SipProvider.CreateClientDialog(clientTransaction as SipInviteClientTransaction);
            clientTransaction.SendRequest();
            _softPhone.ChangeState(_transitionTo);
        }
    }
}