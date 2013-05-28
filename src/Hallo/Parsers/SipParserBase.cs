using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip;
using Hallo.Util;
using NLog;

namespace Hallo.Parsers
{
   public abstract class SipParser<T> : ISipParser<T>
   {
       private readonly Logger _logger = NLog.LogManager.GetLogger(string.Format("Hallo.Parsers.SipParser<{0}>", typeof(T).Name));

        protected SipParser()
        {
           if(_logger.IsTraceEnabled)
               _logger.Trace("Constructor called.");
        }

        public T Parse(string text)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug("Parse called. text:'{0}' ...", text);

            return Parse(new StringReader(text));
        }

        public abstract T Parse(StringReader r);
       
    
        //TODO: does this rely belong in the base class.
        protected void IfNullThrowParseExceptionInvalidFormat(string word)
        {
            if (word == null)
            {
                throw new SipParseException(ExceptionMessage.InvalidFormat);
            }
        }

        protected void IfNullOrEmptyThrowParseExceptionInvalidFormat(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new SipParseException(ExceptionMessage.InvalidFormat);
            }
        }

        protected void IfFalseThrowParseException(bool condition, string message)
        {
            if (!condition)
            {
                throw new SipParseException(message);
            }
        }

        protected void TryActionElseThrowParseExceptionInvalidFormat(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                throw new SipParseException(ExceptionMessage.InvalidFormat, e);
            }
        }
    }
}
