using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Util
{
    public class CodeContracts
    {
        public static void RequiresNotNull(object value, string name)
        {
            if(value == null) throw new ArgumentNullException(string.Format("{0} can not be null", name));
        }

        public static void RequiresIsTrue(bool condition, string message)
        {
            if (!condition) throw new ArgumentException(message);
        }
    }
}
