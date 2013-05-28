using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Util;
using NLog;
using Hallo.Util;
using Hallo;

namespace Hallo.Parsers
{

    /// <summary>
    /// parses a raw SIP-byte stream
    /// </summary>
    public class SipParser
    {
        private readonly ISipParserListener _listener;
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private ParserMethod _parserMethod;
        private byte[] _buffer;
        private readonly BufferReader _reader = new BufferReader();
        private int _bodyBytesLeft;
        private string _headerName;
        private string _headerValue;

        public SipParser(ISipParserListener listener)
        {
            _listener = listener;
            _parserMethod = ParseFirstLine;
        }

        public int Parse(byte[] buffer, int offset, int count)
        {
            _logger.Debug("Parsing " + count + " bytes.");
            _logger.Trace("Parsing " + count + " bytes from offset " + offset + " using " + _parserMethod.Method.Name);
            _buffer = buffer;
            _reader.Assign(buffer, offset, count);
            
            while (_parserMethod())
            {
                _logger.Trace("Switched parser method to " + _parserMethod.Method.Name + " at index " + _reader.Index);
            }
            return _reader.Index;
        }

        public bool ParseFirstLine()
        {
            _reader.Consume('\r', '\n');

            if (!_reader.Contains('\n'))
                throw new SipParseException("Invalid firstline.");

            var firstLine = _reader.ReadFoldedLine(); 
            
            if (firstLine.EndsWith(SipConstants.SipTwoZeroString))
            {
                SipRequestLine requestLine = new SipRequestLineParser().Parse(firstLine);
                _listener.OnRequest(requestLine);
            }
            else if (firstLine.StartsWith(SipConstants.SipTwoZeroString))
            {
                var message = new SipResponse();
                var statusLine = new SipStatusLineParser().Parse(firstLine);
                _listener.OnResponse(statusLine);
            }
            else
            {
                throw new SipParseException(ExceptionMessage.InvalidFirstLineFormat);
            }

            _parserMethod = GetHeaderName;
            
            return true;
        }

        private bool GetHeaderName()
        {
            // empty line. body is begining.
            if (_reader.Current == '\r' && _reader.Peek == '\n')
            {
                // Eat the line break
                _reader.Consume('\r', '\n');

                // Don't have a body?
                if (_bodyBytesLeft == 0)
                {
                     _listener.OnComplete();
                    _parserMethod = ParseFirstLine;
                    return false;
                }
                else
                {
                    _parserMethod = GetBody;
                    return true;
                }
            }

            _headerName = _reader.ReadUntil(':');
            if (_headerName == null)
                throw new SipParseException(ExceptionMessage.InvalidHeaderName);

            _reader.Consume(); // eat colon
            _parserMethod = GetHeaderValue;
            return true;
        }


        private bool GetHeaderValue()
        {
            // remove white spaces.
            _reader.ConsumeWhiteSpaces();
            
            string value = _reader.ReadFoldedLine();
            if (value == null)
                throw new SipParseException(ExceptionMessage.InvalidFormat);

            _headerValue += value;
            
            _headerValue = ProcessSingleLine(_headerValue);
            
            if (System.String.Compare(_headerName, SipHeaderNames.ContentLength, System.StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!int.TryParse(value, out _bodyBytesLeft))
                    throw new SipParseException("Content length is not a number.");
            }

            _listener.OnHeader(_headerName, _headerValue);

            _headerName = null;
            _headerValue = string.Empty;
            _parserMethod = GetHeaderName;
            return true;
        }

        /// <summary>
        /// parses each comma separated header value in the line, returns the last header value
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string ProcessSingleLine(string line)
        {
            List<string> headerStrings = new List<string>();

            StringReader r = new StringReader(line);

            while (r.Available > 0)
            {
                headerStrings.Add(r.QuotedReadToDelimiter(','));
            }

            var lastHeaderString = headerStrings.Last();

            headerStrings.Remove(lastHeaderString);

            foreach (var headerString in headerStrings)
            {
                _listener.OnHeader(_headerName, headerString);
            }

            return lastHeaderString;
        }

        private bool GetBody()
        {
            if (_reader.RemainingLength == 0)
                return false;

            // Got enough bytes to complete body.
            if (_reader.RemainingLength >= _bodyBytesLeft)
            {
                _listener.OnBodyBytes(_buffer.ToList().Skip(_reader.Index).Take(_bodyBytesLeft).ToArray());
                _reader.Index += _bodyBytesLeft;
                _bodyBytesLeft = 0;
                _listener.OnComplete();
                return false;
            }

            throw new SipParseException(ExceptionMessage.InvalidBodyLenght);
        }

        /// <summary>
        /// Used to be able to quickly swap parser method.
        /// </summary>
        /// <returns></returns>
        private delegate bool ParserMethod();
    }

    
}
