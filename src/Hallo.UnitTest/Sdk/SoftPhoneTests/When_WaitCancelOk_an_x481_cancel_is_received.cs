using System.Linq;
using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitCancelOk_an_x481_cancel_is_received : When_WaitCancelOk_Base
    {
        protected ManualResetEvent _waitforProcessed = new ManualResetEvent(false);

        protected override void When()
        {
            var ok = _receivedCancel.CreateResponse(SipResponseCodes.x481_Call_Transaction_Does_Not_Exist);
            _network.SendTo(SipFormatter.FormatMessage(ok), _testClientUaEndPoint, _phoneUaEndPoint);
            _waitforProcessed.WaitOne();
        }

        protected override void AfterPhoneProcessedResponse(IInternalSoftPhone softPhone, Hallo.Sip.Stack.SipResponseEvent responseEvent)
        {
            if (responseEvent.Response.StatusLine.StatusCode == 180)
            {
                _ringingProcessed.Set();
            }

            if (responseEvent.Response.StatusLine.StatusCode == 481 && responseEvent.Response.CSeq.Command == SipMethods.Cancel)
            {
                _waitforProcessed.Set();
            }
        }

        [Test]
        public void Expect_the_InternalState_to_transition_to_idle()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetIdle());
        }

        [Test]
        public void Expect_InviteClientTransactionTable_to_have_2_tx()
        {
            /*invite + cancel. Cancel tx is removed from table after the endcompleted timer fires.*/
            _sipProvider1.ClientTransactionTable.Count.Should().Be(2);
        }

        [Test]
        public void Expect_one_of_the_tx_to_be_inviteclient_in_proceeding_state()
        {
            /*invite*/
            var inviteTx = _sipProvider1.ClientTransactionTable.Values.ToList().OfType<SipInviteClientTransaction>().SingleOrDefault();
            inviteTx.Should().NotBeNull();
            inviteTx.State.Should().Be(SipInviteClientTransaction.ProceedingState);
        }

        [Test]
        public void Expect_the_second_tx_to_be_noninviteclient_in_completed_state()
        {
            /*cancel*/
            var cancelTx = _sipProvider1.ClientTransactionTable.Values.ToList().OfType<SipNonInviteClientTransaction>().SingleOrDefault();
            cancelTx.Should().NotBeNull();
            cancelTx.State.Should().Be(SipNonInviteClientTransaction.CompletedState);
        }

        [Test]
        public void Expect_the_callstate_to_be_Cancelled()
        {
            _callState.Should().Be(CallState.Cancelled);
        }

       
    }
}