using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Hallo.Parsers;

namespace Hallo.UnitTest.Sip
{
   
    public class SipMessageParserTest
    {
        private string messageContent;

        [Test]
        public void Parse_7CharString()
        {
            String message = "1234567\r\n\r\n";


            byte[] bytes = Encoding.UTF8.GetBytes(message);
            
            var p = new SipMessageParser();
            var result = p.ParseSipMessage(bytes, messageContent);
        }


        [Test]
        public void Parse_ValidFormat_ExpectEqualFormatAfterParsing()
        {
            String message = "SIP/2.0 200 OK\r\n"
                        + "To: \"The Little Blister\" <sip:LittleGuy@there.com>;tag=469bc066\r\n"
                        + "From: \"The Master Blaster\" <sip:BigGuy@here.com>;tag=11\r\n"
                        + "Via: SIP/2.0/UDP 139.10.134.246:5060;branch=z9hG4bK8b0a86f6_1030c7d18e0_17;received=139.10.134.246\r\n"
                        + "Call-ID: 1030c7d18ae_a97b0b_b@8b0a86f6\r\n"
                        + "CSeq: 1 SUBSCRIBE\r\n"
                        + "Contact: <sip:172.16.11.162:5070>\r\n"
                        + "Content-Length: 0\r\n\r\nBody";

            byte[] bytes = Encoding.UTF8.GetBytes(message);
                        
                var p =
                  new SipMessageParser();
                var result = p.ParseSipMessage(bytes, messageContent);
        }

        [Test]
        public void Parse_ValidFormat2ViaHeaders_ExpectEqualFormatAfterParsing()
        {
            String message = "SIP/2.0 200 OK\r\n"
                        + "Via: SIP/2.0/UDP 139.10.134.246:5060;branch=z9hG4bK8b0a86f6_1030c7d18e0_17;received=139.10.134.246\r\n"
                        + "Via: SIP/2.0/UDP 140.10.134.246:5060;branch=z9hG4bK8b0a86f6_1030c7d18e0_17;received=139.10.134.246\r\n"
                        + "\r\n\r\n";

            byte[] bytes = Encoding.UTF8.GetBytes(message);

            var p =
              new SipMessageParser();
            var result = p.ParseSipMessage(bytes, messageContent);
            var viaHeadersFormatString = result.Vias.FormatToString();
        }
    }
}
