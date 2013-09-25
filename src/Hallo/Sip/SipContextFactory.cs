using Hallo.Sip.Stack;
using Hallo.Util;
using NLog;

namespace Hallo.Sip
{
    public class SipContextFactory
    {
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public SipContextFactory(SipMessageFactory messageFactory, SipHeaderFactory headerFactory)
        {
            _logger.Trace("Constructor called.");
            _messageFactory = messageFactory;
            _headerFactory = headerFactory;
        }

        private SipMessageFactory _messageFactory;
        private SipHeaderFactory _headerFactory;

        public SipContext CreateContext(Datagram datagram)
        {
            Check.Require(datagram, "datagram");
            
            _logger.Trace("Calling CreateContext...");
            
            var parserContext = new SipParserContext(_messageFactory, _headerFactory);
            SipContext context = null;
            parserContext.ParseCompleted += (s, e) => context = ParserContext_ParseCompleted(e, datagram);
            
            _logger.Trace("Calling Parse on parser context...");
            parserContext.Parse(datagram.DataBytes);
            return context;
        }

        SipContext ParserContext_ParseCompleted(ParseCompletedEventArgs e, Datagram datagram)
        {
            var c = new SipContext();

            _logger.Trace("Parse on parser context completed.");

            c.Request = e.Message as SipRequest;
            c.Response = e.Message as SipResponse;
            c.RemoteEndPoint = datagram.RemoteEndPoint;
            c.LocalEndPoint = datagram.LocalEndPoint;

            if (c.Request != null)
                _logger.Debug("Request Received '" + c.Request.RequestLine.Method + " " + c.Request.RequestLine.Uri.FormatToString() + "' from " +
                              datagram.RemoteEndPoint);

            if (c.Response != null)
                _logger.Debug("Response Received '" + c.Response.StatusLine.StatusCode + " " + c.Response.StatusLine.ReasonPhrase + "' from " +
                              datagram.RemoteEndPoint);
            return c;
        }
    }
}