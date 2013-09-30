using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.UnitTest.Stubs
{
    /// <summary>
    /// fakes a network. Sends a SipContext to a registered receiver by address. 
    /// </summary>
    public class FakeNetwork
    {
        public FakeNetwork()
        {
            _messageFactory = new SipMessageFactory();
            _headerFactory = new SipHeaderFactory();
        }

        Dictionary<IPEndPoint, Action<SipContext>> _receivers = new Dictionary<IPEndPoint, Action<SipContext>>();
        private SipHeaderFactory _headerFactory;
        private SipMessageFactory _messageFactory;

        public void AddReceiver(IPEndPoint receiverEndPoint, Action<SipContext> onReceive)
        {
            if(_receivers.ContainsKey(receiverEndPoint)) throw new Exception("Already a receiver. A receiver can only be added once.");

            _receivers.Add(receiverEndPoint, onReceive);
        }

        public void SendTo(byte[] bytes, IPEndPoint from,  IPEndPoint to)
        {
            if (!_receivers.ContainsKey(to)) throw new Exception(string.Format("Failed to deliver message to receiver. A receiver with endpoint:'{0}' could not be found.", to));

            var context = ConvertToContext(bytes, from, to);

            var pts = new ParameterizedThreadStart(Target);
            var t = new Thread(pts);
            t.Start(new { Context = context, To = to});
        }

       

        private void Target(object o)
        {
            var oDyn = o as dynamic;
            _receivers[oDyn.To].Invoke(oDyn.Context);
        }

        private SipContext ConvertToContext(byte[] bytes, IPEndPoint from,  IPEndPoint to)
        {
            Datagram datagram = new Datagram(bytes, from , to);
            var message = new SipParser2(_messageFactory, _headerFactory).Parse(datagram);

            return CreateSipContext(message, datagram);
        }

        private SipContext CreateSipContext(SipMessage message, Datagram datagram)
        {
            var request = message as SipRequest;
            var response = message as SipResponse;
            
            var c = new SipContext();
            
            c.Request = request;
            c.Response = response;
            c.RemoteEndPoint = datagram.RemoteEndPoint;
            c.LocalEndPoint = datagram.LocalEndPoint;

            return c;
        }
    }
}