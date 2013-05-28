using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amib.Threading;
using Hallo.Sip;
using Hallo.Parsers;
using System.Net;
using Hallo.Sip.Headers;
using Hallo.Sip.Util;
using Hallo.Util;

namespace Hallo.Udp
{
    /// <summary>
    /// represents the rfc 3261 transport layer. Because the transport layer has state,
    /// I choose to model the transport layer as a Channel.
    /// </summary>
    public class UdpMessageChannel : IUdpMessageChannel
    {
        //private UdpPacket _udpPacket;
        private readonly IUdpServer _server;
        private SipStack _stack;
        private readonly ISipValidator _validator;
        private ISipMessageParser _parser;
        private readonly ISipTransportMessageListener _listener;
        private readonly LogUtil _logger;
        private IPEndPoint _localEndPoint;
        private IPEndPoint _remoteEndPoint;
        private int _thisPort;

        public UdpMessageChannel(SipStack stack, ISipValidator validator, ISipMessageParser parser)
        {
            _stack = stack;
            //_server = server;
            _validator = validator;
            _parser = parser;// _stack.GetMessageParserFactory().CreateMessageParser();
            //_listener = listener;
            //_logger = logger;
        }

        public UdpMessageChannelReadResult Read(UdpPacket packet, IUdpServer server)
        {
            _localEndPoint = server.ListeningPoint;
            _remoteEndPoint = packet.RemoteEndPoint;

            SipMessage message = null;
            string messageContent = "";
            try
            {
                message = _parser.ParseSipMessage(packet.DataBytes, messageContent);
            }
            catch (SipParseException pe)
            {
                var result = new ParseExceptionReadResult();
                result.ParseException = pe;
                result.UdpPacket = packet;
                return result;
                LogUtil.Error(pe, "MessageContent could not be parsed: '{0}'", messageContent);
                //_stack
                //return;
            }

            var vr = _validator.ValidateMessage(message);

            if(!vr.IsValid)
            {
                var result = new InvalidMessageReadResult();
                result.UdpPacket = packet;
                result.Message = message;

                if(message is SipRequest)
                {
                    var vrRequest = vr as ValidateRequestResult;
                    var request = message as SipRequest;

                    if (vrRequest.HasInvalidSCeqMethod)
                    {
                        result.BadRequestResponse = request.CreateResponse(SipResponseCodes.x400_Bad_Request_InvalidCseqMethod);
                    }
                    if (vrRequest.HasUnSupportedSipVersion)
                    {
                        result.BadRequestResponse = request.CreateResponse(SipResponseCodes.x400_Bad_Request_UnsupportedSipVersion);
                    }
                    if (vrRequest.HasRequiredHeadersMissing)
                    {
                        result.BadRequestResponse = request.CreateResponse(string.Format(SipResponseCodes.x400_Bad_Request_HasMissingHeaderField_FormatString, vr.MissingRequiredHeader));
                    }
                }

                return result;
            }
            else
            {
                var result = new SipMessageReadResult();
                result.Message = message;
                result.UdpPacket = packet;

                var request = message as SipRequest;
                if (request != null)
                {
                    EnsureRemoteEndPointInformationIsStoredInRequest(request);
                    result.Message = request;
                }
                return result;
            }
        }

        public void SendResponse(SipResponse response)
        {
            IPEndPoint remoteEp = GetRemoteEndPointInformation(response);

            byte[] bytes = null;// SipUtil.ToUtf8Bytes(SipFormatter.FormatMessage(response.FormatEnvelopeToString()));
            
            try
            {
                _server.SendBytes(bytes, remoteEp);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void SendRequest(SipRequest request)
        {
            IPEndPoint remoteEp = GetRemoteEndPointInformation(request);

            //var bytes = SipUtil.ToUtf8Bytes(request.FormatEnvelopeToString());

            //try
            //{
            //    _server.SendBytes(bytes, remoteEp);
            //}
            //catch (Exception e)
            //{
            //    throw;
            //}
        }

        private IPEndPoint GetRemoteEndPointInformation(SipMessage response)
        {
            var topMostVia = response.Vias.GetTopMost();

            CodeContracts.RequiresNotNull(topMostVia, "Vias.GetTopMost()");
            CodeContracts.RequiresNotNull(topMostVia.SentBy, "via.SentBy");
            CodeContracts.RequiresIsTrue(topMostVia.Transport == SipConstants.Udp, "Transport must be Udp.");
           
            IPAddress address = topMostVia.Received ?? topMostVia.SentBy.Address;
            int port =  topMostVia.Rport != -1 ? topMostVia.Rport : topMostVia.SentBy.Port;
            
            return new IPEndPoint(address, port);
        }

        internal void EnsureRemoteEndPointInformationIsStoredInRequest(SipRequest request)
        {
            var topMostVia = request.Vias.GetTopMost();
            if(!topMostVia.SentBy.Address.Equals(_remoteEndPoint.Address))
            {
                topMostVia.Received = _remoteEndPoint.Address;
            }

            //rfc3581
            if(topMostVia.UseRport)
            {
                topMostVia.Rport = _remoteEndPoint.Port;
            }
        }

        //private void SendMessage(SipResponse response)
        //{
        //    _udpPacket.Send(response.FormatToString(), int, )
        //}

        private void SendMessage(SipResponse versionNotSupported, IPEndPoint remoteEndPoint, string protocol)
        {
            throw new NotImplementedException();
        }

       
    }

    public class UdpMessageChannelReadResult
    {
        public UdpPacket UdpPacket { get; set; }
        //public SipResponse SipResponse { get; set; }
        //public SipRequest SipRequest { get; set; }
        ////public bool HasParseException
        //{
        //    get { return ParseException != null; }
        //}
        //public bool HasValidationErrors
        //{
        //    get { return ValidationErrorResponse != null; }
        //}
        //public SipResponse ValidationErrorResponse { get; set; }
        //public UdpMessageChannel Channel { get; set; }
        ////public SipParseException ParseException { get; set; }
    }

    public class SipMessageReadResult : UdpMessageChannelReadResult
    {
        public SipMessage Message { get; set; }
    }

    public class InvalidMessageReadResult : UdpMessageChannelReadResult
    {
        public SipMessage Message { get; set; }
        public SipResponse BadRequestResponse { get; set; }
    }

    public class ParseExceptionReadResult : UdpMessageChannelReadResult
    {
        public SipParseException ParseException { get; set; }
    }


    public class SipApplication
    {
        private ISipMessageParser _parser;
        private readonly SipRequestPipeLine _requestPipeLine;

        public SipApplication(ISipMessageParser parser, SipRequestPipeLine requestPipeLine)
        {
            _parser = parser;
            _requestPipeLine = requestPipeLine;
        }

        public void ProcessIncomingPacket(UdpPacket packet)
        {
            SipMessage message = null;
            string messageContent = "";
            try
            {
                message = _parser.ParseSipMessage(packet.DataBytes, messageContent);
            }
            catch (SipParseException pe)
            {
                LogUtil.Error(pe, "Message could not be parsed: '{0}'", messageContent);
                //_stack
                //return;
            }

            //create SipContext
            var context = CreateContext(packet, message);

            if(message is SipRequest)
            {
                _requestPipeLine.ProcessRequest(context);
            }
        }

        private SipContext CreateContext(UdpPacket packet, SipMessage message)
        {
            var context = new SipContext();
            context.LocalEndPoint = packet.LocalEndPoint;
            context.RemoteEndPoint = packet.RemoteEndPoint;
            context.Request = message as SipRequest;
            context.Response = message as SipResponse;
            return context;
        }
    }

    public class SipRequestPipeLine
    {
        public void ProcessRequest(SipContext context)
        {
            //validate request
            //new SipRequestValidatorModule().Execute(context);

            if(!context.HasResponse)
            {
                //continue processing
                EnsureRemoteEndPointInformationIsStoredInRequest(context);
            }

            SendResponse(context);
            //authenticate request
        }

        private void SendResponse(SipContext context)
        {
            IPEndPoint remoteEp = GetRemoteEndPointInformation(context.Response);

            //var bytes = SipUtil.ToUtf8Bytes(context.Response.FormatEnvelopeToString());

            //try
            //{
            //    //_server.SendBytes(bytes, remoteEp);
            //}
            //catch (Exception e)
            //{
            //    throw;
            //}
        }

        internal static void EnsureRemoteEndPointInformationIsStoredInRequest(SipContext context)
        {
            var topMostVia = context.Request.Vias.GetTopMost();
            if (!topMostVia.SentBy.Address.Equals(context.RemoteEndPoint.Address))
            {
                topMostVia.Received = context.RemoteEndPoint.Address;
            }

            //rfc3581
            if (topMostVia.UseRport)
            {
                topMostVia.Rport = context.RemoteEndPoint.Port;
            }
        }

        private IPEndPoint GetRemoteEndPointInformation(SipMessage response)
        {
            var topMostVia = response.Vias.GetTopMost();

            CodeContracts.RequiresNotNull(topMostVia, "Vias.GetTopMost()");
            CodeContracts.RequiresNotNull(topMostVia.SentBy, "via.SentBy");
            CodeContracts.RequiresIsTrue(topMostVia.Transport == SipConstants.Udp, "Transport must be Udp.");

            IPAddress address = topMostVia.Received ?? topMostVia.SentBy.Address;
            int port = topMostVia.Rport != -1 ? topMostVia.Rport : topMostVia.SentBy.Port;

            return new IPEndPoint(address, port);
        }
    }

    public class SipContext
    {
        public SipRequest Request { get; set; }
        public SipResponse Response { get; set; }
        public IPEndPoint LocalEndPoint { get; set; }
        public IPEndPoint RemoteEndPoint { get; set; }
        public bool HasResponse
        {
            get { return Response != null; }
        }
    }
}
