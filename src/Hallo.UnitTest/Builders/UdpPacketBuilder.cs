using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.UnitTest.Helpers;

namespace Hallo.UnitTest.Builders
{
    public class DatagramPacketBuilder : ObjectBuilder<Datagram>
    {
        private IPEndPoint _remoteEndPoint;
        private Byte[] _dataBytes;
        private IPEndPoint _localEndPoint;

        public DatagramPacketBuilder()
        {
            _localEndPoint = TestConstants.IpEndPoint1;
            _remoteEndPoint = TestConstants.IpEndPoint2;
            _dataBytes = new DataBytesBuilder().Build();
        }
        
        public DatagramPacketBuilder WithRemoteEp(IPEndPoint value)
        {
            _remoteEndPoint = value;
            return this;
        }

        public DatagramPacketBuilder WithDataBytes(Byte[] value)
        {
            _dataBytes = value;
            return this;
        }

        public override Datagram Build()
        {
            Datagram item = new Datagram(_dataBytes, _localEndPoint, _remoteEndPoint);

            return item;
        }
    }
}