using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Util;

namespace Hallo.Sip.Headers
{
    [HeaderFor(SipHeaderNames.Route)]
    public class SipRouteHeader : SipRouteHeaderBase<SipRouteHeader>
    {
        internal SipRouteHeader()
            : base()
        {
            Name = SipHeaderNames.Route;
        }
    }
}
