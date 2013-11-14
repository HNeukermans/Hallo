using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FluentAssertions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.Util;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{

    [TestFixture]
    public class When_a_ok_response_is_created_from_a_request : Specification
    {
        private SipResponse _response;
        private SipRequest Request;
        protected override void Given()
        {
            
        }

        protected override void When()
        {
            Request = new SipRequestBuilder().Build();
            _response = Request.CreateResponse(SipResponseCodes.x200_Ok);
        }

        [Test]
        public void Expect_the_ReasonPhrase_to_be_Ok()
        {
            _response.StatusLine.ReasonPhrase.Should().Be("OK");
        }

        [Test]
        public void Expect_the_StatusCode_to_be_200()
        {
            _response.StatusLine.StatusCode.Should().Be(200);
        }

        [Test]
        public void Expect_the_from_headers_to_be_equal()
        {
            ObjectComparer.Create().Compare(_response.From, Request.From).Should().BeTrue();
        }

        [Test]
        public void Expect_the_to_headers_to_be_equal()
        {
            var c = ObjectComparer.Create();
            c.Compare(_response.To,Request.To).Should().BeTrue();
        }

        [Test]
        public void Expect_the_callId_headers_to_be_equal()
        {
            ObjectComparer.Create().Compare(_response.CallId, Request.CallId).Should().BeTrue();
        }

        [Test]
        public void Expect_the_Cseq_headers_to_be_equal()
        {
            ObjectComparer.Create().Compare(_response.CSeq,Request.CSeq).Should().BeTrue();
        }

        [Test]
        public void Expect_the_MaxForwards_headers_to_be_equal()
        {
            ObjectComparer.Create().Compare(_response.MaxForwards,Request.MaxForwards).Should().BeTrue();
        }

        [Test]
        public void Expect_the_Via_headers_to_be_equal()
        {
            var c = ObjectComparer.Create();
            c.Compare(_response.Vias, Request.Vias);
            c.Differences.Should().BeEmpty();
        }

        [Test]
        public void Expect_the_Contact_headers_not_to_copied_from_request()
        {
            _response.Contacts.Count.Should().NotBe(Request.Contacts.Count);
        }
    }

}