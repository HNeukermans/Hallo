using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.Sip.Util;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.UnitTest.Stubs;

namespace Hallo.UnitTest.Sip.SipClientDialogTests
{
    public class SipDialogSpecificationBase : Specification
    {
        protected int _onProcessResponseCount;
        protected FakeSipContextSource _contextSource;
        protected SipProvider _provider;
        protected SipRequest _inviteRequest;
        protected SipInviteClientDialog _dialog;
        protected string _fromTag;
        protected string _toTag;
        protected string _callId;
        protected SipStack _sipStack;
        protected SipResponseEvent _sipResponseEvent;
        protected SipResponse _response;
        protected SipInviteClientTransaction _inviteTransaction;


        protected SipRequest CreateInviteRequest()
        {
            var r = new SipRequestBuilder()
                .WithRequestLine(
                    new SipRequestLineBuilder().WithMethod(SipMethods.Invite).Build())
                .WithCSeq(
                    new SipCSeqHeaderBuilder().WithCommand(SipMethods.Invite).Build())
                .WithFrom(
                    new SipFromHeaderBuilder().WithTag(_fromTag).Build())
                .WithCallId(
                    new SipCallIdHeaderBuilder().WithValue(_callId).Build())
                .WithContacts(
                    new SipContactHeaderListBuilder()
                        .Add(new SipContactHeaderBuilder().WithSipUri(TestConstants.AliceContactUri).Build())
                        .Build())
                .Build();

            return r;
        }

        protected SipResponse CreateRingingResponse()
        {
            var r = _inviteRequest.CreateResponse(SipResponseCodes.x180_Ringing);
            r.To.Tag = _toTag;

            _inviteRequest.Contacts.ToList().ForEach(r.Contacts.Add);

            return r;
        }

        protected SipResponse CreateOkResponse()
        {
            var r = _inviteRequest.CreateResponse(SipResponseCodes.x200_Ok);
            r.To.Tag = _toTag;
            return r;
        }

        public SipDialogSpecificationBase()
        {
            _toTag = SipUtil.CreateTag();
            _fromTag = SipUtil.CreateTag();
            _callId = SipUtil.CreateCallId();
        }

        protected override void  Given()
        {
            _contextSource = new FakeSipContextSource();
            _sipStack = new SipStack();
            _provider = new SipProvider(_sipStack, _contextSource);
            _inviteRequest = CreateInviteRequest();
            BeforeCreateInviteTransaction();
            _inviteTransaction = _provider.CreateClientTransaction(_inviteRequest).As<SipInviteClientTransaction>();
            _dialog = _provider.CreateClientDialog(_inviteTransaction);
            _inviteTransaction.SendRequest();
            GivenOverride();
        }

        protected virtual void GivenOverride()
        {

        }

        protected virtual void BeforeCreateInviteTransaction()
        {

        }
    }
}