using System.Collections.Generic;

namespace Hallo.Sdp.Parsers
{
    [ParserFor("m")]
    public class SdpMediaDescription
    {
        public int Port { get; set; }
        public string TransportProtocol { get; set; }
        public List<SdpAttribute> Attributes { get; set; }
    }
}