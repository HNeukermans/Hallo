using Hallo.Sip;
using NLog;

namespace Hallo.Sdk.SoftPhoneStates
{
    internal class WaitProvisionalState : ISoftPhoneState
    {
        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public void Initialize(IInternalSoftPhone softPhone)
        {
            _logger.Debug("Initialized.");
        }

        public void ProcessRequest(IInternalSoftPhone softPhone, Sip.Stack.SipRequestEvent requestEvent)
        {
            
        }

        public void ProcessResponse(IInternalSoftPhone softPhone, Sip.Stack.SipResponseEvent responseEvent)
        {
            SipStatusLine statusLine = responseEvent.Response.StatusLine;

            _logger.Debug("processing response: {0} ...", statusLine.ResponseCode);
            
            int statusCodeDiv100 = statusLine.StatusCode/100;

            if (_logger.IsInfoEnabled)
            {
                _logger.Info("'{0}XX' response. Begin processing...", statusCodeDiv100);
            }

            if (responseEvent.ClientTransaction == null)
            {
                if (_logger.IsInfoEnabled) _logger.Info("Processing ABORTED. A '{0}XX' ResponseEvent is expected to have a 'NOT NULL' ClientTx. DebugInfo: TxId created from RESPONSE: '{1}'. This Id could not be matched to a clienttransaction in the provider's clienttransactiontable.", statusCodeDiv100, SipProvider.GetClientTransactionId(responseEvent.Response));
                return;
            }

            if (responseEvent.ClientTransaction.GetId() != softPhone.PendingInvite.InviteSendTransaction.GetId())
            {
                if (_logger.IsInfoEnabled) _logger.Info("Processing ABORTED. The '{0}XX' ResponseEvent.ClientTransaction, is expected to match only to the InviteSendTransaction of the PendingInvite. DebugInfo: TxId created from RESPONSE: '{1}'. This case is not supposed to occur, since the phone can only process ONE PendingInvite at a time. Check what's going on !!", statusCodeDiv100, SipProvider.GetClientTransactionId(responseEvent.Response));
                return;
            }

            if (statusCodeDiv100 > 2)
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Transitioning back to 'IDLE'...", statusLine.ResponseCode);
                softPhone.ChangeState(softPhone.StateProvider.GetIdle());
                return;
            }

            if (statusCodeDiv100 == 1)
            {
                /*go to wait for final*/
                if (_logger.IsInfoEnabled) _logger.Info("Transitioning to 'WAITFINAL'...");

                softPhone.ChangeState(softPhone.StateProvider.GetWaitFinal());
            }
            else if (statusCodeDiv100 == 2)
            {
                if (_logger.IsInfoEnabled) _logger.Info("Transitioning to 'ESTABLISHED'...");
                
                softPhone.ChangeState(softPhone.StateProvider.GetEstablished());
            }
        }

        public void Terminate(IInternalSoftPhone softPhone)
        {

        }

        public SoftPhoneState StateName
        {
            get { return SoftPhoneState.Waiting; }
        }
    }
}