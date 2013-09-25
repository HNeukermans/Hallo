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
    public class SipCSeqHeaderTests
    {
        [Test]
        public void Parse_ValidFormats_ExpectNotToFail()
        {
            String[] strings = 
            {
                " 1002 REGISTER",
                " 1002    REGISTER       ",
                " 1002 REGISTER",
                " 1002 INVITE",
            };


            foreach (string s in strings)
            {
                var h = new SipCSeqHeaderParser().Parse(s);
            }
        }

        [Test]
        public void Parse_InvalidFormats_ExpectToThrowParseException()
        {
            String[] strings = 
            {
                "1002 REGISTER ,",
                "1002 invite",
                "1002 INvite",
                ", 1002 REGISTER ",
                "1002, REGISTER ",
                "1002",
                "REGISTER",
                "aaaaa REGISTER",
                " 1 2"
            };

            int exceptions = 0;
            foreach (string s in strings)
            {
                try
                {
                    new SipCSeqHeaderParser().Parse(s);
                }
                catch (ParseException e)
                {
                    exceptions++;
                }
            }

            exceptions.Should().Be(strings.Length);
        }

        [Test]
        [ExpectedException(typeof(ParseException))]
        public void Parse_ContainsNonNumericSequence_ExpectThrowException()
        {
            String[] strings = 
            {
                "CSeq: 1002a REGISTER",
            };

            for (int i = 0; i < strings.Length; i++)
            {
                var p = new SipCSeqHeaderParser();
                var result = p.Parse(strings[i]);
                Assert.AreEqual(strings[i], SipFormatter.FormatHeader(result));
            }
        }
    }
}
