using System;
using FluentAssertions;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerDialogTests
{
    public class When_in_null_state_a_ok_response_is_sent : SipInviteServerDialogSpecificationBase
    {
        bool _thrown = false;

        protected override void When()
        {
            try
            {
                var okResponse = CreateOkResponse();
                ServerDialog.SetLastResponse(okResponse);
            }
            catch (Exception)
            {
                _thrown = true;
            }
        }

        [Test]
        public void Expect_an_exception_to_be_thrown()
        {
            _thrown.Should().BeTrue();
        }


    }
}