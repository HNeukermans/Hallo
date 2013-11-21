using System.Linq;
using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_Ringing_a_cancel_is_received : When_Ringing_Base
    {
        private ManualResetEvent[] waitHandles = new[] {new ManualResetEvent(false), new ManualResetEvent(false)};
            
        protected override void When()
        {
            var cancelRequest = CreateCancelRequest(_invite);
            _network.SendTo(SipFormatter.FormatMessage(cancelRequest), _testClientUaEndPoint, _phoneUaEndPoint);
            WaitHandle.WaitAll(waitHandles, 2000*100);
        }

        protected override void OnTestClientUaReceive(SipContext sipContext)
        {
            if (sipContext.Response != null)
            {
                if (sipContext.Response.CSeq.Command == SipMethods.Invite)
                {
                    if (sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x180_Ringing)
                    {
                        if (_receivedRingingResponse == null) _receivedRingingResponse = sipContext.Response;
                    }
                    else if (sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x487_Request_Terminated)
                    {
                        /**/
                        waitHandles[0].Set();
                    }
                }
                else if (sipContext.Response.CSeq.Command == SipMethods.Cancel && sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x200_Ok)
                {
                    /*receive ok to cancel*/
                    waitHandles[1].Set();
                }
            }
        }

        [Test]
        public void Expect_the_phone_to_transition_to_idle_state()
        {
            _phone.InternalState.Should().Be(_stateProvider.GetIdle());
        }

        [Test]
        public void Expect_the_dialogtable_to_contains_2_tx()
        {
            /*invite + cancel*/
            _sipProvider1.ServerTransactionTable.Values.ToList().Should().HaveCount(2);
        }

        [Test]
        public void Expect_one_of_the_tx_to_be_inviteserver_in_completed_state()
        {
            /*invite*/
            var inviteServerTx = _sipProvider1.ServerTransactionTable.Values.ToList().OfType<SipInviteServerTransaction>().SingleOrDefault();
            inviteServerTx.Should().NotBeNull();
            inviteServerTx.State.Should().Be(SipInviteServerTransaction.CompletedState);
        }

        [Test]
        public void Expect_the_second_tx_to_be_noninviteserver_in_completed_state()
        {
            /*cancel*/
            var cancelServerTx = _sipProvider1.ServerTransactionTable.Values.ToList().OfType<SipNonInviteServerTransaction>().SingleOrDefault();
            cancelServerTx.Should().NotBeNull();
            cancelServerTx.State.Should().Be(SipNonInviteServerTransaction.CompletedState);
        }
        
        [Test]
        public void Expect_the_callstate_to_Cancelled()
        {
            _callState.Should().Be(CallState.Cancelled);
        }
        
        [Test]
        public void Expect_dialogtable_to_be_empty()
        {
            _sipProvider1.DialogTable.Count.Should().Be(0);
        }
    }
}