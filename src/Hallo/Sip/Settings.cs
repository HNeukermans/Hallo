using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Hallo.Sip
{
    public class Settings
    {
        public IPEndPoint ProxyServer { get; set; }
        public IPEndPoint Registrar { get; set; }
    }
}
