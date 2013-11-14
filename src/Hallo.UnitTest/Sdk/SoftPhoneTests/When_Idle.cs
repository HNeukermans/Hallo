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
        
        [Test]
        public void Expect_the_provider_to_have_0_tx()
        {
            //_sipProvider1.ServerTransactionTable.Count.Should().Be(0);
            _sipProvider1.ClientTransactionTable.Count.Should().Be(0);
        }

        [Test]
        public void Expect_a_new_call_started_not_to_throw_exception()
        {
            var call = _phone.CreateCall();
            Action a = () => call.Start(TestConstants.EndPoint1Uri.FormatToString());
            a.ShouldNotThrow();
        }

        
        protected override void OnTestClientUaReceive(SipContext sipContext)
        {
            
        }
    }
}
