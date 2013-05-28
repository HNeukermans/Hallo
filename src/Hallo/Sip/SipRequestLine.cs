using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip
{
    public class SipRequestLine : IStringFormatable
    {
        public string Method { get; set; }
        public SipUri Uri { get; set; }
        public string Version { get; set; }

        public string FormatToString()
        {
            return Method + " " + Uri.FormatToString() + " " + Version; 
        }
    }
}
