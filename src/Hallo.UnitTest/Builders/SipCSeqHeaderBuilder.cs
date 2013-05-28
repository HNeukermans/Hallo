using Hallo.Sip;
using System.Linq;
using Hallo.Sip.Headers;

namespace Hallo.UnitTest.Builders
{
    public class SipCSeqHeaderBuilder : ObjectBuilder<SipCSeqHeader>
    {
        private int _sequence;
        private string _command;
        private string _name;
        private bool _isList;


        public SipCSeqHeaderBuilder()
        {
            _sequence = 1;
            _command = SipMethods.Register;
            _name = SipHeaderNames.CSeq;
        }

        public SipCSeqHeaderBuilder WithSequence(int value)
        {
            _sequence = value;
            return this;
        }

        public SipCSeqHeaderBuilder WithCommand(string value)
        {
            _command = value;
            return this;
        }

        public SipCSeqHeaderBuilder WithName(string value)
        {
            _name = value;
            return this;
        }

        public SipCSeqHeaderBuilder WithIsList(bool value)
        {
            _isList = value;
            return this;
        }

        public override SipCSeqHeader Build()
        {
            SipCSeqHeader item = new SipCSeqHeader();
            item.Sequence = _sequence;
            item.Command = _command;
           
            return item;
        }
    }
}