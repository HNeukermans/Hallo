using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip.Headers;
using NUnit.Framework;
using Hallo.Sip;
using Hallo.Sip.Util;

namespace Hallo.UnitTest
{
    public abstract class Specification
    {
        //protected SipHeaderFactory HeaderFactory { get; private set; }
        //protected SipParserFactory ParserFactory { get; private set; }
        //protected SipMessageFactory MessageFactory { get; private set; }
        //protected SipRequest Request { get; private set; }

        protected Specification()
        {
            //HeaderFactory = new SipHeaderFactory();
            //ParserFactory = new SipParserFactory();
            //MessageFactory = new SipMessageFactory();

            //InitializeHeaders();

            //Request = MessageFactory.CreateRequest(ToHeader.SipUri, SipMethods.Register, CallIdHeader, CSeqHeader,
            //                                       FromHeader, ToHeader, ViaHeaders.First(), MaxForwardsHeader);
        }

        private void InitializeHeaders()
        {
            //var header = HeaderFactory.CreateViaHeader(IPAddress.Loopback, SipConstants.DefaultSipPort, SipConstants.Udp, SipConstants.BranchPrefix + "branch1");
            //var topHeader = HeaderFactory.CreateViaHeader(IPAddress.Parse("1.0.0.1"), SipConstants.DefaultSipPort, SipConstants.Udp, SipConstants.BranchPrefix + "branch2");

            //ViaHeaders = new SipStackedHeaderList<SipViaHeader>();
            //ViaHeaders.Add(header);
            //ViaHeaders.SetTopMost(topHeader);

            //ToHeader = HeaderFactory.CreateToHeader("Callee", new SipUriParser().Parse("sip:callee@1.2.3.4"), "calleetag");
            //FromHeader = HeaderFactory.CreateFromHeader("Caller", new SipUriParser().Parse("sip:caller@4.3.2.1"),  "callertag");
            //CallIdHeader = HeaderFactory.CreateCallIdHeader(SipUtil.CreateCallId());
            //CSeqHeader = HeaderFactory.CreateSCeqHeader(SipMethods.Register, 1);
            //MaxForwardsHeader = HeaderFactory.CreatMaxForwardsHeader();
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            Given();
            When();
        }

        [TestFixtureTearDown]
        public virtual void CleanUp()
        { 
            
        }

        protected virtual void Given() { }
        protected virtual void When() { }

        //public  SipStackedHeaderList<SipViaHeader> ViaHeaders { get; set; }

        //public SipFromHeader FromHeader { get; set; }

        //public SipToHeader ToHeader { get; set; }

        //public SipMaxForwardsHeader MaxForwardsHeader { get; set; }
    }
}
