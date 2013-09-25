using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Hallo.Parsers;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class SipStatusLineParserTest
    {
        [Test]
        public void Parse_ValidFormats_ExpectNotToFail()
        {
            String[] strings = 
            {
                 "SIP/2.0 407 Proxy Authentication Required",
                 "SIP/2.0 1 2"
            };
            
            foreach (string s in strings)
            {
                var h = new SipStatusLineParser().Parse(s);
            }
        }

        [Test]
        public void Parse_InvalidFormats_ExpectToThrowParseException()
        {
            String[] strings = 
            {
                "SIP/2.0  407  Proxy Authentication Required ",
                " SIP/2.0 407 Proxy Authentication Required",
                "SIP/2.0 407",
                ",SIP/2.0 407 Proxy Authentication Required",
                "407 Proxy Authentication Required SIP/2.0",
                "Proxy Authentication Required 407  SIP/2.0",
                "SIP/3.0 aaa Proxy Authentication Required",
                " ",
                " , , "
            };

            int exceptions = 0;
            foreach (string s in strings)
            {
                try
                {
                    new SipStatusLineParser().Parse(s);
                }
                catch (ParseException e)
                {
                    exceptions++;
                }
            }

            exceptions.Should().Be(strings.Length);
        }
        
    }
}
