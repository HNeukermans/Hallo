using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack;
using Hallo.Util;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipParserContextTests.rfc_7_3_1
{
    [TestFixture]
    public class When_multiple_headerlines_with_the_same_name_are_present : Specification
    {
        protected List<SipRequest> _requestUnderTest = new List<SipRequest>();
        private List<string> _messages;
        
        protected override void Given()
        {
            var messageFormat = "REGISTER sip:192.168.0.1 SIP/2.0\r\n{0}\r\n\r\n";

            _messages = new List<string>() 
            {
                string.Format(messageFormat, "Route: <sip:alice@atlanta.com>\r\nSubject: Lunch\r\nRoute: <sip:bob@biloxi.com>\r\nRoute: <sip:carol@chicago.com>"),
                string.Format(messageFormat, "Route: <sip:alice@atlanta.com>, <sip:bob@biloxi.com>\r\nRoute: <sip:carol@chicago.com>\r\nSubject: Lunch"),
                string.Format(messageFormat, "Subject: Lunch\r\nRoute: <sip:alice@atlanta.com>, <sip:bob@biloxi.com>,\r\n\t<sip:carol@chicago.com>"),
            };
        }
     
        protected override void When()
        {
            var f =  new SipStack();
            var messageFacttory = f.CreateMessageFactory();
            var headerFactory = f.CreateHeaderFactory();
            foreach (string message in _messages)
            {
                var parserContext = new SipParserContext(messageFacttory, headerFactory);
                parserContext.ParseCompleted += (s, e) => _requestUnderTest.Add((SipRequest)e.Message);
                parserContext.Parse(SipFormatter.FormatToBytes(message));
            }
        }

        /// <summary>
        /// tests equality
        /// </summary>
        /// <remarks> tests equality via message stringformatting.</remarks>
        [Test]
        public void Expect_the_order_of_the_route_headers_to_be_preserved()
        {
            string first = null;
            foreach (var request in _requestUnderTest)
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
