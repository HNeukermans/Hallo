using System.Collections.Generic;
using Hallo.Sip;
using Hallo.Sip.Stack;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipParserContextTests.rfc_7_3_1
{
    [TestFixture]
    public class When_headerlines_are_folded : Specification
    {
        protected List<SipRequest> _subjectFoldedRequest = new List<SipRequest>();
        private List<string> _messages;

        protected override void Given()
        {
            var messageFormat = "REGISTER sip:192.168.0.1 SIP/2.0\r\n{0}\r\n\r\n";

            _messages = new List<string>()
                            {
                                string.Format(messageFormat,
                                              "Subject: I know you're there, pick up the phone and talk to me!"),
                                string.Format(messageFormat,
                                              "Subject: I know you're there,\r\n pick up the phone\r\n\tand talk to me!")
                            };
        }

        protected override void When()
        {
            var f = new SipStack();
            var messageFacttory = f.CreateMessageFactory();
            var headerFactory = f.CreateHeaderFactory();
            foreach (string message in _messages)
            {
                var parserContext = new SipParserContext(messageFacttory, headerFactory);
                parserContext.ParseCompleted += (s, e) => _subjectFoldedRequest.Add((SipRequest)e.Message);
                parserContext.Parse(SipFormatter.FormatToBytes(message));
            }
        }

        [Test]
        public void Expect_all_request_are_equal()
        {
            string first = null;
            foreach (var request in _subjectFoldedRequest)
            {
                if (first == null)
                    first = SipFormatter.FormatMessageEnvelope(request);
                else
                {
                    var other = SipFormatter.FormatMessageEnvelope(request);
                    Assert.AreEqual(other, first);
                }
            }
        }

    }
}