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
        /// <summary>
        /// the listener that gets notified when new messages are received by the tranports
        /// </summary>
        /// <remarks>
        /// had to make this member public for unit testing purposes.
        /// </remarks>
        ISipListener SipListener { get; }

        SipListeningPoint ListeningPoint { get; }
        
        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// had to make this member public for unit testing purposes.
        /// </remarks>
        ISipServerTransaction CreateServerTransaction(SipRequest request);


        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// had to make this member public for unit testing purposes.
        /// </remarks>
        SipInviteServerDialog CreateServerDialog(ISipServerTransaction transaction);

        void AddSipListener(ISipListener listener);
        void AddExceptionHandler(IExceptionHandler handler);
        void Start();
        void Stop();
        ISipClientTransaction CreateClientTransaction(SipRequest invite);
        SipInviteClientDialog CreateClientDialog(ISipClientTransaction transaction);
    }
    
}