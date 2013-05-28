using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Hallo.Parsers;
using Hallo.Sip;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class SipUriParserTest
    {
        [Test]
        public void Parse()
        { 
            String[] requestLines = {
                "REGISTER sip:192.168.0.68 SIP/2.0",
                "REGISTER sip:company.com SIP/2.0",
                "INVITE sip:3660@166.35.231.140 SIP/2.0",
                "INVITE sip:user@company.com SIP/2.0" };

            for (int i = 0; i < requestLines.Length; i++ ) {
                SipRequestLineParser rlp =
                  new SipRequestLineParser();
                SipRequestLine rl = rlp.Parse(requestLines[i]);
                Assert.AreEqual(requestLines[i], rl.FormatToString());
            }
        }
    }
}
