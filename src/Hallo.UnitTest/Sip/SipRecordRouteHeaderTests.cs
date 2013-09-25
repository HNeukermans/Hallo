using System;
using FluentAssertions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    public class SipRecordRouteHeaderTests
    {
        [Test]
        public void Parse_ValidFormats_ExpectNotToFail()
        {
            String[] strings = 
            {
                " <sip:100@192.168.0.1>;lr",
                "<sip:100@192.168.0.1>;lr",
            };


            foreach (string s in strings)
            {
                var h = new SipRecordRouteHeaderParser().Parse(s);
            }
        }

        [Test]
        public void Parse_InvalidFormats_ExpectToThrowParseException()
        {
            String[] strings = 
            {
                "<sip:100@192.168.0.1>;", /*strict routing is not supported*/
               
            };

            int exceptions = 0;
            foreach (string s in strings)
            {
                try
                {
                    new SipFromHeaderParser().Parse(s);
                }
                catch (ParseException e)
                {
                    exceptions++;
                }
            }

            exceptions.Should().Be(strings.Length);
        }

        [Test]
        public void Parse_WithLooseRouting_ExpectTheParsedToBeEqualToTheOriginal()
        {
            var original = new SipRecordRouteHeader();
            original.SipUri = new SipUriBuilder().WithLoooseRouting(true).Build();

            var bodyString = original.FormatBodyToString();

            var parsed = new SipRecordRouteHeaderParser().Parse(bodyString);

            var c = ObjectComparer.Create();
            c.Compare(original, parsed);
            c.Differences.Should().BeEmpty();
        }
    }
}