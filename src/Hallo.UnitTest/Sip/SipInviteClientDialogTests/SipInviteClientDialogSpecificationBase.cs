using System;
using Hallo.Component;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using Hallo.Sip.Util;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Moq;

namespace Hallo.UnitTest.Sip.SipInviteClientDialogTests
{
    public class SipInviteClientDialogSpecificationBase : Specification
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
        protected string _remoteUri;
        protected SipUri _remoteTarget;


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
            var r = InvitingRequest.CreateResponse(SipResponseCodes.x180_Ringing);
            r.To.Tag = _toTag;
            AddContactHeader(r);
            return r;
        }

        private void AddContactHeader(SipResponse response)
        {
            var contactHeader = new SipContactHeaderBuilder().WithSipUri(TestConstants.BobContactUri).Build();
            response.Contacts.Add(contactHeader);
        }

        protected SipResponse CreateOkResponse()
        {
            var r = InvitingRequest.CreateResponse(SipResponseCodes.x200_Ok);
            r.To.Tag = _toTag;
            AddContactHeader(r);
            return r;
        }

        public SipInviteClientDialogSpecificationBase()
        {
            _toTag = SipUtil.CreateTag();
            _fromTag = SipUtil.CreateTag();
            _callId = SipUtil.CreateCallId();
            _cSeq = 2;

            Sender = new Mock<ISipMessageSender>();
            Listener = new Mock<ISipListener>();
            DialogTable = new SipDialogTable();
            InvitingRequest = CreateInviteRequest();

            InviteCtx = new Mock<ISipClientTransaction>();
            InviteCtx.Setup((tx) => tx.Request).Returns(InvitingRequest);

            var tfb = new TimerFactoryStubBuilder();
           
            TimerFactory = tfb.Build();
        }

        protected SipResponse ReceivedResponse { get; set; }

        protected Mock<ISipClientTransaction> InviteCtx { get; set; }

        protected SipRequest InvitingRequest { get; set; }

        protected SipDialogTable DialogTable { get; set; }

        protected SipInviteClientDialog ClientDialog { get; set; }

        protected override void Given()
        {
            //setup the Request property of the inc inviteserver
            InvitingRequest = CreateInviteRequest();
            var tfb = new TimerFactoryStubBuilder();
            InviteCtx = new Mock<ISipClientTransaction>();
            InviteCtx.Setup((tx) => tx.Request).Returns(InvitingRequest);

            //create the dialog;
            ClientDialog = new SipInviteClientDialog(InviteCtx.Object,
                                                     DialogTable,
                                                     new SipHeaderFactory(),
                                                     new SipMessageFactory(), 
                                                     new SipAddressFactory(),
                                                     Sender.Object,
                                                     Listener.Object,
                                                     TestConstants.IpEndPoint1);

            GivenOverride();
        }

        protected virtual void GivenOverride()
        {

        }

        protected SipResponse CreateBusyHereResponse()
        {
            var r = InvitingRequest.CreateResponse(SipResponseCodes.x486_Busy_Here);
            r.To.Tag = _toTag;
            return r;
        }
    }

}