using Hallo.Sip.Stack.Dialogs;

namespace Hallo.Sip.Stack.Transactions.NonInviteServer
{
    /// <summary>
    /// server transaction mocking interface. (initialy intended as a facade interface, but for that purpose
    /// a separate interface is needed. (bc mocking needs more behavior then facade))
    /// </summary>
    public interface ISipServerTransaction : IRequestProcessor
    {
        void SendResponse(SipResponse response);
        SipRequest Request { get; }
    }

    //public interface ISipInviteServerTransaction : ISipServerTransaction
    //{
    //    void SetDialog(SipInviteServerDialog dialog);
    //}


    public interface IRequestProcessor
    {
        void ProcessRequest(SipRequestEvent requestEvent);
    }

    public interface IResponseProcessor
    {
        void ProcessResponse(SipResponseEvent responseEvent);
    }
}