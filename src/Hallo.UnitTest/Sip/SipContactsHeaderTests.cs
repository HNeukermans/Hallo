using System;
using FluentAssertions;
using Hallo.Parsers;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class SipContactsHeaderTests
    {
        [Test]
        public void Parse_ValidFormats_ExpectNotToFail()
        {
            String[] strings = 
            {
                //" <sip:100@192.168.0.1>",
                //" <sip:100@192.168.0.1;transport=udp>",
                "<sip:100@192.168.0.5:5060;transport=udp>;reg-id=1;+sip.instance=\"<urn:uuid:00000000-0000-1000-8000-000b82240363>\""
            };
            
            foreach (string s in strings)
            {
                var h = new SipContactHeaderParser().Parse(s);
            }
        }

        [Test]
        public void Parse_WithExpires_ExpectTheParsedToBeEqualToTheOriginal()
        {
            var original = new SipContactHeaderBuilder()
                .WithExpires(3600).Build();

            var bodyString = original.FormatBodyToString();

            var parsed = new SipContactHeaderParser().Parse(bodyString);

            var c = ObjectComparer.Create();
            c.Compare(original, parsed);
            c.Differences.Should().BeEmpty();
        }

        [Test]
        public void Parse_WithWildCard_ExpectTheParsedToBeEqualToTheOriginal()
        {
            var original = new SipContactHeaderBuilder()
                .WithIsWildCard(true).Build();

            var bodyString = original.FormatBodyToString();

            var parsed = new SipContactHeaderParser().Parse(bodyString);

            var c = ObjectComparer.Create();
            c.Compare(original, parsed);
            c.Differences.Should().BeEmpty();
        }
    }
}