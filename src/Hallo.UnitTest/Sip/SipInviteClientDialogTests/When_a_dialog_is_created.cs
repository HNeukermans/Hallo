using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Hallo.Sip.Stack.Dialogs;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipInviteClientDialogTests
{
    public class When_a_dialog_is_created : SipInviteClientDialogSpecificationBase
    {
        protected override void When()
        {
            base.When();
        }

        [Test]
        public void Expect_the_State_to_be_Null()
        {
            ClientDialog.State.Should().Be(DialogState.Null);
        }

    }
}
