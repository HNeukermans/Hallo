using System;
using Hallo.Sdp.Parsers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdp.ParserTests
{
    public class SdpOriginParserTests
    {
        [Test]
        public void Parse_ValidFormats_ExpectNotToFail()
        {
            String[] strings = 
            {
                "jdoe 2890844526 2890842807 IN IP4 10.47.16.5",
            };
                
            foreach (string s in strings)
            {
                var h = new SdpOriginParser().Parse(s);
            }
        }
    }
}