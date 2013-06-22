using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientDialogTests
{
    public class When_in_confirmed_state_a_bye_request_is_sent : SipInviteClientDialogSpecificationBase
    {
        private int _beforeSentSeqNr;
        private Mock<ISipClientTransaction> _txMock;

        protected override void GivenOverride()
        {
            /*force it to go into confirmed state*/
            ReceivedResponse = CreateOkResponse();
            ClientDialog.SetLastResponse(CreateRingingResponse());
            ClientDialog.SetLastResponse(CreateOkResponse());
            ClientDialog.State.Should().Be(DialogState.Confirmed); /*required assertion*/
            _beforeSentSeqNr = ClientDialog.LocalSequenceNr;
        }

        protected override void When()
        {
            var request = ClientDialog.CreateRequest(SipMethods.Bye);
            _txMock = new Mock<ISipClientTransaction>();
            _txMock.Setup((p) => p.Request).Returns(request);
            ClientDialog.SendRequest(_txMock.Object);
        }

        [Test]
        public void Expect_the_LocalSequenceNr_to_be_incremented()
        {
            _beforeSentSeqNr++;
            ClientDialog.LocalSequenceNr.Should().Be(_beforeSentSeqNr);
        }

        [Test]
        public void Expect_the_request_to_be_sent()
        {
            _txMock.Verify((tx)=> tx.SendRequest(), Times.Once());
        }

        [Test]
        public void Expect_the_dialog_to_transition_to_terminated_state()
        {
            ClientDialog.State.Should().Be(DialogState.Terminated);
        }
    }
}