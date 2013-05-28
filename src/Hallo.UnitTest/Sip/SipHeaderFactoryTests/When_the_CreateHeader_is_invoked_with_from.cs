using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class When_the_CreateHeader_is_invoked_with_from : When_the_CreateHeader_is_invoked_with_Base
    {

        protected override void When()
        {
            _fromHeaderParsed = _headerFactory.CreateHeader("from", _fromHeaderBodyString);
        }
    }
}