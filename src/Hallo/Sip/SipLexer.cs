using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip.Headers;
using Hallo.Util;
using NLog;

namespace Hallo.Sip
{
    internal class SipLexer
    {
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private ParserMethod _parserMethod;
        private readonly BufferReader _reader = new BufferReader();
        private int _bodyBytesLeft;
        private string _headerName;
        private string _headerValue;
        private Action<string> _onFirstLine;
        private Action<string, string> _onHeader;
        private Action<byte[]> _onBody;
        private byte[] _buffer;

        internal SipLexer()
        {
        }

        internal void Lex(byte[] bytes, Action<string> onFirstLine, Action<string, string> onHeader, Action<byte[]> onBody)
        {
            _onBody = onBody;
            _onHeader = onHeader;
            _onFirstLine = onFirstLine;
            _parserMethod = ParseFirstLine;

            _buffer = bytes;

            _logger.Debug("Lexing " + _buffer.Length + " bytes.");
            _reader.Assign(_buffer, 0, _buffer.Length);

            while (_parserMethod())
            {
                _logger.Trace("Switched lexer method to " + _parserMethod.Method.Name + " at index " + _reader.Index);
            }
        }

        private bool ParseFirstLine()
        {
            _reader.Consume('\r', '\n');

            if (!_reader.Contains('\n'))
                throw new ParseException("Invalid firstline.");
            
            var firstLine = _reader.ReadFoldedLine();
            _onFirstLine(firstLine);

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
                    _onBody(null);
                    _parserMethod = ParseFirstLine;
                    return false;
                }

                _parserMethod = GetBody;
                return true;
            }

            _headerName = _reader.ReadUntil(':');
            if (_headerName == null)
                throw new ParseException(ExceptionMessage.InvalidHeaderName);

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
                throw new ParseException(ExceptionMessage.InvalidFormat);

            _headerValue += value;

            _headerValue = ProcessSingleLine(_headerValue);

            if (System.String.Compare(_headerName, SipHeaderNames.ContentLength, System.StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!int.TryParse(value, out _bodyBytesLeft))
                    throw new ParseException("Content length is not a number.");
            }

            _onHeader(_headerName, _headerValue);

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
                _onHeader(_headerName, headerString);
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
                _onBody(_buffer.ToList().Skip(_reader.Index).Take(_bodyBytesLeft).ToArray());
                return false;
            }

            throw new ParseException(ExceptionMessage.InvalidBodyLenght);
        }

        /// <summary>
        /// Used to be able to quickly swap parser method.
        /// </summary>
        /// <returns></returns>
        private delegate bool ParserMethod();
    }
}
