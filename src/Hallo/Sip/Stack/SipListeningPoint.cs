﻿using System.Net;

namespace Hallo.Sip
{
    public class SipListeningPoint : IPEndPoint
    {
        internal    SipListeningPoint(IPAddress address, int port)
            : base(address, port)
        {
        }

        internal SipListeningPoint(long address, int port)
            : base(address, port)
        {
        }

        internal SipListeningPoint(IPEndPoint endPoint)
            : base(endPoint.Address, endPoint.Port)
        {
        }

    }
}
