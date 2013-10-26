using System;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_Idle : SoftPhoneSpecificationBase
    {
        protected override void OnReceive(SipContext sipContext)
        {
        }

        protected override void When()
        {
            base.When();
        }

        protected override void _calleePhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {

        }

        protected override void _calleePhone_StateChanged(object sender, VoipEventArgs<SoftPhoneState> e)
        {

        }

        [Test]
        public void Expect_the_provider_to_have_0_tx()
        {
            _sipProvider1.ServerTransactionTable.Count.Should().Be(0);
            _sipProvider1.ClientTransactionTable.Count.Should().Be(0);
        }

        [Test]
        public void Expect_a_new_call_started_not_to_throw_exception()
        {
            var call = _phone.CreateCall();
            Action a = () => call.Start(TestConstants.EndPoint2Uri.FormatToString());
            a.ShouldNotThrow();
        }



        protected override void _calleePhone_InternalStateChanged(object sender, EventArgs e)
        {

        }
    }
}
}