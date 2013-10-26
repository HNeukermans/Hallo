using System;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_WaitProvisional : SoftPhoneSpecificationBase
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
        
        protected override void _calleePhone_InternalStateChanged(object sender, EventArgs e)
        {

        }
    }
}