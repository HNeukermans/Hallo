using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
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
        public void Expect_the_InternalState_to_remain_in_WaitCancelOk()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetWaitCancelOk());
        }

        [Test]
        public void Expect_CancelTransaction_to_transition_to_completed_state()
        {
            _phone.PendingInvite.CancelTransaction.As<SipNonInviteClientTransaction>().State.Should().Be(SipNonInviteClientTransaction.CompletedState);
        }

        [Test]
        public void Expect_InviteClientTransactionTable_to_have_2_tx()
        {
            /*invite + cancel. Cancel tx is removed from table after the endcompleted timer fires.*/
            _sipProvider1.ClientTransactionTable.Count.Should().Be(2);
        }
    }
}