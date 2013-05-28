using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip;
using Hallo.Sip.Headers;

namespace Hallo.UnitTest.Builders
{
    public class SipViaHeaderListBuilder : ObjectBuilder<SipHeaderList<SipViaHeader>>
    {
        private SipViaHeader _topMost;
        private SipHeaderList<SipViaHeader> _vias; 

        public SipViaHeaderListBuilder()
        {
            _vias = new SipHeaderList<SipViaHeader>();
        }

        public SipViaHeaderListBuilder Add(SipViaHeader value)
        {
            _vias.Add(value);
            return this;
        }

        public SipViaHeaderListBuilder WithTopMost(SipViaHeader value)
        {
            _topMost = value;
            return this;
        }

        public override SipHeaderList<SipViaHeader> Build()
        {
            if(_topMost != null) _vias.SetTopMost(_topMost);
            return _vias;
        }
    }
}
