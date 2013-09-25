using System;
using FluentAssertions;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using Hallo.Sip.Util;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Sip.SipClientDialogTests;
using Hallo.UnitTest.Stubs;
using Hallo.Util;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipServerDialogTests
{
    public class SipDialogSpecificationBase : Specification
    {
        protected int _onProcessResponseCount;
        protected FakeSipContextSource _contextSource;
        protected SipProvider _provider;
        protected SipRequest _inviteRequest;
        protected SipInviteServerDialog _dialog;
        protected string _fromTag;
        protected string _toTag;
        protected string _callId;
        protected SipStack _sipStack;
        protected SipResponseEvent _responseEvent;
        protected SipRequestEvent _requestEvent;
        protected SipResponse _response;
        protected ISipServerTransaction _inviteTransaction;
        
        protected SipRequest CreateInviteRequest()
        {
            var r = new SipRequestBuilder()
                .WithRequestLine(
                    new SipRequestLineBuilder().WithMethod(SipMethods.Invite).Build())
                .WithCSeq(
                    new SipCSeqHeaderBuilder().WithCommand(SipMethods.Invite).Build())
                .WithFrom(
                    new SipFromHeaderBuilder().WithTag(_fromTag).Build())
                .WithTo(
                    new SipToHeaderBuilder().WithTag(null).Build())
                .WithCallId(
                    new SipCallIdHeaderBuilder().WithValue(_callId).Build())
                .Build();

            return r;
        }

        protected SipResponse CreateRingingResponse()
        {
            var r = _inviteRequest.CreateResponse(SipResponseCodes.x180_Ringing);
            r.To.Tag = _toTag;
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
            //_inviteRequest = CreateInviteRequest();
            _provider.AddSipListener(new StubSipListener(onProcessRequest: OnProcessRequest, onProcessResponse: OnProcessResponse));
            //_response = CreateRingingResponse();
            //var c = new SipContext();
            //c.Response = _response;
            //_contextSource.FireNewContextReceivedEvent(c);

            //_inviteTransaction = _provider.CreateServerTransaction(_inviteRequest).As<SipInviteServerTransaction>();
            //_dialog = _provider.CreateServerDialog(_inviteTransaction);
            //_inviteTransaction.SendResponse();
            GivenOverride();
        }

        private void OnProcessResponse(SipResponseEvent responseEvent)
        {
            _responseEvent = responseEvent;
            OnProcessResponseOverride();
        }

        public void OnProcessRequest(SipRequestEvent requestEvent)
        {
            _requestEvent = requestEvent;
            OnProcessRequestOverride();
        }

        public virtual void  OnProcessRequestOverride()
        {

        }

        public virtual void OnProcessResponseOverride()
        {

        }
       

        protected virtual void GivenOverride()
        {

        }

        protected virtual void BeforeCreateInviteTransaction()
        {

        }

        protected void GoFromEarlyToConfirmedState()
        {
            Assert.Fail();
            //Check.IsTrue(_provider.GetDialog(_inviteTransaction).State == DialogState.Early, "The dialog is not in Early state.");

            /*go to confirmed state*/
            var okResponse = _inviteTransaction.Request.CreateResponse(SipResponseCodes.x200_Ok);
            _inviteTransaction.SendResponse(okResponse);

            //Check.IsTrue(_provider.GetDialog(_inviteTransaction).State == DialogState.Confirmed, "The dialog is not in Confirmed state.");

        }

        protected void GoToEarlyState()
        {
            Check.IsTrue(_inviteTransaction == null, "_inviteTransaction must be null. When going into Early State a new tx must be created.");

            /*go to early state*/
            _inviteTransaction = _provider.CreateServerTransaction(_requestEvent.Request);
            _dialog = _provider.CreateServerDialog(_inviteTransaction);
            var response = CreateRingingResponse();
            _inviteTransaction.SendResponse(response);
            Assert.Fail();
            //Check.IsTrue(_provider.GetDialog(_inviteTransaction).State == DialogState.Early, "The dialog is not in Early state.");
        }
    }
}