using System;
using FluentAssertions;
using Hallo.Sip;
using NUnit.Framework;
using Hallo.Parsers;

namespace Hallo.UnitTest.Sip
{
        [TestFixture]
        public class SipCallIdHeaderTests
        {
            [Test]
            public void Parse_ValidFormats_ExpectTheValueNotToBeEmpty()
            {
                String[] strings = 
                {
                    "1",
                    "11111",
                    " aaabbb ",
                    "ccccdddddd ",
                    " ccccdddddd",
                    " hannes_neukermans@hotmail.com",
                    "01234567890123456789012345678901234567890123456789",
                    " ??? "
                };

                foreach (string s in strings)
                {
                    var h = new SipCallIdHeaderParser().Parse(s);
                    h.Value.Should().NotBeEmpty();
                    h.Value.Should().NotContain(" ");
                }
            }

            [Test]
            public void Parse_InvalidFormats_ExpectToThrowParseException()
            {
                String[] strings = 
                {
                    " ",
                    "hannes_neukermans ,",
                    ",hannes_neukermans ",
                     ", hannes_neukermans ",
                    "100aaaaa hannes_neukermans",
                    "012345678901234567890123456789012345678901234567890",
                    "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                    " ,,, "
                };

                int exceptions = 0;
                foreach (string s in strings)
                {
                    try
                    {
                        new SipCallIdHeaderParser().Parse(s);
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
