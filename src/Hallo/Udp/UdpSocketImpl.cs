using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Hallo.Udp;

namespace Hallo.Udp
{
	public class UdpSocketImpl : ISocket
	{
		private Socket _socket;

        public UdpSocketImpl()
		{
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		}
	
		public void Close()
		{
			_socket.Close();
		}
		
		public int SendTo(byte[] buffer, IPEndPoint remoteEp)
		{
			return _socket.SendTo(buffer, remoteEp);
		}

        public bool Poll(int microseconds, SelectMode selectMode)
        {
            return _socket.Poll(microseconds, selectMode);
        }

        public int ReceiveFrom(byte[] buffer, ref System.Net.EndPoint remoteEP)
        {
            return _socket.ReceiveFrom(buffer, ref remoteEP);
        }

        public void Bind(System.Net.IPEndPoint endpoint)
        {
            _socket.Bind(endpoint);
        }
    }
}
