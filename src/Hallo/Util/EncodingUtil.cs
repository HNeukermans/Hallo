using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Util
{
    public class EncodingUtil
    {
        public static byte[] GetUtf8Bytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
    }
}
