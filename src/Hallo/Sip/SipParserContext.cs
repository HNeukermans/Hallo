using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip.Stack;
using NLog;

namespace Hallo.Sip
{
    public class SipParserContext : ISipParserListener
    {
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public event EventHandler<ParseCompletedEventArgs> ParseCompleted = delegate { };
        private readonly SipHeaderFactory _headerFactory;
        private readonly SipMessageFactory _messageFactory;
        private SipMessage _message;
        private SipParser _sipParser;
       
        public SipParserContext(SipMessageFactory messageFactory, SipHeaderFactory headerFactory)
        {
            _logger.Trace("Constructor called.");
            _messageFactory = messageFactory;
            _headerFactory = headerFactory;
            _sipParser = new SipParser(this);
        }

        public void OnRequest(SipRequestLine requestLine)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug("OnRequest called. Method:'{0}', Uri:'{1}'", requestLine.Method, requestLine.Uri.FormatToString());
    
            _message = _messageFactory.CreateRequest(requestLine);
        }

        public void OnResponse(SipStatusLine statusLine)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug("OnResponse called.  StatusCode:'{0}', ReasonPhrase:'{1}'", statusLine.StatusCode, statusLine.ReasonPhrase);
    
            _message = _messageFactory.CreateResponse(statusLine);
         }

        public void OnComplete()
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug("OnComplete called. Raising ParseCompleted event.");
        
            ParseCompleted(this, new ParseCompletedEventArgs { Message = _message });
        }

        public void OnHeader(string name, string value)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug("OnHeader called.  name:'{0}', value:'{1}'", name, value);
     
            var header = _headerFactory.CreateHeader(name, value);
            _message.SetHeader(header);
       }

        public void OnBodyBytes(byte[] body)
        {
            _message.Body = body;

            if (_logger.IsDebugEnabled)
                _logger.Debug("OnBodyBytes called. body(#bytes):{0} ", body == null ? 0 : body.Length);
        }

        public void Parse(byte[] buffer)
        {
            if (_logger.IsTraceEnabled)
                _logger.Trace("Start parsing {0} bytes ...", buffer.Length);

            _sipParser.Parse(buffer, 0, buffer.Length);

            if (_logger.IsTraceEnabled)
                _logger.Trace("bytes parsed.");
        }
    }
}
