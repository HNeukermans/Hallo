using System;
using System.Net;
using Hallo.Sdk.Commands;
using Hallo.Sdk.SoftPhoneStates;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Util;
using Hallo.Util;
using Hallo.Sip.Stack.Transactions.InviteClient;

namespace Hallo.Sdk
{
    internal class SoftPhone : IInternalSoftPhone, ISipListener
    {
        private ISipProvider _provider = null;
        private readonly SipMessageFactory _messageFactory;
        private readonly SipHeaderFactory _headerFactory;
        private readonly SipAddressFactory _addressFactory;
        private readonly ICommandFactory _commandFactory;

        private RingingState _ringingState = new RingingState();
        private IdleState _idleState = new IdleState();
        
        //the registered phoneline
        private IPhoneLine _phoneLine;
        private bool _isRunning;

        //the started phonecall
        private IPhoneCall _pendingPhoneCall;
        private int _counter;
        private ISoftPhoneStateProvider _stateProvider;

        public ISoftPhoneState InternalState { get; private set; }
        public ISoftPhoneStateProvider StateProvider { get { return _stateProvider; } }

        internal SoftPhone(ISipProvider provider, SipMessageFactory messageFactory, SipHeaderFactory headerFactory,
            SipAddressFactory addressFactory, ISoftPhoneStateProvider stateProvider)
        {
            _provider = provider;
            _messageFactory = messageFactory;
            _headerFactory = headerFactory;
            _addressFactory = addressFactory;
            _stateProvider = stateProvider;

            InternalState = _stateProvider.GetIdle();
            InternalState.Initialize(this);
        }

        public IPhoneLine CreatePhoneLine(SipAccount account)
        {
            Check.Require(account, "account");
            Check.IsTrue(_isRunning, "Failed to create phoneLine. The Phone must be started first.");

            /*for now unregistered phoneline*/
            return new PhoneLine(account, new Command<IPhoneLine>(OnRegisterPhoneLine));
        }
        
        public IPhoneCall CreateCall()
        {
            //Check.Require(to, "to");
            //Check.Require(_phoneLine, "Failed to create a call. The Phone requires phoneLine in 'Registered' state.");
            //Check.IsTrue(_phoneLine != null && _phoneLine.State == PhoneLineState.Registered, "Failed to create a call. The Phone requires phoneLine in 'Registered' state.");

            return new PhoneCall(this, false, new Command<IPhoneCall>(OnPhoneCallStarted));
        }
        
        private void OnRegisterPhoneLine(IPhoneLine phoneLine)
        {
            Check.IsTrue(_isRunning, "Failed to register phone line. The phone must be started first.");
            Check.Require(phoneLine, "phoneLine");
            /*simplification. Up o now we only support IsRegistrationRequired = false*/
            //Check.IsTrue(!phoneLine.SipAccount, "Registration is not supported. RegistrationRequired must be false.");
            //_phoneLine = phoneLine;
            throw new NotSupportedException("Failed to register phone. Phone regsitration is not supported.");
        }

        private void OnPhoneCallStarted(IPhoneCall phoneCall)
        {
            var idleState = InternalState as IdleState;

            Check.Require(phoneCall, "phoneCall");
            Check.Require(idleState, "Failed to start the call. The phone has a call 'PENDING'. You need to terminate the 'PENDING' call.");
            Check.IsTrue(_isRunning, "Failed to start the call. The phone must be started first.");
            
            _pendingPhoneCall = phoneCall;

            StartCall(phoneCall.GetToUri());
        }

        public event EventHandler<VoipEventArgs<IPhoneCall>> IncomingCall;

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            ValidateSipSchema(requestEvent.Request);

            ////not sure if this is the right place?
            //if (State is IdleState)
            //{
            //    throw new SipException(SipResponseCodes.x486_Busy_Here);
            //}

            InternalState.ProcessRequest(this, requestEvent);
        }

        /// <summary>
        /// validates the sip schema of the request.
        /// </summary>
        /// <param name="request"></param>
        private void ValidateSipSchema(SipRequest request)
        {
            if (request.From.SipUri.HasNamedHost())
                throw new SipException(ExceptionMessage.NamedHostsAreNotSupported);

            if (request.RequestLine.Method == SipMethods.Invite)
            {
                /*13.3.1 Processing of the INVITE*/

                /*If the request is an INVITE that contains an Expires header == TODO*/

                /*according to rfc verify that an invite has 1 contact. Adddtional to rfc, verify that the contact is not a named host.*/
                if (request.Contacts.Count != 1)
                {
                    throw new SipException(SipResponseCodes.x400_Bad_Request, "An 'INVITE' request MUST contain 1 Contact header.");
                }

                if (request.Contacts[0].SipUri.HasNamedHost())
                {
                    throw new SipException(SipResponseCodes.x400_Bad_Request, "The Sipuri-host specified in the contact header must be a numeric ipaddress.");
                }

                //var from = request.Contacts[0].SipUri.GetHostAndPort();

                /*fire incoming call*/
                //var pc = new PhoneCall(this, _provider, _messageFactory, _headerFactory, _addressFactory, from, PhoneCallDirection.Incoming);
                //pc.Start(); /**/
                //pc.State = CallState.Ringing;
                //IncomingCall(this, new VoipEventArgs<IPhoneCall>(pc));

                //StartRinging();

                ///*start the phone to ring + fire ringing*/
                //pc.InternalSendRinging();
                //pc.FireCallStateChanged(CallState.Ringing);
            }
        }

        public void StartRinging()
        {
            //State = SoftPhoneState.Ringing;
            
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {

        }

        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {

        }

        public SipAccount Account
        {
            get { throw new NotImplementedException(); }
        }

        public void StartCall(SipUri calleeUri)
        {
            var thisUri = AddressFactory.CreateUri(string.Empty, SipProvider.ListeningPoint.ToString());

            var requestUri = calleeUri;
            var toAddress = AddressFactory.CreateAddress(string.Empty, calleeUri);
            var fromAddress = AddressFactory.CreateAddress(string.Empty, thisUri);
            var toHeader = HeaderFactory.CreateToHeader(toAddress);
            var fromHeader = HeaderFactory.CreateFromHeader(fromAddress, SipUtil.CreateTag());
            var cseqHeader = HeaderFactory.CreateSCeqHeader(SipMethods.Invite, MessageCounter++);
            var callIdheader = HeaderFactory.CreateCallIdHeader(SipUtil.CreateCallId());
            var viaHeader = HeaderFactory.CreateViaHeader(SipProvider.ListeningPoint.Address,
                                                          SipProvider.ListeningPoint.Port, SipConstants.Udp,
                                                          SipUtil.CreateBranch());
            var maxForwardsHeader = HeaderFactory.CreateMaxForwardsHeader(1);
            var request = MessageFactory.CreateRequest(
                requestUri,
                SipMethods.Invite,
                callIdheader,
                cseqHeader,
                fromHeader,
                toHeader,
                viaHeader,
                maxForwardsHeader);

            /*add contactheader*/
            var contactUri = AddressFactory.CreateUri(thisUri.User, viaHeader.SentBy.ToString());
            var contactHeader = HeaderFactory.CreateContactHeader(contactUri);
            request.Contacts.Add(contactHeader);

            var clientTransaction = SipProvider.CreateClientTransaction(request);
            var dialog = SipProvider.CreateClientDialog(clientTransaction as SipInviteClientTransaction);
            clientTransaction.SendRequest();
            ChangeState(_stateProvider.GetRinging());
        }
       
        public InviteInfo PendingInvite { get; set; }
    
        public ITimer RingingTimer { get; set; }

        public ISipProvider SipProvider
        {
            get { return _provider; }
        }

        public void ChangeState(ISoftPhoneState state)
        {
            Check.IsTrue(!state.Equals(InternalState), string.Format("Can not change state to '{0}'. The phone is already in '{0}' state",  this.InternalState.StateName));

            state.Initialize(this);
            InternalState = state;
            StateChanged(this, new VoipEventArgs<SoftPhoneState>(InternalState.StateName));
        }

        public SipAddressFactory AddressFactory { get { return _addressFactory; } }
        
        public SipHeaderFactory HeaderFactory { get { return _headerFactory; } }

        public SipMessageFactory MessageFactory { get { return _messageFactory; } }
        
        public ICommandFactory CommandFactory
        {
            get { return _commandFactory; }
        }

        public void Start()
        {
            _isRunning = true;
            _provider.AddSipListener(this);
            _provider.Start();
        }

        public event EventHandler<VoipEventArgs<SoftPhoneState>> StateChanged = delegate {};
       
        public int MessageCounter { get; set; }
   
        public IPhoneLine RegisteredPhoneLine 
        {
            get { return _phoneLine; }
        }

        public SoftPhoneState CurrentState
        {
            get { return InternalState.StateName; }
        }
        
        public void RaiseIncomingCall(SipUri from)
        {
            _pendingPhoneCall = new PhoneCall(this, true, from, new Command(OnPhoneCallAnswered), new Command(OnPhoneCallRejected));
            IncomingCall(this, new VoipEventArgs<IPhoneCall>(_pendingPhoneCall));
        }

        private void OnPhoneCallAnswered()
        {

        }

        private void OnPhoneCallRejected() 
        { 

        }      
    }

    internal interface ISoftPhoneStateProvider 
    {
        ISoftPhoneState GetRinging();
        ISoftPhoneState GetIdle();       
    }

    internal class SoftPhoneStateProvider : ISoftPhoneStateProvider
    {
        public readonly ISoftPhoneState _ringing = new RingingState();
        public readonly ISoftPhoneState _idle = new IdleState();


        public ISoftPhoneState GetRinging() 
        {
            return _ringing;  
        }

        public ISoftPhoneState GetIdle()
        {
            return _idle;
        }
    }
}