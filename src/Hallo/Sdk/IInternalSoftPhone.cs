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
        SipMessageFactory MessageFactory { get; }
        int MessageCounter { get; set; }
        void RaiseIncomingCall(SipUri from);
        ISoftPhoneStateProvider StateProvider { get; }
    }

    public class InviteInfo
    {
        public SipRequest OriginalRequest { get; set; }
        public SipInviteServerTransaction InviteTransaction { get; set; }
        public SipInviteServerDialog Dialog { get; set; }
        public SipResponse RingingResponse { get; set; }
        public SipUri From { get; set; }
        public SipUri To { get; set; }
        public bool IsIncomingCall { get; set; }
    }
}