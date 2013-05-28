using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.UnitTest.Builders
{
    public class SipMessageBody
    {
        public List<Byte> Bytes { get; set; }
        public string ContentType { get; set; }
        public int ContentLenght
        {
            get { return Bytes.Count; }
        }
    }
}
