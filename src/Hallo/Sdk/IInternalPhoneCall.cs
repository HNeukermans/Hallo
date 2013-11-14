using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.Sdk
{
    public interface IInternalPhoneCall : IPhoneCall
    {
        void ChangeState(CallState state);
        void RaiseCallErrorOccured(CallError error);
        SipAddress From { get; }
        SipUri GetToUri();
        
    }
}