using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sdk.SoftPhoneStates;

namespace Hallo.Sdk
{
    internal interface ISoftPhoneStateProvider
    {
        ISoftPhoneState GetRinging();
        ISoftPhoneState GetIdle();
        ISoftPhoneState GetWaitForAck();

        ISoftPhoneState GetEstablished();
    }
}
