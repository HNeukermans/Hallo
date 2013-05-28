using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Hallo.Sip.Stack;
using Hallo.Sip.Util;
using Hallo.Util;

namespace Hallo.Server
{
    public class SipAddressBinding
    {
        internal SipAddressBinding(string aor, IPEndPoint host, int cSeq, string callId, int expires)
        {
            AddressOfRecord = aor;
            Host = host;
            CSeq = cSeq;
            CallId = callId;
            StartTime = DateTime.Now;
            EndTime = StartTime.AddSeconds(expires);
            Expires = expires;
        }

        public IPEndPoint Host { get; set; }

        public string AddressOfRecord { get; private set; }
        
        public int CSeq { get; private set; }

        public string CallId { get; private set; }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        
        public int Expires { get; private set; }
       
    }
}