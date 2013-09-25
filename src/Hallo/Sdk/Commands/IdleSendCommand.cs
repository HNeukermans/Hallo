using System;
using Hallo.Sdk.Commands;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions.InviteServer;

namespace Hallo.Sdk.SoftPhoneStates
{
    internal class IdleSendCommand : ICommand
    {
        private readonly IInternalSoftPhone _softPhone;
        private readonly SipRequestEvent _requestEvent;
        private ISoftPhoneState _transitionTo;
        private readonly SipResponse _response;

        public SipResponse Response
        {
            get { return _response;  }
        }

        public IdleSendCommand(SipResponse response, ISoftPhoneState transitionTo, SipRequestEvent requestEvent, IInternalSoftPhone softPhone)
        {
            _softPhone = softPhone;
            _requestEvent = requestEvent;
            _transitionTo = transitionTo;
            _response = response;
        }

        public void Execute()
        {
            var serverTransaction = _softPhone.SipProvider.CreateServerTransaction(_requestEvent.Request);
            var dialog = _softPhone.SipProvider.CreateServerDialog(serverTransaction as SipInviteServerTransaction);
            serverTransaction.SendResponse(_response);
            _requestEvent.IsSent = true;
            _softPhone.ChangeState(_transitionTo);
        }
    }
}