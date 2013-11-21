using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Net;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.UnitTest.Helpers
{
    public class TestConstants
    {
        public static IPEndPoint LocalEndPoint1 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 22221);
        public static IPEndPoint LocalEndPoint2 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 22222);
        public static IPAddress MyIpAddress = Dns.GetHostAddresses("").First(i=> i.AddressFamily == AddressFamily.InterNetwork);
        public static IPAddress IpAddress1 = IPAddress.Parse("192.168.0.1");
        public static IPAddress IpAddress2 = IPAddress.Parse("192.168.0.2");
        public static IPEndPoint IpEndPoint1 = new IPEndPoint(IpAddress1, SipConstants.DefaultSipPort);
        public static IPEndPoint IpEndPoint2 = new IPEndPoint(IpAddress2, SipConstants.DefaultSipPort);
        public static string MyUserName = "Hannes-HP";
        public static SipUri AliceUri = new SipAddressFactory().CreateUri("alice", "atlanta.com");
        public static SipUri BobUri = new SipAddressFactory().CreateUri("bob", "biloxi.com");
        public static SipUri AliceContactUri = new SipAddressFactory().CreateUri("", "u1.atlanta.com");
        public static SipUri BobContactUri = new SipAddressFactory().CreateUri("", "u2.biloxi.com");
        public static SipUri EndPoint1Uri = new SipAddressFactory().CreateUri("", "192.168.0.1");
        public static SipUri EndPoint2Uri = new SipAddressFactory().CreateUri("", "192.168.0.2");
        public static SipUri BobProxyUri;
        public static SipUri AliceProxyUri;

        static TestConstants()
        {
            BobProxyUri = new SipAddressFactory().CreateUri("", "p2.biloxi.com");
            BobProxyUri.IsLooseRouting = true;
            AliceProxyUri = new SipAddressFactory().CreateUri("", "p1.atlanta.com");
            AliceProxyUri.IsLooseRouting = true;
        }
    }
}
