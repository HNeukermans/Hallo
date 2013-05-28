using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Hallo.Sip.Headers;
using Hallo.Sip.Util;
using Hallo.UnitTest.Helpers;

namespace Hallo.UnitTest.Builders
{
    public class SipViaHeaderBuilder : ObjectBuilder<SipViaHeader>
    {
        private string _transport;
        private IPEndPoint _sentBy;
        private IPAddress _received;
        private string _branch;
        private bool _rport;
        private bool _useRport;

        public SipViaHeaderBuilder()
        {
            _transport = SipConstants.Udp;
            _sentBy = TestConstants.IpEndPoint1;
            _branch = SipUtil.CreateBranch();
        }

        public SipViaHeaderBuilder WithTransport(string value)
        {
            _transport = value;
            return this;
        }

        public SipViaHeaderBuilder WithSentBy(IPEndPoint value)
        {
            _sentBy = value;
            return this;
        }
        
        public SipViaHeaderBuilder WithReceived(IPAddress value)
        {
            _received = value;
            return this;
        }

        public SipViaHeaderBuilder WithUseRport(bool value)
        {
            _useRport = value;
            return this;
        }

        public SipViaHeaderBuilder WithBranch(string value)
        {
            _branch = value;
            return this;
        }
        
        public override SipViaHeader Build()
        {
            SipViaHeader item = new SipViaHeader();
            item.Transport = _transport;
            item.SentBy = _sentBy;
            item.Received = _received;
            item.Branch = _branch;
            item.UseRport = _useRport;
            return item;
        }
    }
}
