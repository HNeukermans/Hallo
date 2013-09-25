using System;
using FluentAssertions;
using NUnit.Framework;
using Hallo.Parsers;

namespace Hallo.UnitTest.Sip
{

    [TestFixture]
    public class SipContentLenghtHeaderTests
    {
        [Test]
        public void Parse_ValidFormats_ExpectTheValueToBeOne()
        {
            String[] strings = 
            {
                "1",
                " 1 ",
                "  1",
            };

            
            foreach (string s in strings)
            {
                var h = new SipContentLenghtHeaderParser().Parse(s);
                h.Value.Should().Be(1);
            }
        }

        [Test]
        public void Parse_InvalidFormats_ExpectToThrowParseException()
        {
            String[] strings = 
            {
                "1 ,",
                ",1 ",
                 ", 1 ",
                "100aaaaa",
                "aaaa",
                " aaa ",
                " ,,, ",
                " 1 2"
            };

            int exceptions = 0;
            foreach (string s in strings)
            {
                try
                {
                    new SipContentLenghtHeaderParser().Parse(s);
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
