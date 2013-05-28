using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hannes.Net.Udp;

namespace Hannes.Net
{
    public interface ISipModule
    {
        void Execute(SipContext context);
    }
}
