using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    /// <summary>
    /// Tests the condition under which an ok is received with a branchid that does not match the cancel brnachid.
    /// </summary>
    internal class When_WaitCancelOk_a_not_tx_matching_ok_cancel_is_received : When_WaitCancelOk_Base
    {
        protected ManualResetEvent _waitforOkCancelProcessed = new ManualResetEvent(false);

        protected override void When()
        {
            /*send ok to cancel*/
            var ok = _receivedCancel.CreateResponse(SipResponseCodes.x200_Ok);
            ok.Vias.GetTopMost().Branch = "no match";
            _network.SendTo(SipFormatter.FormatMessage(ok), _testClientUaEndPoint, _phoneUaEndPoint);
            _waitforOkCancelProcessed.WaitOne();
        }

        protected override void AfterPhoneProcessedResponse(IInternalSoftPhone softPhone, Hallo.Sip.Stack.SipResponseEvent responseEvent)
        {
            if (responseEvent.Response.StatusLine.StatusCode == 180)
            {
                _ringingProcessed.Set();
            }

            if (responseEvent.Response.StatusLine.StatusCode == 200 && responseEvent.Response.CSeq.Command == SipMethods.Cancel)
            {
                _waitforOkCancelProcessed.Set();
            }
        }

        [Test]
        public void Expect_CancelTransaction_to_be_remain_in_trying_state()
        {
            _phone.PendingInvite.CancelTransaction.As<SipNonInviteClientTransaction>().State.Should().Be(SipNonInviteClientTransaction.TryingState);
        }


    }
}