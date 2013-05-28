using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Parsers
{
    public interface IParseExceptionHandler
    {
        void HandleException(SipParseException exception);

    }
}
