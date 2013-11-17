using Hallo.Sip;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.Sip.Stack.Transactions.InviteServer;


namespace Hallo.Sdk
{
    public class InviteInfo
    {
        public bool CancelOnWaitFinal { get; set; }
        public SipRequest OriginalRequest { get; set; }
        public SipInviteServerTransaction InviteServerTransaction { get; set; }
        public SipInviteClientTransaction InviteClientTransaction { get; set; }
        public SipAbstractDialog Dialog { get; set; }
        public SipResponse RingingResponse { get; set; }
        public SipUri From { get; set; }
        public SipUri To { get; set; }
        public bool IsIncomingCall { get; set; }
        public ISipClientTransaction CancelTransaction { get; set; }
        
    }
}