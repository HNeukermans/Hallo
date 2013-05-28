using System;
using Hallo.Parsers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    public class SipUserAgentHeaderTests
    {
        [Test]
        public void Parse_ValidFormats_ExpectNotToFail()
        {
            String[] strings = 
                {
                    " Grandstream BT200 1.2.2.19",
                };


            foreach (string s in strings)
            {
                var h = new SipUserAgentHeaderParser().Parse(s);
            }
        }

    }
}