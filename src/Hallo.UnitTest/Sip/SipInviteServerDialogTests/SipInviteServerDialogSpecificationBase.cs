using System;
using Hallo.Component;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using Hallo.Sip.Util;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.UnitTest.Sip.SipClientDialogTests;
using Hallo.UnitTest.Stubs;
using Hallo.Util;
using Moq;

namespace Hallo.UnitTest.Sip.SipInviteServerDialogTests
{
    public class SipInviteServerDialogSpecificationBase : Specification
    {
        protected ITimerFactory TimerFactory { get; set; }
        protected Mock<ISipMessageSender> Sender { get; set; }
        protected Mock<ISipListener> Listener { get; set; }
        protected SipNonInviteServerTransaction _ctx;
        protected SipServerTransactionTable _txTable;
        protected SipRequest _request;

        protected int _onProcessResponseCount;
        protected SipProvider _provider;
        protected SipRequest _inviteRequest;
        protected SipInviteServerDialog _dialog;
        protected string _fromTag;
        protected string _toTag;
        protected int _cSeq;
        protected string _callId;
        protected SipStack _sipStack;
        protected SipResponseEvent _responseEvent;
        protected SipRequestEvent _requestEvent;
        protected SipResponse _response;
        protected ISipServerTransaction _inviteTransaction;


        protected TxTimerStub RetransitOkTimer { get; set; }
        protected TxTimerStub TimeOutTimer { get; set; }
        

        protected SipRequest CreateInviteRequest()
        {
            var r = new SipRequestBuilder()
                .WithRequestLine(
                    new SipRequestLineBuilder().WithMethod(SipMethods.Invite).Build())
                .WithCSeq(
                    new SipCSeqHeaderBuilder().WithCommand(SipMethods.Invite).WithSequence(_cSeq).Build())
                .WithFrom(
                    new SipFromHeaderBuilder().WithSipUri(TestConstants.AliceUri).WithTag(_fromTag).Build())
                .WithTo(
                    new SipToHeaderBuilder().WithSipUri(TestConstants.BobUri).WithTag(null).Build())
                .WithCallId(
                    new SipCallIdHeaderBuilder().WithValue(_callId).Build())
                .WithContacts(
                new SipContactHeaderListBuilder()
                .Add(new SipContactHeaderBuilder().WithSipUri(TestConstants.AliceContactUri).Build())
                .Build())
                .WithRecordRoutes(
                new SipRecordRouteHeaderListBuilder()
                .Add(new SipRecordRouteHeaderBuilder().WithSipUri(TestConstants.AliceProxyUri).Build())
                .Build())
                .Build();

            return r;
        }

        protected SipResponse CreateRingingResponse()
        {
            var r = ReceivedRequest.CreateResponse(SipResponseCodes.x180_Ringing);
            r.To.Tag = _toTag;
            return r;
        }

        protected SipResponse CreateOkResponse()
        {
            var r = ReceivedRequest.CreateResponse(SipResponseCodes.x200_Ok);
            r.To.Tag = _toTag;
            return r;
        }

        public SipInviteServerDialogSpecificationBase()
        {
            _toTag = SipUtil.CreateTag();
            _fromTag = SipUtil.CreateTag();
            _callId = SipUtil.CreateCallId();
            _cSeq = 2;

            Sender = new Mock<ISipMessageSender>();
            Listener = new Mock<ISipListener>();
            DialogTable = new SipDialogTable();
            ReceivedRequest = CreateInviteRequest();
            var tfb = new TimerFactoryStubBuilder();
            InviteStx = new Mock<ISipInviteServerTransaction>();
            InviteStx.Setup((tx) => tx.Request).Returns(ReceivedRequest);
            TimerFactory = tfb.Build();

        }

        protected Mock<ISipInviteServerTransaction> InviteStx { get; set; }

        protected SipRequest ReceivedRequest { get; set; }
        
        protected SipDialogTable DialogTable { get; set; }

        protected SipInviteServerDialog ServerDialog { get; set; }
        
        protected override void  Given()
        {
            //setup the Request property of the inc inviteserver
            ReceivedRequest = CreateInviteRequest();
            var tfb = new TimerFactoryStubBuilder();
            InviteStx = new Mock<ISipInviteServerTransaction>();
            InviteStx.Setup((tx) => tx.Request).Returns(ReceivedRequest);
            
            //create the dialog;
            ServerDialog = new SipInviteServerDialog(InviteStx.Object,
                                                   DialogTable,
                                                   TimerFactory,
                                                   new SipHeaderFactory(),
                                                   new SipMessageFactory(), new SipAddressFactory(), 
                                                   Sender.Object,
                                                   Listener.Object,
                                                   TestConstants.IpEndPoint1);

            GivenOverride();
        }

       
        protected virtual void GivenOverride()
        {

        }
    }
}