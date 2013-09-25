using System;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.Sdk
{
    public interface IPhoneCall
    {
        event EventHandler<VoipEventArgs<CallError>> CallErrorOccured;
        void Invite(string to);

        void Answer();

        SipAddress From { get; }

        SipUri GetToUri();
    }

    public class CallError
    {

    }
}