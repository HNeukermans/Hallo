using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Util;
using Hallo.Util;

namespace Hallo.Sip.Stack
{
    public class SipAddressFactory
    {
        internal SipAddressFactory()
        {}

        public SipAddress CreateAddress(string displayInfo, SipUri uri)
        {
            return new SipAddress() { DisplayInfo = displayInfo, Uri = uri};
        }

        public SipUri CreateUri(string user, string hostOrDomain)
        {
            Check.NotNullOrEmpty(hostOrDomain, "hostOrDomain");
           
            var splitted = hostOrDomain.Split(':').ToList();

            Check.IsTrue(splitted.Count, x=> x <= 2, "hostOrDomain has invalid format");

            int port = -1;
            if (splitted.Count == 2)
            {
                Check.IsTrue(splitted[1], x => int.TryParse(x, out port), "hostOrDomain has invalid format");
            }

            var uri = new SipUri() {User = user, Host = splitted[0]};
            if (port > -1) uri.Port = port;

            return uri;
        }
    }
}
