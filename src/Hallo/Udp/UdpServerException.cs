using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Udp
{
    public class UdpServerException : Exception
    {
        public UdpServerException(Exception e) : base(e.Message, e) 
        {
        }
    }
}
