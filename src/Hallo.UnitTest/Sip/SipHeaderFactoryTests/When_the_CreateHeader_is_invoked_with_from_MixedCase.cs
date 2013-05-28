using System.Text;
using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class When_the_CreateHeader_is_invoked_with_from_mixedcase : When_the_CreateHeader_is_invoked_with_Base
    {
        
        protected override void When()
        {
            _fromHeaderParsed = _headerFactory.CreateHeader("FrOm", _fromHeaderBodyString);
        }
    }

    
}