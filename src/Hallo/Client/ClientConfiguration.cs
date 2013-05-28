using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Hallo.Client.Logic
{
    public class ClientConfiguration : SipConfigurationBase
    {
        [Category("Stack")]
        [Browsable(true)]
        [ReadOnly(false)]
        [Bindable(false)]
        [DesignOnly(false)]
        [Description("Enter the outbound proxy IP endpoint.")]
        public string OutboundProxyIpEndPoint { get; set; }

        public ClientConfiguration(): base()
        {
            
        }

    }
}
