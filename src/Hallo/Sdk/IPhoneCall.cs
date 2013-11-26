using System;

namespace Hallo.Sdk
{
    public interface IPhoneCall
    {
        event EventHandler<VoipEventArgs<CallErrorObject>> CallErrorOccured;

        event EventHandler<VoipEventArgs<CallState>> CallStateChanged;

        CallState State { get; }

        void Start(string to);

        void Accept();

        void Reject();

        void Stop();

        bool IsIncoming { get; }
    }
}