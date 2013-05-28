using Hallo.Sip;
using Hallo.UnitTest.Helpers;

namespace Hallo.UnitTest.Builders
{
    public class SipUriBuilder : ObjectBuilder<SipUri>
    {
        private string _user;
        private string _host;
        private int _port;
        private bool _lr;
        private string _transport;


        public SipUriBuilder()
        {
            _user = TestConstants.MyUserName;
            _host = TestConstants.IpAddress1.ToString();
            _port = SipConstants.DefaultSipPort;
        }

        public SipUriBuilder WithUser(string value)
        {
            _user = value;
            return this;
        }

        public SipUriBuilder WithHost(string value)
        {
            _host = value;
            return this;
        }

        public SipUriBuilder WithPort(int value)
        {
            _port = value;
            return this;
        }

        public SipUriBuilder WithLoooseRouting(bool value)
        {
            _lr = value;
            return this;
        }

        public SipUriBuilder WithTransport(string transport)
        {
            _transport = transport;
            return this;
        }


        public override SipUri Build()
        {
            SipUri item = new SipUri();
            item.User = _user;
            item.Host = _host;
            item.Port = _port;
            item.IsLooseRouting = _lr;
            item.Transport = _transport;
            return item;
        }
    }
}