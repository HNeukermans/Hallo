using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip.Stack
{
    public class SipAddress
    {
        internal SipAddress()
        {
        }

        public string DisplayInfo { get; set; }
        public SipUri Uri { get; set; }
    }
}
