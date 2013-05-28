using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Hallo.Sip;
using NUnit.Framework;
using Hallo.Parsers;
using Hallo.Sip.Headers;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class SipFromHeaderTests
    {
        [Test]
        public void Parse_ValidFormats_ExpectNotToFail()
        {
            String[] strings = 
            {
                " \"100\" <sip:100@192.168.0.1>;tag=182eb73f4c0f1e61",
                " <sip:100@192.168.0.1>;tag=182eb73f4c0f1e61;parameter1=value1",
                " <sip:100@192.168.0.1>;tag=182eb73f4c0f1e61  ",
                " <sip:100@192.168.0.1> "
            };


            foreach (string s in strings)
            {
                var h = new SipFromHeaderParser().Parse(s);
            }
        }

        [Test]
        public void Parse_InvalidFormats_ExpectToThrowParseException()
        {
            String[] strings = 
            {
                "<sip:100@192.168.0.1>;tag=182eb73f4c0f1e61,aaaa",
                //"<sip:100@192.168.0.1>;tag=182eb73f4c0f1e61, <sip:100@192.168.0.1>;tag=182eb73f4c0f1e61",
                // " \"100aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\" <sip:100@192.168.0.1>;tag=182eb73f4c0f1e61",
                //"sip:100@192.168.0.1",
                //", ",
                //" "
            };

            int exceptions = 0;
            foreach (string s in strings)
            {
                try
                {
                    new SipFromHeaderParser().Parse(s);
                }
                catch (SipParseException e)
                {
                    exceptions++;
                }
            }

            exceptions.Should().Be(strings.Length);
        }
    }
}
