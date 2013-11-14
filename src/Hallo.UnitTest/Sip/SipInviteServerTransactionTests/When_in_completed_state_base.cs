using FluentAssertions;
using Hallo.Component;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.UnitTest.Builders;

namespace Hallo.UnitTest.Sip.SipInviteServerTransactionTests
{
    public abstract class When_in_completed_state_base : TxSpecificationBase
    {
        protected SipResponse _non200FinalResponse;

        protected When_in_completed_state_base()
            : base()
        {
            var tf = new TimerFactoryStubBuilder().Build();
            TimerFactory = tf;
        }

        protected override void GivenOverride()
        {
            //((SipAbstractServerTransaction) Stx).Initialize();
            _non200FinalResponse = CreateFinalResponse(302, "Moved Temporarily");
            Stx.SendResponse(_non200FinalResponse);
            Stx.State.Should().Be(SipInviteServerTransaction.CompletedState); /*required assertion*/
        }
    }
}