using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip.Headers;
using Hallo.Util;

namespace Hallo.Sip
{
    public class Datagram
    {
        private readonly IPEndPoint _remoteEP = null;
        private readonly byte[] _data = null;
        private readonly IPEndPoint _localEndPoint;

        public Datagram(byte[] data, IPEndPoint localEp, IPEndPoint remoteEp)
        {
            Check.Require(data, "data");
            Check.Require(localEp, "localEp");
            Check.Require(remoteEp, "remoteEp");

            _data = data;
            _localEndPoint = localEp;
            _remoteEP = remoteEp;
        }

        public IPEndPoint RemoteEndPoint
        {
            get { return _remoteEP; }
        }

        public IPEndPoint LocalEndPoint
        {
            get { return _localEndPoint; }
        }

        public byte[] DataBytes
        {
            get { return _data; }
        }
    }
}
