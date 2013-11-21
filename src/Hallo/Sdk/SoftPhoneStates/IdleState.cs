

using Hallo.Sdk.Commands;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.Sip.Util;
using Hallo.Util;
using NLog;
using Hallo.Sip.Stack.Transactions.InviteClient;

namespace Hallo.Sdk.SoftPhoneStates
{
    internal class IdleState : ISoftPhoneState
    {
        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
         
        public void ProcessResponse(Sip.Stack.SipResponseEvent responseEvent)
        {
            
        }
        
        public void Initialize(IInternalSoftPhone softPhone)
        {
            softPhone.PendingInvite = null;
            _logger.Debug("Initialized.");
        }

        public void AfterInitialize(IInternalSoftPhone softPhone)
        {
        }

        public void ProcessRequest(IInternalSoftPhone softPhone, Sip.Stack.SipRequestEvent requestEvent)
        {
            string method = requestEvent.Request.RequestLine.Method;

            _logger.Debug("processing request: {0} ...", method);

            if (method != SipMethods.Invite)
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Received request: '{0}'. Request ignored.", method);
                return;
            }
            
            if(_logger.IsInfoEnabled) _logger.Info("'INVITE' received. Creating 'RINGING' response...");

            var ringingResponse = requestEvent.Request.CreateResponse(SipResponseCodes.x180_Ringing);
            ringingResponse.To.Tag = SipUtil.CreateTag();
            var contactUri = softPhone.AddressFactory.CreateUri("", softPhone.SipProvider.ListeningPoint.ToString());
            ringingResponse.Contacts.Add(softPhone.HeaderFactory.CreateContactHeader(contactUri));

            if (_logger.IsDebugEnabled) _logger.Debug("Sending response ... ");
                               
            var serverTransaction = softPhone.SipProvider.CreateServerTransaction(requestEvent.Request);
            var dialog = softPhone.SipProvider.CreateServerDialog(serverTransaction as SipInviteServerTransaction);
            serverTransaction.SendResponse(ringingResponse);
            requestEvent.IsSent = true;

            if (_logger.IsInfoEnabled) _logger.Info("Response send. Transitioning to 'RINGING' state.");

            softPhone.PendingInvite = new InviteInfo() 
            { 
                OriginalRequest = requestEvent.Request, 
                RingingResponse = ringingResponse,
                From = requestEvent.Request.From.SipUri,
                To = requestEvent.Request.To.SipUri,
                InviteServerTransaction = (SipInviteServerTransaction) serverTransaction,
                IsIncomingCall = true,
                Dialog = dialog
            };

            softPhone.RaiseIncomingCall();/* !! the incoming call event is raised, before any callstate events can occur*/

            softPhone.ChangeState(softPhone.StateProvider.GetRinging()); 
            
            if (_logger.IsDebugEnabled) _logger.Debug("'RINGING' response created. Raising Incoming PhoneCall...");
            
            if (_logger.IsDebugEnabled) _logger.Debug("Raised.");
        }

        public void ProcessResponse(IInternalSoftPhone softPhone, Sip.Stack.SipResponseEvent responseEvent)
        {
           
        }


        public void Terminate(IInternalSoftPhone softPhone)
        {
            
        }
    }

}