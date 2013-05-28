using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip;
using NUnit.Framework;
using Hallo.Parsers;
using Hallo.Sip.Headers;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class SipToHeaderParserTest
    {
        [Test]
        public void Parse_ValidFormat_ExpectEqualFormatAfterParsing()
        {
            String[] strings = 
            {
                "To: <sip:100@192.168.0.1>",
                "To: <sip:100@192.168.0.1>;tag=1111"
            };

            for (int i = 0; i < strings.Length; i++)
            {
                var p = new SipToHeaderParser();
                var result = p.Parse(strings[i]);
                Assert.AreEqual(strings[i], SipFormatter.FormatHeader(result));
            }
        }

        [Test]
        [ExpectedException(typeof(SipParseException))]
        public void Parse_DoesNotStartWithLAQOUT_ExpectThrowException()
        {
            String[] strings = 
            {
                "To: sip:1.1.1.1:222>",
            };

            for (int i = 0; i < strings.Length; i++)
            {
                var p = new SipToHeaderParser();
                var result = p.Parse(strings[i]);
                Assert.AreEqual(strings[i], SipFormatter.FormatHeader(result));
            }
        }

        [Test]
        public void Parse_ToUriDoesNotHavePortSpecified_ExpectToUseDefaultPort5060()
        {
            String[] strings = 
            {
                "To: <sip:1.1.1.1>",
            };

            for (int i = 0; i < strings.Length; i++)
            {
                var p = new SipToHeaderParser();
                var result = p.Parse(strings[i]);
                Assert.AreEqual(strings[i], SipFormatter.FormatHeader(result));
            }
        }
    }
}
