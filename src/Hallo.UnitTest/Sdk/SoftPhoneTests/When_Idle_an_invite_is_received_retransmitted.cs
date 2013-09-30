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
using Hallo.Sdk.SoftPhoneStates;
using Moq;

namespace Hallo.UnitTest.Sdk
{
    internal class InvocationCounter : ISoftPhoneState 
    {
        ISoftPhoneState _state;

        public InvocationCounter(ISoftPhoneState state) 
        {
            _state = state;
        }

        public int ProcessRequestCounter { get; set; }

        public int ProcessResponseCounter { get; set; }

        public void Initialize(IInternalSoftPhone softPhone)
        {
            _state.Initialize(softPhone);
        }

        public void ProcessRequest(IInternalSoftPhone softPhone, SipRequestEvent requestEvent)
        {
            ProcessRequestCounter++;

            _state.ProcessRequest(softPhone, requestEvent);
        }

        public void ProcessResponse(IInternalSoftPhone softPhone, SipResponseEvent responseEvent)
        {
            ProcessResponseCounter++;

            _state.ProcessResponse(softPhone, responseEvent);
        }

        public SoftPhoneState StateName
        {
            get { throw new NotImplementedException(); }
        }
    } 


    public class When_Idle_an_invite_is_retransmitted : Specification
    {
        private FakeNetwork _network;
        private SipRequest _invite;
        //TODO:test invite no from tag
        //TODO:test invite no contact header

        ManualResetEvent _wait = new ManualResetEvent(false);
        private decimal _counter;
        private ISoftPhone _calleePhone;
        private bool _firedStateChanged;
        private bool _firedIncomingCall;
        private IPhoneCall _incomingCall;
        private InvocationCounter _idleStateCountable;
        
        protected override void Given()
        {
            //create invite.
            _invite = CreateInviteRequest(TestConstants.EndPoint1Uri, TestConstants.EndPoint2Uri);

            //create phone
            var cs1 = new FakeSipContextSource(TestConstants.IpEndPoint2);

            _idleStateCountable = new InvocationCounter(new IdleState());

            //create IdleMock to verify # invocations
            Mock<ISoftPhoneStateProvider> softPhoneStateProviderMock = new Mock<ISoftPhoneStateProvider>();
            softPhoneStateProviderMock.Setup(s => s.GetIdle()).Returns(_idleStateCountable);
            _network = new FakeNetwork();
            var sipProvider1 = new SipProvider(new SipStack(), cs1);
            _calleePhone = new SoftPhone(sipProvider1, new SipMessageFactory(), new SipHeaderFactory(), new SipAddressFactory(), softPhoneStateProviderMock.Object);
            cs1.AddToNetwork(_network);
            _network.AddReceiver(TestConstants.IpEndPoint1, OnReceiveFromSoftPhone);
            _calleePhone.IncomingCall += (s, e) => 
            { 
                _incomingCall = e.Item;
                _firedIncomingCall = true;
            };
            _calleePhone.StateChanged += _calleePhone_StateChanged;
            _calleePhone.Start();
        }

        void _calleePhone_StateChanged(object sender, VoipEventArgs<SoftPhoneState> e)
        {
            _firedStateChanged = true;
        }

        private void OnReceiveFromSoftPhone(SipContext sipContext)
        {
            _counter++;
            if( _counter == 2) _wait.Set();

            /*continue test execution*/
            //_wait.Set(); move to statechanged, as this is the last event in code.
        } 

        protected override void When()
        {
            _network.SendTo(SipFormatter.FormatMessage(_invite), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _network.SendTo(SipFormatter.FormatMessage(_invite), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);

             _wait.WaitOne(TimeSpan.FromSeconds(1));
            // _wait.WaitOne(); /*debug*/
        }
        
        [Test]
        public void Expect_the_IdleState_ProcessRequest_invocations_to_be_1()
        {
            _idleStateCountable.ProcessRequestCounter.Should().Be(1);
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
    }
    
}