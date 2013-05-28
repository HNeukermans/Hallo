using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Hallo.Server
{
    public class SipServerConfiguration : SipConfigurationBase
    {
        public SipServerConfiguration()
            : base()
        {

            var localIp4Address = Dns.GetHostAddresses(string.Empty).FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
            BindIpEndPoint = localIp4Address.ToString() + ":33333";
        }

        [Category("Registrar")]
        [DisplayName("MinimumExpires")]
        [Browsable(true)]
        [ReadOnly(false)]
        [Bindable(false)]
        [DefaultValue(60)]
        [DesignOnly(false)]
        [Description("Determines the minimum expires value a client can suggest. " +
                      "Too small expire intervals will result in registrar bloating.")]
        public int RegistrarMinimumExpires { get; set; }

        [Category("Registrar")]
        [DisplayName("DefaultExpires")]
        [Browsable(true)]
        [ReadOnly(false)]
        [Bindable(false)]
        [DefaultValue(3600)]
        [DesignOnly(false)]
        [Description("Sets the default expiration time (sec)")]
        public int RegistrarDefaultExpires { get; set; }
    }
}
