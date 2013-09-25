

using Hallo.Sdk.Commands;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.Sip.Util;
using NLog;

namespace Hallo.Sdk.SoftPhoneStates
{
    internal class IdleState : ISoftPhoneState
    {
        private readonly IInternalSoftPhone _softPhone;
        private static readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public IdleState(IInternalSoftPhone softPhone)
        {
            _softPhone = softPhone;
        }

        public void Initialize()
        {
            _softPhone.PendingInvite = null;
            logger.Debug("Initialized.");
        }

        public void ProcessResponse(Sip.Stack.SipResponseEvent responseEvent)
        {
            
        }

        public ICommand ProcessRequest(Sip.Stack.SipRequestEvent requestEvent)
        {
            string method = requestEvent.Request.RequestLine.Method;

            logger.Debug("processing request: {0} ...", method);

            if (requestEvent.Request.RequestLine.Method == SipMethods.Invite)
            {
                logger.Info("'INVITE' received. Creating ringing response...", method);

                var ringingResponse = requestEvent.Request.CreateResponse(SipResponseCodes.x180_Ringing);
                ringingResponse.To.Tag = SipUtil.CreateTag();
                var contactUri = _softPhone.AddressFactory.CreateUri("", _softPhone.SipProvider.ListeningPoint.ToString());
                ringingResponse.Contacts.Add(_softPhone.HeaderFactory.CreateContactHeader(contactUri));

                logger.Info("'In-dialog RINGING' response sent.");

                var command = _softPhone.CommandFactory.CreateIdleSendCommand(ringingResponse, new RingingState(_softPhone, true),  requestEvent);
                return command;
            }
            else
            {
                //maybe sent method not supported ??? 
            }

            return new EmptyCommand();
        }

        public ICommand StartCall(SipUri calleeUri)
        {
            var thisUri = _softPhone.AddressFactory.CreateUri(string.Empty, _softPhone.SipProvider.ListeningPoint.ToString());

            var requestUri = calleeUri;
            var toAddress = _softPhone.AddressFactory.CreateAddress(string.Empty, calleeUri);
            var fromAddress = _softPhone.AddressFactory.CreateAddress(string.Empty, thisUri);
            var toHeader = _softPhone.HeaderFactory.CreateToHeader(toAddress);
            var fromHeader = _softPhone.HeaderFactory.CreateFromHeader(fromAddress, SipUtil.CreateTag());
            var cseqHeader = _softPhone.HeaderFactory.CreateSCeqHeader(SipMethods.Invite, _softPhone.MessageCounter++);
            var callIdheader = _softPhone.HeaderFactory.CreateCallIdHeader(SipUtil.CreateCallId());
            var viaHeader = _softPhone.HeaderFactory.CreateViaHeader(_softPhone.SipProvider.ListeningPoint.Address,
                                                          _softPhone.SipProvider.ListeningPoint.Port, SipConstants.Udp,
                                                          SipUtil.CreateBranch());
            var maxForwardsHeader = _softPhone.HeaderFactory.CreateMaxForwardsHeader(1);
            var request = _softPhone.MessageFactory.CreateRequest(
                requestUri,
                SipMethods.Invite,
                callIdheader,
                cseqHeader,
                fromHeader,
                toHeader,
                viaHeader,
                maxForwardsHeader);

            /*add contactheader*/
            var contactUri = _softPhone.AddressFactory.CreateUri(thisUri.User, viaHeader.SentBy.ToString());
            var contactHeader = _softPhone.HeaderFactory.CreateContactHeader(contactUri);
            request.Contacts.Add(contactHeader);

            var command = _softPhone.CommandFactory.CreateInitialInviteRequestSendCommand(request);
            return command;
        }

        public SoftPhoneState StateName
        {
            get { return SoftPhoneState.Idle; }
        }
    }
}