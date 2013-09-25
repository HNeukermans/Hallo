using System;
using System.Text;
using FluentAssertions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack;
using Hallo.UnitTest.Builders;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;


namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class When_the_CreateHeader_is_invoked_with_unknown : Specification
    {
        private SipHeaderFactory _headerFactory;
        private bool _exceptionIsThrown;

        protected override void Given()
        {
            _headerFactory = new SipStack().CreateHeaderFactory();
        }

        protected override void When()
        {
            try
            {
                _headerFactory.CreateHeader("unknown", "");
            }
            catch (ParseException e)
            {
                _exceptionIsThrown = true;
            }
            
        }

        [Test]
        public void Test()
        {
            new object().Should().NotBeNull();
        }

        [Test]
        public void Expect_it_to_throw_the_SipParseException()
        {
            _exceptionIsThrown.Should().BeTrue();
        }
        
    }
}
