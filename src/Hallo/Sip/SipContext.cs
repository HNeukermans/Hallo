using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Stack;
using NLog;
using Hallo.Util;

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

    public class SipContext
    {
        internal SipContext()
        {
            
        }

        //public SipContext(Datagram datagram, SipParserContext parserContext)
        //{
        //    Check.Require(datagram, "datagram");
        //    Check.Require(parserContext, "parserContext");

        //    _datagram = datagram;
        //    _parserContext = parserContext;
        //}

        public IPEndPoint RemoteEndPoint { get; set; }
    

        public IPEndPoint LocalEndPoint  { get; set; }
        
        //public void Start()
        //{
        //    _logger.Trace("Calling Start...");

        //    _parserContext.ParseCompleted += ParserContext_ParseCompleted;

        //    try
        //    {
        //        _logger.Trace("Calling Parse on parser context...");
        //        _parserContext.Parse(_datagram.DataBytes);
        //    }
        //    catch (SipParseException pe)
        //    {
        //        _logger.Error(pe);
        //    }
        //}

        //void ParserContext_ParseCompleted(object sender, ParseCompletedEventArgs e)
        //{
        //    _logger.Trace("Parse on parser context completed.");

        //    Request = e.Message as SipRequest;
        //    Response = e.Message as SipResponse;

        //    if(Request != null)
        //    _logger.Debug("Request Received '" + Request.RequestLine.Method + " " + Request.RequestLine.Uri.FormatToString() + "' from " +
        //                  _datagram.RemoteEndPoint);

        //    if (Response != null)
        //        _logger.Debug("Response Received '" + Response.StatusLine.StatusCode + " " + Response.StatusLine.ReasonPhrase + "' from " +
        //                      _datagram.RemoteEndPoint);

        //    //validate
        //    MessageReceived(this, new MessageReceivedEventArgs(e.Message));
        //}

        public SipRequest Request { get; set; }
        public SipResponse Response { get; set; }


        //public event EventHandler<MessageReceivedEventArgs> MessageReceived = delegate { };

        public bool IsHandled { get; set; }
        
        public bool IsSent { get; set; }

        internal void Update(SipRequestEvent requestEvent)
        {
            Response = requestEvent.Response;
            IsHandled = requestEvent.IsHandled;
            IsSent = requestEvent.IsSent;
        }

        internal void Update(SipResponseEvent responseEvent)
        {
            
        }
    }
}
