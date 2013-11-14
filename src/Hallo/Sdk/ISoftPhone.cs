using System;
using System.Net;
using System.Reactive.Linq;
using Hallo.Sdk.SoftPhoneStates;

namespace Hallo.Sdk
{
    public interface ISoftPhone
    {
        IPhoneLine CreatePhoneLine(SipAccount account);
        IPhoneCall CreateCall();
        event EventHandler<VoipEventArgs<IPhoneCall>> IncomingCall;
        void Start();
        event EventHandler<VoipEventArgs<SoftPhoneState>> StateChanged;
        SoftPhoneState CurrentState { get; }
        bool IsRunning { get; }
    }
}