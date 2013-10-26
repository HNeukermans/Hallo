using Hallo.Sdk.SoftPhoneStates;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.Sip.Stack.Transactions.InviteServer;
using System;

namespace Hallo.Sdk
{
    internal interface IInternalSoftPhone : ISoftPhone
    {
        SipAccount Account { get; }
        InviteInfo PendingInvite { get; set; }
        ITimer RetransmitRingingTimer { get; set; }
        ITimer EndWaitForAckTimer { get; set; }
        SipAddressFactory AddressFactory { get; }
        SipHeaderFactory HeaderFactory { get; }
        ISipProvider SipProvider { get; }
        void ChangeState(ISoftPhoneState ringingState);
        ISoftPhoneState InternalState { get; }
        SipMessageFactory MessageFactory { get; }
        int MessageCounter { get; set; }
        void RaiseIncomingCall(SipUri from);
        ISoftPhoneStateProvider StateProvider { get; }
        event EventHandler<EventArgs> InternalStateChanged;
    }

    public class InviteInfo
    {
        public SipRequest OriginalRequest { get; set; }
        public SipInviteServerTransaction InviteServerTransaction { get; set; }
        public SipInviteServerDialog ServerDialog { get; set; }
        public SipInviteClientTransaction InviteClientTransaction { get; set; }
        public SipInviteClientDialog ClientDialog { get; set; }
        public SipResponse RingingResponse { get; set; }
        public SipUri From { get; set; }
        public SipUri To { get; set; }
        public bool IsIncomingCall { get; set; }
        public SipInviteClientTransaction InviteSendTransaction { get; set; }
    }
}