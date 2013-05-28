using System.Text;
using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    public class When_the_CreateHeader_is_invoked_with_Base : Specification
    {
        protected SipHeaderFactory _headerFactory;
        protected SipFromHeader _fromHeader;
        protected SipHeaderBase _fromHeaderParsed;
        protected string _fromHeaderBodyString;

        [Test]
        public void Expect_the_header_not_to_be_null()
        {
            _fromHeaderParsed.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_header_to_be_of_type_SipFromHeader()
        {
            _fromHeaderParsed.Should().BeOfType<SipFromHeader>();
        }

        [Test]
        public void Expect_the_result_to_be_equal_to_the_original()
        {
            var c = ObjectComparer.Create();

            c.Compare(_fromHeader, _fromHeaderParsed).Should().BeTrue();
        }

        protected override void Given()
        {
            _headerFactory = new SipStack().CreateHeaderFactory();
            _fromHeader = new SipFromHeaderBuilder().Build();
            StringBuilder sb = new StringBuilder();
            _fromHeaderBodyString = _fromHeader.FormatBodyToString();
        }
    }
}