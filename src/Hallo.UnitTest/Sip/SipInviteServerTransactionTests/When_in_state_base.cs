using System;
using Hallo.Component;
using Hallo.Sip.Stack;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Stubs;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public abstract class When_in_state_base : TxSpecificationBase
    {
       
        protected When_in_state_base():base()
        {
            var tf = new TimerFactoryStubBuilder().Build();
           TimerFactory = tf;
        }

    }
}