using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.Sdk
{
    public interface IInternalPhoneCall : IPhoneCall
    {
        void RaiseCallStateChanged(CallState state);
        void RaiseCallErrorOccured(CallError error, string message = null);
        SipAddress From { get; }
        SipUri GetToUri();
        
    }
}