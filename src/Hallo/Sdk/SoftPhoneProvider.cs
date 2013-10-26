﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sdk.SoftPhoneStates;

namespace Hallo.Sdk
{
    internal class SoftPhoneStateProvider : ISoftPhoneStateProvider
    {
        public readonly ISoftPhoneState _ringing = new RingingState();
        public readonly ISoftPhoneState _idle = new IdleState();
        public readonly ISoftPhoneState _waitForAck = new WaitForAckState();
        public readonly ISoftPhoneState _established = new EstablishedState();
        public readonly ISoftPhoneState _waitProv = new WaitProvisionalState();
        public readonly ISoftPhoneState _waitFinal = new WaitProvisionalState();

        public ISoftPhoneState GetRinging()
        {
            return _ringing;
        }

        public ISoftPhoneState GetIdle()
        {
            return _idle;
        }

        public ISoftPhoneState GetWaitForAck()
        {
            return _waitForAck;
        }
        
        public ISoftPhoneState GetEstablished()
        {
            return _established;
        }

        public ISoftPhoneState GetWaitFinal()
        {
            return _waitFinal;
        }
        
        public ISoftPhoneState GetWaitProvisional()
        {
            return _waitProv;
        }
    }
}
