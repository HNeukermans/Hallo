using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip
{
    interface IStringFormatable 
    {
        string FormatToString();
    }

    interface IBodyStringFormatable
    {
        string FormatBodyToString();
    }
}
