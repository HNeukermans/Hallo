using FluentAssertions;
using Hallo.Sip;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Sip.SipParserTests;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class When_a_request_is_lexed : SipLexerTestBase
    {
        protected override void Given()
        {
            base.Given();
            SipRequest sipMessage = new SipRequestBuilder().Build();
            _bytes = SipFormatter.FormatMessage(sipMessage);
        }

        [Test]
        public void Expect_the_listener_OnBodyBytes_to_be_null()
        {
            OnBodyBytesExecuteReceived.Should().BeTrue();
        }

        [Test]
        public void Expect_the_listener_OnFirstLine_to_be_invoked()
        {
            OnFirstLineExecuteReceived.Should().NotBeNull();
        }

        [Test]
        public void Expect_the_listener_OnHeader_to_be_invoked()
        {
            OnHeaderExecuteReceived.Should().NotBeEmpty();
        }
    }
}