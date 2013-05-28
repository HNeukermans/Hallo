using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Hallo.Udp
{
	public interface ISocket
	{
		void Close();
        bool Poll(int i, SelectMode selectMode);
        void Bind(IPEndPoint endpoint);
        int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP);
		int SendTo(byte[] buffer, IPEndPoint remoteEp);
	}
}
