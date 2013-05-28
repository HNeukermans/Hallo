using FluentAssertions;
using Hallo.Sip;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public class When_in_proceeding_state_a_x200final_response_is_send : TxSpecificationBase
    {
        private SipResponse _200FinalResponse;

        public When_in_proceeding_state_a_x200final_response_is_send()
            : base()
        {

        }

        protected override void GivenOverride()
        {
            Stx.Start();
            TxTable.ContainsKey(Stx.GetId()).Should().BeTrue();
        }

        protected override void When()
        {
            _200FinalResponse = CreateFinalResponse();
            Stx.SendResponse(_200FinalResponse);
        }

        [Test]
        public void Expect_the_the_transaction_to_be_removed_from_TxTable_()
        {
            TxTable.ContainsKey(Stx.GetId()).Should().BeFalse();
        }

        [Test]
        public void Expect_the_LatestResponse_to_be_the_nonx200_final_response()
        {
            Stx.LatestResponse.Should().Be(_200FinalResponse);
        }

        [Test]
        public void Expect_the_most_recent_response_to_be_send()
        {
            Sender.Verify((r) => r.SendResponse(_200FinalResponse), Times.Exactly(1));
        }
    }
}