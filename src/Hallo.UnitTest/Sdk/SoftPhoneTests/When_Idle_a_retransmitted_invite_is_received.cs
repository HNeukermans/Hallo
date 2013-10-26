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
using Hallo.Sdk.SoftPhoneStates;
using Moq;
using Hallo.Component;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{   
    internal class When_Idle_a_retransmitted_invite_is_received : SoftPhoneSpecificationBase
    {
        private SoftPhoneStateProxy _idleStateCountable;


       public When_Idle_a_retransmitted_invite_is_received()
       {
           _idleStateCountable = new SoftPhoneStateProxy(new IdleState());

           Mock<ISoftPhoneStateProvider> softPhoneStateProviderMock = new Mock<ISoftPhoneStateProvider>();
           softPhoneStateProviderMock.Setup(s => s.GetIdle()).Returns(_idleStateCountable);
           _stateProvider = softPhoneStateProviderMock.Object;
       }     

        protected override void _calleePhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {
            _incomingCall = e.Item;
            _firedIncomingCall = true;
        }

        protected override void _calleePhone_StateChanged(object sender, VoipEventArgs<SoftPhoneState> e)
        {
            _firedStateChanged = true;
        }
                
        protected override void OnReceive(SipContext sipContext)
        {
            _counter++;
            if( _counter == 2) _wait.Set();

            /*continue test execution*/
            //_wait.Set(); move to statechanged, as this is the last event in code.
        } 

        protected override void When()
        {
            _network.SendTo(SipFormatter.FormatMessage(_invite), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _network.SendTo(SipFormatter.FormatMessage(_invite), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);

             _wait.WaitOne(TimeSpan.FromSeconds(1));
            // _wait.WaitOne(); /*debug*/
        }
        
        [Test]
        public void Expect_the_IdleState_ProcessRequest_invocations_to_be_1()
        {
            _idleStateCountable.ProcessRequestCounter.Should().Be(1);
        }

        protected override void _calleePhone_InternalStateChanged(object sender, EventArgs e)
        {
            
        }
    }
    
}