using Hallo.Sdk.SoftPhoneStates;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions;
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
        void ChangeState(ISoftPhoneState state);
        IInternalPhoneCall PendingCall { get; }
        ISoftPhoneState InternalState { get; }
        SipMessageFactory MessageFactory { get; }
        int MessageCounter { get; set; }
        void RaiseIncomingCall();
        ISoftPhoneStateProvider StateProvider { get; }
        event EventHandler<EventArgs> InternalStateChanged;
        void SendCancel();
        SoftPhoneState CurrentState { get; }
        bool IsRunning { get; }
    }
}