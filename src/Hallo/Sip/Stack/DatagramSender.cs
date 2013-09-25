using System;
using System.Net;
using System.Net.Sockets;
using Hallo.Util;
using NLog;

namespace Hallo.Sip
{
    public class DatagramSender
    {
        private readonly IPEndPoint _fromEndPoint;
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private Socket _socket = null;
        private object _locker = new object();

        public DatagramSender(IPEndPoint fromEndPoint)
        {
            _fromEndPoint = fromEndPoint;
        }

        /// <summary>
        /// sends a message to the socket.
        /// </summary>
        /// <remarks>externalizing the socket would require to mock it.</remarks>
        /// <param name="bytes"></param>
        /// <param name="ipEndPoint"></param>
        public bool SendTo(byte[] bytes, IPEndPoint ipEndPoint)
        {
            Check.Require(bytes, "bytes");
            Check.Require(ipEndPoint, "ipEndPoint");

            _logger.Trace("Sending message from: '{0}' --> '{1}'", _fromEndPoint, ipEndPoint);

            lock (_locker)
            {
                try
                {
                    if (_socket == null)
                        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    _socket.SendTo(bytes, ipEndPoint);
                    return true;
                }
                catch (Exception ex)
                {
                    if (_logger.IsErrorEnabled) _logger.Error("Could not send message.", ex.Message);
                    _socket = null;
                    return false;
                    //throw;
                }
            }
        }
    }
}