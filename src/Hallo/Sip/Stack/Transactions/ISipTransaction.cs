using Hallo.Sip.Stack.Dialogs;

namespace Hallo.Sip.Stack.Transactions
{
    public interface ISipTransaction
    {
        SipRequest Request { get; }
        SipTransactionType Type { get; } //really ??
        string GetId();
        SipAbstractDialog GetDialog();
        void SetDialog(SipAbstractDialog dialog);
    }
}