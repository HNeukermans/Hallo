using System;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.Sdk
{
    public interface IPhoneCall
    {
        event EventHandler<VoipEventArgs<CallError>> CallErrorOccured;
        void Start(string to);

        void Accept();

        SipAddress From { get; }

        SipUri GetToUri();

        void RaiseCallErrorOccured(CallError error);       
    }

    public enum CallError
    {
        WaitForAckTimeOut
    }
}