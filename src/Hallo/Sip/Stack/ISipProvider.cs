using System.Collections.Concurrent;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions;

namespace Hallo.Sip
{
    public interface ISipProvider : ISipMessageSender//, IRequestSender, IResponseSender
    {
        //SipListeningPoint ListeningPoint { get; }
        
        //ISipClientTransaction CreateClientTransaction(SipRequest request);
        //void CreateNewClientTransaction();
        //void AddSipListener(ISipListener sipListener);
        //SipProviderDiagnosticInfo GetDiagnosticsInfo();

        /// <summary>
        /// the listener that gets notified when new messages are received by the tranports
        /// </summary>
        /// <remarks>
        /// had to make this member public for unit testing purposes.
        /// </remarks>
        ISipListener SipListener { get; }
    }

    public interface ISipMessageSender
    {
        /// <summary>
        /// sends a response to the underlying transport
        /// </summary>
        new void SendResponse(SipResponse response);

        /// <summary>
        /// sends a request to the underlying transport
        /// </summary>
        new void SendRequest(SipRequest request);
    }

    //internal interface ISupportClientTransaction
    //{
    //    void SendRequest(SipRequest request);
    //    SipTransactionTable Transactions { get; }
    //}
}