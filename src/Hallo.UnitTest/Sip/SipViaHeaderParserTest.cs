using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.UnitTest.Builders;
using NUnit.Framework;
using Hallo.Parsers;
using Hallo.Sip.Headers;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class SipViaHeaderParserTest
    {
        [Test]
        public void Parse_ValidFormat_ExpectEqualFormatAfterParsing()
        {
            String[] strings = 
            {
                "Via: SIP/2.0/UDP 192.168.0.5:5061;branch=z9hG4bK11a797d2df9210b5;received=192.168.0.5",
            };

            for (int i = 0; i < strings.Length; i++)
            {
                var vhp =
                   new SipHeaderNameParser(SipHeaderNames.Via, new SipViaHeaderParser());
                var result = (SipViaHeader)vhp.Parse(strings[i]);
                Assert.AreEqual(strings[i], result.ToString());
            }
        }

        [Test]
        [ExpectedException(typeof(SipParseException))]
        public void Parse_SentByIsNotIpAddress_ExpectThrowException()
        {
            String[] strings = 
            {
                "Via: SIP/2.0/UDP localhost:5060;branch=z9hG4bK-870e4c52b7694dec80f75c42549a342c;rport",
            };

            for (int i = 0; i < strings.Length; i++)
            {
                var vhp =
                  new SipHeaderNameParser(SipHeaderNames.Via, new SipViaHeaderParser());
                var result = (SipViaHeader)vhp.Parse(strings[i]);
                Assert.AreEqual(strings[i], result.ToString());
            }
        }

        [Test]
        public void Parse_SentByDoesNotHavePortSpecified_ExpectToUseDefaultPort5060()
        {
            String[] strings = 
            {
                "Via: SIP/2.0/UDP 1.2.3.4;branch=z9hG4bK-870e4c52b7694dec80f75c42549a342c;rport",
            };

            for (int i = 0; i < strings.Length; i++)
            {
                var vhp =
                  new SipHeaderNameParser(SipHeaderNames.Via, new SipViaHeaderParser());
                var result = (SipViaHeader) vhp.Parse(strings[i]);
                Assert.AreEqual(result.SentBy.Port, 5060);
            }
        }
    }
}
