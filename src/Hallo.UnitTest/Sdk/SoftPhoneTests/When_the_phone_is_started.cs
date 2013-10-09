using System;
using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Util;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;
using Hallo.Component;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_the_phone_is_started : SoftPhoneSpecificationBase
    {      
        protected override void OnReceive(SipContext sipContext)
        {
            /*continue test execution*/
            //_wait.Set(); move to statechanged, as this is the last event in code.
        }

        protected override void _calleePhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {

        }

        protected override void _calleePhone_StateChanged(object sender, VoipEventArgs<SoftPhoneState> e)
        {

        }
                
        [Test]
        public void Expect_the_phone_to_be_in_Idle_state()
        {
            _calleePhone.InternalState.Should().Be(_stateProvider.GetIdle());
        }
               
        [Test]
        public void Expect_the_provider_to_have_0_tx()
        {
            _sipProvider1.ServerTransactionTable.Count.Should().Be(0);
            _sipProvider1.ClientTransactionTable.Count.Should().Be(0);
        }


        protected override void _calleePhone_InternalStateChanged(object sender, EventArgs e)
        {
           
        }
    }
   
}