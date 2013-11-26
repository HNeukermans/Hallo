using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_Alice_calls_Offline_Bob : IntegrationTestBase
    {
        protected ManualResetEvent _waitforTimeOut = new ManualResetEvent(false);
        
        protected override void When()
        {
            _phoneBob.Stop();
            CallBob();
            _waitforTimeOut.WaitOne();
            _outgoingCallBob.Stop();
            CallBob();
        }

        [Test]
        public void Expect_alice_call_to_be_in_error_state()
        {
            _callStateAlice.Should().Be(CallState.Error);
        }

        [Test]
        public void Expect_alice_callerror_to_not_be_null()
        {
            _callErrorAlice.Should().NotBeNull();
        }

        [Test]
        public void Expect_alice_callerror_to_be_TransactionTimeout()
        {
            _callErrorAlice.Type.Should().Be(CallError.TransactionTimeout);
        }

        protected override void OnAliceCallStateChanged()
        {
            if(_callStateAlice == CallState.Error) _waitforTimeOut.Set();
        }
    }
}