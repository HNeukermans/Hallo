using Hallo.Sdk.SoftPhoneStates;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions.InviteServer;

namespace Hallo.Sdk
{
    internal interface IInternalSoftPhone : ISoftPhone
    {
        SipAccount Account { get; }
        InviteInfo PendingInvite { get; set; }
        ITimer RingingTimer { get; set; }
        SipAddressFactory AddressFactory { get; }
        SipHeaderFactory HeaderFactory { get; }
        ISipProvider SipProvider { get; }
        void ChangeState(ISoftPhoneState ringingState);
        ISoftPhoneState InternalState { get; }
        ICommandFactory CommandFactory { get; }
        SipMessageFactory MessageFactory { get; }
        int MessageCounter { get; set; }
        void RaiseIncomingCall();
    }

    public class InviteInfo
    {
        public SipRequest OriginalRequest { get; set; }
        public SipInviteServerTransaction Transaction { get; set; }
        public SipInviteServerDialog Dialog { get; set; }
        public SipResponse RingingResponse { get; set; }
    }
}