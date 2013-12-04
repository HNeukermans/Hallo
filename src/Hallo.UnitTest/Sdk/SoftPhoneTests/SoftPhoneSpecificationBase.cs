using System;
using System.Net;
using System.Threading;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sdk.SoftPhoneStates;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Util;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.UnitTest.Stubs;
using Moq;
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
        protected ManualResetEvent _waitingforInviteReceived = new ManualResetEvent(false);
        protected decimal _counter;
        protected internal IInternalSoftPhone _phone;
        protected bool _firedStateChanged;
        protected bool _firedIncomingCall;
        protected IPhoneCall _incomingCall;
        protected SipProvider _sipProvider1;
        protected TimerFactoryStub _timerFactory;
        protected ISoftPhoneStateProvider _stateProvider;
        protected SoftPhoneStateProxy _idleStateProxy;
        protected SoftPhoneStateProxy _ringingStateProxy;
        protected SoftPhoneStateProxy _waitforAckStateProxy;
        protected SoftPhoneStateProxy _establishedStateProxy;
        protected SoftPhoneStateProxy _waitForProvStateProxy;
        protected SoftPhoneStateProxy _waitForFinalStateProxy;
        protected SoftPhoneStateProxy _waitForCancelOkStateProxy;
        protected SoftPhoneStateProxy _waitForByeOkStateProxy;
        protected IPEndPoint _testClientUaEndPoint;
        protected SipUri _testClientUaUri;
        protected SipUri _phoneUaUri;
        protected IPEndPoint _phoneUaEndPoint;
        protected CallState? _callState;
        protected ManualResetEvent _waitingforOkReceived = new ManualResetEvent(false);
        protected ManualResetEvent _waitingforInviteProcessed = new ManualResetEvent(false);

        protected SoftPhoneSpecificationBase()
        {
            _testClientUaUri = TestConstants.EndPoint1Uri;
            _testClientUaEndPoint = TestConstants.IpEndPoint1;
            _phoneUaUri = TestConstants.EndPoint2Uri;
            _phoneUaEndPoint = TestConstants.IpEndPoint2;
            _timerFactory = new TimerFactoryStubBuilder().Build();
            _stateProvider = CreateStateProviderMock();
        }

        private ISoftPhoneStateProvider CreateStateProviderMock()
        {
            _idleStateProxy = new SoftPhoneStateProxy(new IdleState(), AfterPhoneProcessedRequest, AfterPhoneProcessedResponse, AfterInitialized);
            _ringingStateProxy = new SoftPhoneStateProxy(new RingingState(), AfterPhoneProcessedRequest, AfterPhoneProcessedResponse, AfterInitialized);
            _waitforAckStateProxy = new SoftPhoneStateProxy(new WaitForAckState(), AfterPhoneProcessedRequest, AfterPhoneProcessedResponse, AfterInitialized);
            _establishedStateProxy = new SoftPhoneStateProxy(new EstablishedState(), AfterPhoneProcessedRequest, AfterPhoneProcessedResponse, AfterInitialized);
            _waitForProvStateProxy = new SoftPhoneStateProxy(new WaitForProvisionalState(), AfterPhoneProcessedRequest, AfterPhoneProcessedResponse, AfterInitialized);
            _waitForFinalStateProxy = new SoftPhoneStateProxy(new WaitForFinalState(), AfterPhoneProcessedRequest, AfterPhoneProcessedResponse, AfterInitialized);
            _waitForCancelOkStateProxy = new SoftPhoneStateProxy(new WaitForCancelOkState(), AfterPhoneProcessedRequest, AfterPhoneProcessedResponse, AfterInitialized);
            _waitForByeOkStateProxy = new SoftPhoneStateProxy(new WaitForByeOkState(), AfterPhoneProcessedRequest, AfterPhoneProcessedResponse, AfterInitialized);

            Mock<ISoftPhoneStateProvider> mock = new Mock<ISoftPhoneStateProvider>();
            mock.Setup(s => s.GetIdle()).Returns(_idleStateProxy);
            mock.Setup(s => s.GetRinging()).Returns(_ringingStateProxy);
            mock.Setup(s => s.GetWaitForAck()).Returns(_waitforAckStateProxy);
            mock.Setup(s => s.GetEstablished()).Returns(_establishedStateProxy);
            mock.Setup(s => s.GetWaitProvisional()).Returns(_waitForProvStateProxy);
            mock.Setup(s => s.GetWaitFinal()).Returns(_waitForFinalStateProxy);
            mock.Setup(s => s.GetWaitCancelOk()).Returns(_waitForCancelOkStateProxy);
            mock.Setup(s => s.GetWaitByeOk()).Returns(_waitForByeOkStateProxy);
            return mock.Object;
        }

        protected virtual void AfterInitialized(IInternalSoftPhone softPhone)
        {
           
        }

        protected virtual void AfterPhoneProcessedResponse(IInternalSoftPhone softPhone, SipResponseEvent responseEvent)
        {
            
        }

        protected virtual void AfterPhoneProcessedRequest(IInternalSoftPhone softPhone, SipRequestEvent requestEvent)
        {
           
        }

        protected override void Given()
        {
            //create invite that is addresses to the phone's sipuri
            _invite = CreateInviteRequest(_testClientUaUri, _phoneUaUri);
            //create phone that is located at IpEndPoint2
            var phoneCs = new FakeSipContextSource(_phoneUaEndPoint);

            _network = new FakeNetwork();
            _sipProvider1 = new SipProvider(new SipStack(), phoneCs);

            _phone = new SoftPhone(_sipProvider1, new SipMessageFactory(), new SipHeaderFactory(), new SipAddressFactory(), _stateProvider, _timerFactory, new SipListeningPoint(_phoneUaEndPoint));
            phoneCs.AddToNetwork(_network);
            _network.AddReceiver(_testClientUaEndPoint, OnTestClientUaReceive);
            _phone.InternalStateChanged += new EventHandler<EventArgs>(_calleePhone_InternalStateChanged);     
            _phone.IncomingCall += new EventHandler<VoipEventArgs<IPhoneCall>>(_calleePhone_IncomingCall);
            _phone.Start();

            GivenOverride();
        }

        protected virtual void _calleePhone_InternalStateChanged(object sender, EventArgs e) 
        { }

        /// <summary>
        /// creates a ack. this is to be sent by the testclient UA
        /// </summary>
        /// <param name="invite"></param>
        /// <param name="ringing"></param>
        /// <returns></returns>
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


        /// <summary>
        /// creates a bye. this is to be sent by the testclient UA
        /// </summary>
        /// <param name="invite"></param>
        /// <param name="ringing"></param>
        /// <returns></returns>
        protected SipRequest CreateByeRequest(SipRequest invite, SipResponse ringing)
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

            var cseqHeader = headerFactory.CreateSCeqHeader(SipMethods.Bye, localSequenceNr+1);
            var toAddress = addressFactory.CreateAddress(null, remoteUri);
            var toHeader = headerFactory.CreateToHeader(toAddress, remoteTag);
            var fromAddress = addressFactory.CreateAddress(null, localUri);
            var fromHeader = headerFactory.CreateFromHeader(fromAddress, localTag);
            var callIdheader = headerFactory.CreateCallIdHeader(callId);
            var viaHeader = new SipViaHeaderBuilder().WithSentBy(_testClientUaEndPoint).Build();
            var requestUri = remoteUri.Clone();

            var maxForwardsHeader = headerFactory.CreateMaxForwardsHeader();
            var request = messageFactory.CreateRequest(
                requestUri,
                SipMethods.Bye,
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

        /// <summary>
        /// creates a ringing. this is to be sent by the testclient UA
        /// </summary>
        protected SipResponse CreateRingingResponse(SipRequest receivedInvite, string toTag)
        {
            var ringing = receivedInvite.CreateResponse(SipResponseCodes.x180_Ringing);
            ringing.To.Tag = toTag;
            var contactUri = _phone.AddressFactory.CreateUri("", _phoneUaEndPoint.ToString());
            ringing.Contacts.Add(_phone.HeaderFactory.CreateContactHeader(contactUri));
            
            return ringing;
        }

        /// <summary>
        /// creates a ringing. this is to be sent by the testclient UA
        /// </summary>
        protected SipResponse CreateOkResponse(SipRequest receivedInvite, string toTag)
        {
            var r = receivedInvite.CreateResponse(SipResponseCodes.x200_Ok);
            r.To.Tag = toTag;
            var contactUri = _phone.AddressFactory.CreateUri("", _phoneUaEndPoint.ToString());
            r.Contacts.Add(_phone.HeaderFactory.CreateContactHeader(contactUri));

            return r;
        }

        protected SipResponse CreateResponse(SipRequest receivedInvite, string toTag, string responseCode)
        {
            var r = receivedInvite.CreateResponse(responseCode);
            r.To.Tag = toTag;
            var contactUri = _phone.AddressFactory.CreateUri("", _phoneUaEndPoint.ToString());
            r.Contacts.Add(_phone.HeaderFactory.CreateContactHeader(contactUri));

            return r;
        }

        protected virtual void _calleePhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        { }


        protected virtual void _calleePhone_StateChanged(object sender, VoipEventArgs<SoftPhoneState> e) 
        { }
        

        protected virtual void GivenOverride()
        {

        }

        protected SipRequest CreateCancelRequest(SipRequest request)
        {
            var hf = new SipHeaderFactory();
            var mf = new SipMessageFactory();
            var requestUri = request.RequestLine.Uri.Clone();
            var callIdheader = (SipCallIdHeader)request.CallId.Clone();
            var cseqHeader = new SipHeaderFactory().CreateSCeqHeader(SipMethods.Cancel, request.CSeq.Sequence);
            var fromHeader = (SipFromHeader)request.From.Clone();
            var toHeader = (SipToHeader)request.To.Clone();
            var viaHeader = (SipViaHeader)request.Vias.GetTopMost().Clone();
            var maxForwardsHeader = hf.CreateMaxForwardsHeader();
            var cancelRequest = mf.CreateRequest(
                requestUri,
                SipMethods.Cancel,
                callIdheader,
                cseqHeader,
                fromHeader,
                toHeader,
                viaHeader,
                maxForwardsHeader);

            foreach (var route in request.Routes)
            {
                cancelRequest.Routes.Add((SipRouteHeader)route.Clone());
            }

            return cancelRequest;
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
                .WithRecordRoutes(new SipHeaderList<SipRecordRouteHeader>())
                .Build();

            return r;
        }

        protected abstract void OnTestClientUaReceive(SipContext sipContext);

        

    }

}
