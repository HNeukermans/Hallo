using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Util
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ExcludeFromEquality : Attribute
    {
    }
}
