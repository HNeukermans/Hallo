using Hallo.Sip.Stack.Dialogs;

namespace Hallo.Sip.Stack.Transactions
{
    /// <summary>
    /// server transaction mocking interface. (initialy intended as a facade interface, but for that purpose
    /// a separate interface is needed. (bc mocking needs more behavior then facade))
    /// </summary>
    public interface ISipServerTransaction : ISipRequestProcessor
    {
        void SendResponse(SipResponse response);
        SipRequest Request { get; }
    }

    //public interface ISipInviteServerTransaction : ISipServerTransaction
    //{
    //    void SetDialog(SipInviteServerDialog dialog);
    //}
}