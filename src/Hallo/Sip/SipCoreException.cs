using System;

namespace Hallo.Sip
{
    public class SipCoreException : Exception
    {
        public SipCoreException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }

        public SipCoreException(string message)
            : base(message)
        {
        }
    }
}