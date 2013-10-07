using System;
using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Util;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;
using Hallo.Component;
using System.Linq;
using Hallo.Sip.Headers;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal abstract class SoftPhoneSpecificationBase : Specification
    {
        protected FakeNetwork _network;
        protected SipRequest _invite;
        protected SipRequest _ack;
        protected ManualResetEvent _wait = new ManualResetEvent(false);
        protected decimal _counter;
        protected internal IInternalSoftPhone _calleePhone;
        protected bool _firedStateChanged;
        protected bool _firedIncomingCall;
        protected IPhoneCall _incomingCall;
        protected SipProvider _sipProvider1;
        protected TimerFactoryStub _timerFactory;
        protected ISoftPhoneStateProvider _stateProvider;

        protected SoftPhoneSpecificationBase()
        {
            _timerFactory = new TimerFactoryStubBuilder().Build();
            _stateProvider = new SoftPhoneStateProvider();
        }

        protected override void Given()
        {
            //create invite.
            _invite = CreateInviteRequest(TestConstants.EndPoint1Uri, TestConstants.EndPoint2Uri);
            //create phone
            var cs1 = new FakeSipContextSource(TestConstants.IpEndPoint2);

            _network = new FakeNetwork();
            _sipProvider1 = new SipProvider(new SipStack(), cs1);

            _calleePhone = new SoftPhone(_sipProvider1, new SipMessageFactory(), new SipHeaderFactory(), new SipAddressFactory(), _stateProvider, _timerFactory);
            cs1.AddToNetwork(_network);
            _network.AddReceiver(TestConstants.IpEndPoint1, OnReceive);
            _calleePhone.StateChanged += new EventHandler<VoipEventArgs<SoftPhoneState>>(_calleePhone_StateChanged);
            _calleePhone.IncomingCall += new EventHandler<VoipEventArgs<IPhoneCall>>(_calleePhone_IncomingCall);
            _calleePhone.Start();

            GivenOverride();
        }

        protected SipRequest CreateAckRequest(SipRequest invite, SipResponse ringing)
        {
            var addressFactory = new SipAddressFactory();
            var headerFactory = new SipHeaderFactory();
            var messageFactory = new SipMessageFactory();

            var localSequenceNr = invite.CSeq.Sequence;
            /*_remoteSequenceNr remains empty */
            var callId = invite.CallId.Value;
            var localTag = invite.From.Tag;
            var remoteUri = invite.To.SipUri;
            var localUri = invite.From.SipUri;

            var remoteTag = ringing.To.Tag;
            var remoteTarget = ringing.Contacts.GetTopMost().SipUri;
            var routeSet = ringing.RecordRoutes.ToList();//refuse looseroutin-less recordroutes
            routeSet.Reverse();

            var cseqHeader = headerFactory.CreateSCeqHeader(SipMethods.Ack, localSequenceNr);
            var toAddress = addressFactory.CreateAddress(null, remoteUri);
            var toHeader = headerFactory.CreateToHeader(toAddress, remoteTag);
            var fromAddress = addressFactory.CreateAddress(null, localUri);
            var fromHeader = headerFactory.CreateFromHeader(fromAddress, localTag);
            var callIdheader = headerFactory.CreateCallIdHeader(callId);
            var viaHeader = invite.Vias.GetTopMost();
            var requestUri = remoteUri.Clone();

            var maxForwardsHeader = headerFactory.CreateMaxForwardsHeader();
            var request = messageFactory.CreateRequest(
                requestUri,
                SipMethods.Ack,
                callIdheader,
                cseqHeader,
                fromHeader,
                toHeader,
                viaHeader,
                maxForwardsHeader);

            foreach (var route in routeSet)
            {
                request.Routes.Add(new SipRouteHeader() { SipUri = route.SipUri, Parameters = route.Parameters });
            }

            return request;
        }

        protected abstract void _calleePhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e);
        

        protected abstract void _calleePhone_StateChanged(object sender, VoipEventArgs<SoftPhoneState> e);
        

        protected virtual void GivenOverride()
        {

        }

        protected SipRequest CreateInviteRequest(SipUri from, SipUri to)
        {
            var r = new SipRequestBuilder()
                .WithRequestLine(
                    new SipRequestLineBuilder().WithMethod(SipMethods.Invite).Build())
                .WithCSeq(
                    new SipCSeqHeaderBuilder().WithCommand(SipMethods.Invite).WithSequence(1).Build())
                .WithFrom(
                    new SipFromHeaderBuilder().WithSipUri(from).WithTag(SipUtil.CreateTag()).Build())
                .WithTo(
                    new SipToHeaderBuilder().WithSipUri(to).WithTag(null).Build())
                .WithCallId(
                    new SipCallIdHeaderBuilder().WithValue(SipUtil.CreateCallId()).Build())
                .WithContacts(
                    new SipContactHeaderListBuilder()
                        .Add(new SipContactHeaderBuilder().WithSipUri(from).Build())
                        .Build())
                .Build();

            return r;
        }

        protected abstract void OnReceive(SipContext sipContext);

        

    }

}
