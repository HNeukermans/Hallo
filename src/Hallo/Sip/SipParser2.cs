using Hallo.Parsers;
using Hallo.Sip.Stack;
using Hallo.Util;
using NLog;

namespace Hallo.Sip
{
    public class SipParser2
    {
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly SipMessageFactory _messageFactory;
        private readonly SipHeaderFactory _headerFactory;
        private SipMessage _message;

        public SipParser2(SipMessageFactory messageFactory, SipHeaderFactory headerFactory)
        {
            _logger.Trace("Constructor called.");
            _messageFactory = messageFactory;
            _headerFactory = headerFactory;
            _message = null;
        }
        
        private void OnBody(byte[] bytes)
        {
            _message.Body = bytes;

            if (_logger.IsDebugEnabled)
                _logger.Debug("OnBody called. body(#bytes):{0} ", bytes == null ? 0 : bytes.Length);
        }

        private void OnHeaderLine(string name, string value)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug("OnHeader called.  name:'{0}', value:'{1}'", name, value);

            var header = _headerFactory.CreateHeader(name, value);
            _message.SetHeader(header);
        }

        private void OnFirstLine(string firstLine)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug("OnFirstLine called. firstLine:'{0}'", firstLine);

            if (firstLine.EndsWith(SipConstants.SipTwoZeroString))
            {
                if (_logger.IsDebugEnabled)
                    _logger.Debug("Firstline is expected to be a SipRequestLine. Try parsing...");

                var requestLine = new SipRequestLineParser().Parse(firstLine);
                _message = _messageFactory.CreateRequest(requestLine);

                if (_logger.IsDebugEnabled)
                    _logger.Debug("RequestLine parsed. Continuing SipRequest parsing...");
            }
            else if (firstLine.StartsWith(SipConstants.SipTwoZeroString))
            {
                if (_logger.IsDebugEnabled)
                    _logger.Debug("Firstline is expected to be a SipStatusLine. Try parsing...");

                var statusLine = new SipStatusLineParser().Parse(firstLine);
                _message = _messageFactory.CreateResponse(statusLine);

                if (_logger.IsDebugEnabled)
                    _logger.Debug("SipStatusLine parsed. Continuing SipResponse parsing..");
            }
            else
            {
                if (_logger.IsDebugEnabled)
                    _logger.Debug("Firstline format could not be recognized.");

                throw new ParseException(ExceptionMessage.InvalidFirstLineFormat);
            }
        }

        public SipMessage Parse(Datagram datagram)
        {
            Check.Require(datagram, "datagram");

            _logger.Trace("Parsing datagram...");
            
            new SipLexer().Lex(datagram.DataBytes, OnFirstLine, OnHeaderLine, OnBody);

            _logger.Trace("Datagram parsed succesfully.");

            return _message;
        }
    }
}