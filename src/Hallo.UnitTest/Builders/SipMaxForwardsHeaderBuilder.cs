using Hallo.Sip;
using Hallo.Sip.Headers;

namespace Hallo.UnitTest.Builders
{
    public class SipMaxForwardsHeaderBuilder : ObjectBuilder<SipMaxForwardsHeader>
    {
        private int _value;
        private string _name;
        private bool _isList;


        public SipMaxForwardsHeaderBuilder()
        {
            _value = 70;
            _name = SipHeaderNames.MaxForwards;
        }

        public SipMaxForwardsHeaderBuilder WithValue(int value)
        {
            _value = value;
            return this;
        }

        public SipMaxForwardsHeaderBuilder WithName(string value)
        {
            _name = value;
            return this;
        }

        public SipMaxForwardsHeaderBuilder WithIsList(bool value)
        {
            _isList = value;
            return this;
        }

        public override SipMaxForwardsHeader Build()
        {
            SipMaxForwardsHeader item = new SipMaxForwardsHeader();
            item.Value = _value;
            return item;
        }
    }
}