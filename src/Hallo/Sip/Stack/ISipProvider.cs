using System.Collections.Concurrent;
using Hallo.Sdk;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.Sip.Stack.Transactions.NonInviteServer;

namespace Hallo.Sip
{
    public interface ISipProvider : ISipMessageSender
    {
        ISipServerTransaction CreateServerTransaction(SipRequest request);
        
        SipInviteServerDialog CreateServerDialog(ISipServerTransaction transaction);

        void AddSipListener(ISipListener listener);
        void AddExceptionHandler(IExceptionHandler handler);
        void Start();
        void Stop();
        ISipClientTransaction CreateClientTransaction(SipRequest invite);
        SipInviteClientDialog CreateClientDialog(ISipClientTransaction transaction);
        ISipServerTransaction FindServerTransactionById(string id);
    }
    
}