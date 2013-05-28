using System;
using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Sip.SipClientDialogTests;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipServerDialogTests
{
    [TestFixture]
    public class When_in_null_state_a_ringing_response_is_sent_without_a_to_tag : SipDialogSpecificationBase
    {
        private bool _exceptionThrown = false;
        protected override void GivenOverride()
        {

        }

        protected override void When()
        {
            _inviteRequest = CreateInviteRequest();
            var c = new SipContextBuilder().WithRequest(_inviteRequest).Build();

            _contextSource.FireNewContextReceivedEvent(c);
        }

        public override void OnProcessRequestOverride()
        {
            if (_requestEvent.Request.RequestLine.Method == SipMethods.Invite)
            {
                try
                {
                    _inviteTransaction = _provider.CreateServerTransaction(_requestEvent.Request);
                    var dialog = _provider.CreateServerDialog(_inviteTransaction);
                    var response = CreateRingingResponse();
                    response.To.Tag = null;
                    _inviteTransaction.SendResponse(response);
                }
                catch (Exception)
                {
                    _exceptionThrown = true;
                    throw;
                }
            }
        }

        [Test]
        public void Expect_an_exception_to_be_thrown()
        {
            _exceptionThrown.Should().BeTrue();
        }

    }

}