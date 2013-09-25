using System.Net;

namespace Hallo.Sdk
{
    public class SipAccount
    {
        //public bool IsRegistrationRequired { get { return true; } }
        public string UserName { get; set; }
        public string Domain { get; set; }
        //public IPEndPoint MyIpEndPoint { get; set; } remove this. SoftPhone start() use this SipListener.ListeningPoint.
        public IPEndPoint OutboundProxy { get; set; }
    }
}