using System;

namespace Hallo.Sdk
{
    public interface IPhoneCall
    {
        event EventHandler<VoipEventArgs<CallError>> CallErrorOccured;

        event EventHandler<VoipEventArgs<CallState>> CallStateChanged;

        CallState State { get; }

        void Start(string to);

        void Accept();

        void Stop();

        bool IsIncoming { get; }
    }
}