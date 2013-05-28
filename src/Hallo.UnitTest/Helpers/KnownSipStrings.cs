using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.UnitTest.Helpers
{
    public class KnownSipStrings
    {
        public static string RegisterString =
            "";

        public static string InviteString =
            "INVITE sip:joe@blah.com SIP/3.0\r\n" +
            "To: sip:joe@company.com\r\n" +
            "From: sip:caller@university.edu ;tag=1234\r\n" +
            "Call-ID: 0ha0isnda977644900765@10.0.0.1\r\n" +
            "CSeq: 9 INVITE\r\n" +
            "Via: SIP/2.0/UDP 135.180.130.133\r\n" +
            "Content-Type: application/sdp\r\n" +
            "\r\n" +
            "v=0\r\n" +
            "o=mhandley 29739 7272939 IN IP4 126.5.4.3\r\n" +
            "c=IN IP4 135.180.130.88\r\n" +
            "m=video 3227 RTP/AVP 31\r\n" +
            "m=audio 4921 RTP/AVP 12\r\n" +
            "a=rtpmap:31 LPC\r\n";
    }
}
