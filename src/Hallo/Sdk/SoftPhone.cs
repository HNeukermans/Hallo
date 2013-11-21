using System;
using System.Net;
using Hallo.Sdk.Commands;
using Hallo.Sdk.SoftPhoneStates;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Util;
using Hallo.Util;
using Hallo.Sip.Stack.Transactions.InviteClient;
using NLog;

namespace Hallo.Sdk
{
    internal class SoftPhone : IInternalSoftPhone, ISipListener, IExceptionHandler
    {
        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private ISipProvider _provider = null;
        private readonly SipMessageFactory _messageFactory;
        private readonly SipHeaderFactory _headerFactory;
        private readonly SipAddressFactory _addressFactory;
        //private readonly ICommandFactory _commandFactory;

        private RingingState _ringingState = new RingingState();
        private IdleState _idleState = new IdleState();
        
        //the registered phoneline
        private IPhoneLine _phoneLine;
        private bool _isRunning;

        //the started phonecall
        private IInternalPhoneCall _pendingPhoneCall;
        private int _counter;
        private readonly ISoftPhoneStateProvider _stateProvider;
        private readonly ITimerFactory _timerFactory;
        public ISoftPhoneState InternalState { get; private set; }
        public ISoftPhoneStateProvider StateProvider { get { return _stateProvider; } }

        public bool IsRunning { get { return _isRunning; } }

        internal SoftPhone(ISipProvider provider, SipMessageFactory messageFactory, SipHeaderFactory headerFactory,
            SipAddressFactory addressFactory, ISoftPhoneStateProvider stateProvider, ITimerFactory timerFactory)
        {
            _provider = provider;
            _messageFactory = messageFactory;
            _headerFactory = headerFactory;
            _addressFactory = addressFactory;
            _stateProvider = stateProvider;
            _timerFactory = timerFactory;

            InternalState = _stateProvider.GetIdle();
            InternalState.Initialize(this);
            RetransmitRingingTimer = _timerFactory.CreateRingingTimer(OnRetransmitRinging);
            EndWaitForAckTimer = _timerFactory.CreateInviteCtxTimeOutTimer(OnWaitForAckTimeOut);

            if(_logger.IsDebugEnabled) _logger.Debug("Initialized.");
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
            return new PhoneCall(this, false, new Command<IInternalPhoneCall>(OnPhoneCallStarted), new Command(OnPhoneCallStopped));
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
        
        public event EventHandler<VoipEventArgs<IPhoneCall>> IncomingCall = delegate { };

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            string method = requestEvent.Request.RequestLine.Method;

            _logger.Debug("Processing request: {0} ...", method);

            ValidateSipSchema(requestEvent.Request);

            ////not sure if this is the right place?
            //if (State is IdleState)
            //{
            //    throw new SipException(SipResponseCodes.x486_Busy_Here);
            //}

            InternalState.ProcessRequest(this, requestEvent);

            _logger.Debug("Request processed.");
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
            string responseCode = responseEvent.Response.StatusLine.ResponseCode;

            _logger.Debug("Processing response: {0} ...", responseCode);

            InternalState.ProcessResponse(this, responseEvent);

            _logger.Debug("Response processed.");
        }

        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {

        }

        public SipAccount Account
        {
            get { throw new NotImplementedException(); }
        }
        
        public InviteInfo PendingInvite 
        { 
            get;
            set;
        }
    
        public ITimer RetransmitRingingTimer { get; set; }
        public ITimer EndWaitForAckTimer{ get; set; }

        public ISipProvider SipProvider
        {
            get { return _provider; }
        }

        public void ChangeState(ISoftPhoneState state)
        {
            Check.IsTrue(!state.Equals(InternalState), string.Format("Can not change state to '{0}'. The phone is already in '{0}' state",  this.InternalState.GetType().Name));

            InternalState.Terminate(this);

            state.Initialize(this);

            InternalState = state;
            
            state.AfterInitialize(this);
            /*fire internal event*/
            InternalStateChanged(this, new EventArgs());
        }

        public SipAddressFactory AddressFactory { get { return _addressFactory; } }
        
        public SipHeaderFactory HeaderFactory { get { return _headerFactory; } }

        public SipMessageFactory MessageFactory { get { return _messageFactory; } }
        
        //public ICommandFactory CommandFactory
        //{
        //    get { return _commandFactory; }
        //}

        public void Start()
        {
            _isRunning = true;
            _provider.AddSipListener(this);
            _provider.AddExceptionHandler(this);
            _provider.Start();
        }

        public void Stop()
        {
            _isRunning = false;
            _provider.Stop();
        }

        public event EventHandler<VoipEventArgs<SoftPhoneState>> StateChanged = delegate {};

        public event EventHandler<EventArgs> InternalStateChanged = delegate { };

        public int MessageCounter { get; set; }
   
        public IPhoneLine RegisteredPhoneLine 
        {
            get { return _phoneLine; }
        }

        public SoftPhoneState CurrentState
        {
            get { return SoftPhoneState.Idle; } // InternalState.StateName; }
        }

        private void OnWaitForAckTimeOut()
        {
            if (InternalState != _stateProvider.GetWaitForAck())
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Wait_For_Ack Timeout ignored due to race condition. OnWaitForAckTimeOut was called, but the phone is currently not in 'WAITFORACK' state.");
                return;
            }
            
            if (_logger.IsInfoEnabled) _logger.Info("ACK has not been received after 64 * T1. Sending Bye and terminating dialog...");

           /*ack not been received after 64 * T1 end dialog. TODO:terminate session*/
           var bye = PendingInvite.Dialog.CreateRequest(SipMethods.Bye);

           var ctx = _provider.CreateClientTransaction(bye);
                      
           /*send bye in-dialog*/
           PendingInvite.Dialog.SendRequest(ctx);

           PendingInvite.Dialog.Terminate();

           _pendingPhoneCall.RaiseCallErrorOccured(CallError.WaitForAckTimeOut);

           if (_logger.IsInfoEnabled) _logger.Info("Transitioning back to idle state...");
            
           ChangeState(_stateProvider.GetIdle());          
        }

        public void RaiseIncomingCall()
        {
            Check.Require(PendingInvite, "PendingInvite");

            var from = PendingInvite.OriginalRequest.From.SipUri;

            _pendingPhoneCall = new PhoneCall(this, true, from, new Command(OnPhoneCallAccepted), new Command(OnPhoneCallRejected), new Command(OnPhoneCallStopped));
            IncomingCall(this, new VoipEventArgs<IPhoneCall>(_pendingPhoneCall));
        }

        public IInternalPhoneCall PendingCall
        {
            get { return _pendingPhoneCall; }
        }

        private void OnPhoneCallAccepted()
        {
            Check.IsTrue(_isRunning, "Failed to accept the call. The phone can only accept calls while in 'RUNNING' state.");
            Check.Require(_pendingPhoneCall, "The phone has no call pending.");
            Check.IsTrue(_pendingPhoneCall.IsIncoming, "Failed to accept the call. Only incoming calls can be accepted.");
            Check.IsTrue(InternalState == _stateProvider.GetRinging(),
                string.Format("Can not accept the phonecall. The phonecall can only be accepted while the phone is in 'RINGING state'. Currentstate: '{0}'", CurrentState));

            if (_logger.IsDebugEnabled) _logger.Debug("accepting phonecall...");

            var okResponse = PendingInvite.OriginalRequest.CreateResponse(SipResponseCodes.x200_Ok);
            /*be friendly. Adding tag and contacts to the OK is not required.*/
            okResponse.To.Tag = PendingInvite.RingingResponse.To.Tag; 
            PendingInvite.RingingResponse.Contacts.ToList().ForEach(okResponse.Contacts.Add);
        
            PendingInvite.InviteServerTransaction.SendResponse(okResponse);

            ChangeState(_stateProvider.GetWaitForAck());

            if (_logger.IsDebugEnabled) _logger.Debug("Phonecall accepted.");

        }

        private void OnPhoneCallRejected() 
        {
            throw new NotSupportedException(); //TODO: support it.
            Check.IsTrue(_isRunning, "Failed to reject the call. The phone can only reject calls while in 'RUNNING' state.");
            Check.Require(_pendingPhoneCall, "The phone has no call pending.");
            Check.IsTrue(InternalState == _stateProvider.GetRinging(),
               string.Format("Can not reject the phonecall. The phonecall can only be rejected while in the phone is in 'RINGING state'. Currentstate: '{0}'", InternalState.GetType().Name));

            if (_logger.IsDebugEnabled) _logger.Debug("Rejecting phonecall...");

            var rejectResponse = PendingInvite.OriginalRequest.CreateResponse(SipResponseCodes.x486_Busy_Here);
            PendingInvite.InviteServerTransaction.SendResponse(rejectResponse);

            if (_logger.IsDebugEnabled) _logger.Debug("Phonecall rejected.");
        }

        private void OnPhoneCallStopped()
        {
            Check.IsTrue(_isRunning, "Failed to stop the call. The phone can only stop calls while in 'RUNNING' state.");
            Check.Require(_pendingPhoneCall, "The phone has no call pending.");
            Check.IsTrue(InternalState != _stateProvider.GetIdle(), "Failed to stop the call. The phone can only stop calls while not in 'IDLE' state. CurrentState: 'IDLE'");

            if (_logger.IsInfoEnabled) _logger.Info("Stopping phonecall... DebugInfo: IsIncoming: '{0}', InternalState: '{1}'", _pendingPhoneCall.IsIncoming, InternalState.GetType().Name);
            
            if (InternalState == _stateProvider.GetWaitProvisional())
            {
                /*If no provisional response has been received, the CANCEL request MUST
               NOT be sent; rather, the client MUST wait for the arrival of a
               provisional response before sending the request.*/

                if (_logger.IsInfoEnabled) _logger.Info("The call will automatically be stopped when transitioning to 'WAITFINAL' state. Waiting for transition...");

               PendingInvite.CancelOnWaitFinal = true;
            }
            else if(InternalState == _stateProvider.GetWaitFinal())
            {
                SendCancel();

                if (_logger.IsInfoEnabled) _logger.Info("Phonecall stopped.");

                //TODO: send bye also to prevent possible peer-state race conditions?*/
            }
            else if (InternalState == _stateProvider.GetEstablished())
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Sending 'BYE'...");

                /*send bye + terminate dialog*/
                var byeRequest = PendingInvite.Dialog.CreateRequest(SipMethods.Bye);
                var byeCtx = _provider.CreateClientTransaction(byeRequest);
                PendingInvite.Dialog.SendRequest(byeCtx);

                if (_logger.IsDebugEnabled) _logger.Debug("'BYE' sent.");
                
                if (_logger.IsInfoEnabled) _logger.Info("Transitioning to 'WAITBYEOK' state...");

                ChangeState(_stateProvider.GetWaitByeOk());

                if (_logger.IsInfoEnabled) _logger.Info("Phonecall stopped.");
            }
            else
            {
                if (_logger.IsInfoEnabled) _logger.Info("Phonecall could not be stopped. DebugInfo: IsIncoming: '{0}', InternalState: '{1}'", _pendingPhoneCall.IsIncoming, InternalState.GetType().Name);
            }
        }

        private void OnRetransmitRinging()
        {            
            if (InternalState != _stateProvider.GetRinging())
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Retransmit ignored due to race condition. OnRetransmitRinging was called, but the phone is currently not in 'RINGING' state.");
                return;
            }
                        
            if (_logger.IsDebugEnabled) _logger.Debug("Retransmitting ringing response...");

            PendingInvite.InviteServerTransaction.SendResponse(PendingInvite.RingingResponse);

            if (_logger.IsDebugEnabled) _logger.Debug("Ringing response retransmitted.");  
        }

        public void SendCancel()
        {
            if (_logger.IsDebugEnabled) _logger.Debug("Sending 'CANCEL'...");

            var cancelRequest = PendingInvite.InviteClientTransaction.CreateCancelRequest();
            var ctx = _provider.CreateClientTransaction(cancelRequest);
            ctx.SendRequest();

            PendingInvite.CancelTransaction = ctx;

            if (_logger.IsDebugEnabled) _logger.Debug("'CANCEL' sent.");

            if (_logger.IsInfoEnabled) _logger.Info("Transitioning to 'WAITFORCANCELOK' state...");

            ChangeState(_stateProvider.GetWaitCancelOk());
        }

        private void OnPhoneCallStarted(IInternalPhoneCall phoneCall)
        {
            Check.Require(phoneCall, "phoneCall");
            Check.IsTrue(InternalState == _stateProvider.GetIdle(), string.Format("Failed to start the call. The phone can only start calls while in 'IDLE' state. CurrentState: '{0}'", CurrentState));
            Check.IsTrue(_isRunning, "Failed to start the call. The phone must be started first.");

            _pendingPhoneCall = phoneCall;

            if (_logger.IsInfoEnabled) _logger.Info("Starting new phonecall...");
            if (_logger.IsDebugEnabled) _logger.Debug("Creating 'INVITE' request...");
            var thisUri = AddressFactory.CreateUri(string.Empty, SipProvider.ListeningPoint.ToString());

            var requestUri = phoneCall.GetToUri();
            var toAddress = AddressFactory.CreateAddress(string.Empty, phoneCall.GetToUri());
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

            if (_logger.IsDebugEnabled) _logger.Debug("'INVITE' request created. Sending to callee..");

            var clientTransaction = SipProvider.CreateClientTransaction(request);
            var dialog = SipProvider.CreateClientDialog(clientTransaction as SipInviteClientTransaction);
            clientTransaction.SendRequest();

            if (_logger.IsDebugEnabled) _logger.Debug("'INVITE' sent.");

            if (_logger.IsInfoEnabled) _logger.Info("Request sent. Transitioning to 'WAITPROVISIONAL' state...");

            PendingInvite = new InviteInfo()
            {
                OriginalRequest = request,
                From = request.From.SipUri,
                To = request.To.SipUri,
                InviteClientTransaction = (SipInviteClientTransaction)clientTransaction,
                IsIncomingCall = false,
                Dialog = dialog
            };

            ChangeState(_stateProvider.GetWaitProvisional());
        }
        
        public void Handle(Exception e)
       { 
            if (_pendingPhoneCall != null)
            {
                _pendingPhoneCall.RaiseCallErrorOccured(CallError.UnHandeldException);
            }
        }

       
    }
       
}