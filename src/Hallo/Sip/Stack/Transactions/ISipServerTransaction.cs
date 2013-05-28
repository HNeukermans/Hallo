using Hallo.Sip.Stack.Dialogs;

namespace Hallo.Sip.Stack.Transactions.NonInviteServer
{
    public interface ISipServerTransaction : IRequestProcessor
    {
        void SendResponse(SipResponse response);
        SipRequest Request { get; }
    }

    public interface ISipInviteServerTransaction : ISipServerTransaction
    {
        void SetDialog(SipInviteServerDialog dialog);
    }


    public interface IRequestProcessor
    {
        void ProcessRequest(SipRequestEvent requestEvent);
    }

    public interface IResponseProcessor
    {
        void ProcessResponse(SipResponseEvent responseEvent);
    }
}