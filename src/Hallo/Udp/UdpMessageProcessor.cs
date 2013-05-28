using System;
using System.Net;
using Amib.Threading;
using Hallo.Parsers;
using Hallo.Sip.Util;
using Hallo.Udp;
using Hallo.Util;

namespace Hallo.Sip.Stack
{
    /// <summary>
    /// a factory for messagechannels
    /// </summary>
    //public class UdpMessageProcessor : IUdpMessageProcessor
    //{
    //    private readonly SipStack _stack;
    //    private readonly ISipTransportMessageListener _listener;
    //    private readonly LogUtil _logger;

    //    public UdpMessageProcessor(SipStack stack, ISipTransportMessageListener listener, LogUtil logger)
    //    {
    //        _stack = stack;
    //        _listener = listener;
    //        _logger = logger;
    //    }

    //    public void ProcessMessage(Udp.UdpPacket packet, UdpServer server)
    //    {
    //        var channel = new UdpMessageChannel(_stack, new SipValidator(), new SipMessageParser());
    //        var result = channel.Read(packet, server);
    //        UdpMessageChannelResultHandler handler = new UdpMessageChannelResultHandler();
    //        Handle(result);
    //    }

    //    internal void Handle(UdpMessageChannelReadResult result)
    //    {
    //        //if (result is SipMessageReadResult)
    //        //{
    //        //    var mr = (SipMessageReadResult)result;
    //        //    //mr
    //        //}
    //        //else if (result is ParseExceptionReadResult)
    //        //{
    //        //    var per = (ParseExceptionReadResult)result;
    //        //    _logger.Warn(per.ParseException);
    //        //}
    //        //else if (result is InvalidMessageReadResult)
    //        //{
    //        //    var r = (InvalidMessageReadResult)result;
    //        //    _logger.Info(null, "Invalid message received.");
    //        //    SendResponse(r.BadRequestResponse);
    //        //}
    //    }

    //    public void SendResponse(SipResponse response)
    //    {
    //        //IPEndPoint remoteEp = GetRemoteEndPointInformation(response);

    //        //var bytes = SipUtil.ToUtf8Bytes(response.FormatToString());

    //        //try
    //        //{
    //        //    _server.SendBytes(bytes, remoteEp);
    //        //}
    //        //catch (Exception e)
    //        //{
    //        //    throw;
    //        //}
    //    }

    //}
}