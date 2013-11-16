using Hallo.Sip;
using Hallo.Util;
using NLog;

namespace Hallo.Sdk.SoftPhoneStates
{
    internal class WaitForCancelOkState : ISoftPhoneState
    {
        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private bool _receivedFinalCancelResponse;
        private bool _receivedFinalInviteResponse;

        public void ProcessResponse(Sip.Stack.SipResponseEvent responseEvent)
        {

        }

        public void Initialize(IInternalSoftPhone softPhone)
        {
            Check.Require(softPhone.PendingInvite, "softPhone.PendingInvite");
            Check.Require(softPhone.PendingInvite.CancelTransaction, "softPhone.PendingInvite.CancelTransaction");
            Check.Require(softPhone.PendingInvite.InviteClientTransaction, "softPhone.PendingInvite.InviteClientTransaction");

            _logger.Debug("Initialized.");
        }

        public void AfterInitialize(IInternalSoftPhone softPhone)
        {
        }

        public void ProcessRequest(IInternalSoftPhone softPhone, Sip.Stack.SipRequestEvent requestEvent)
        {
           
        }
        
        public void ProcessResponse(IInternalSoftPhone softPhone, Sip.Stack.SipResponseEvent responseEvent)
        {
            SipStatusLine statusLine = responseEvent.Response.StatusLine;

            _logger.Debug("processing response: {0} ...", statusLine.ResponseCode);

            int statusCodeDiv100 = statusLine.StatusCode / 100;

            if (responseEvent.ClientTransaction == null)
            {
                if (_logger.IsInfoEnabled) _logger.Info("Processing ABORTED. In this state, the ResponseEvent is expected to have a 'NOT NULL' ClientTx. DebugInfo: TxId created from RESPONSE: '{1}'. This Id could not be matched to a clienttransaction in the provider's clienttransactiontable.", statusCodeDiv100, SipProvider.GetClientTransactionId(responseEvent.Response));
                return;
            }

            /*wait for ok on cancel + 487 for invite*/

            if (_logger.IsInfoEnabled) _logger.Info("'{0}XX' response. Begin processing...", statusCodeDiv100);
            
            if (statusCodeDiv100 < 2)
            {
                if (_logger.IsInfoEnabled) _logger.Info("Processing ABORTED. Only FINAL (x200-x500) responses are processed in this state.");
                return;
            }

            if (responseEvent.ClientTransaction.GetId() == softPhone.PendingInvite.CancelTransaction.GetId())
            {
                if (_logger.IsInfoEnabled) _logger.Info("Received final response on 'CANCEL' request.");

                _receivedFinalCancelResponse = true;

            }

            if (responseEvent.ClientTransaction.GetId() == softPhone.PendingInvite.InviteClientTransaction.GetId())
            {
                if (_logger.IsInfoEnabled) _logger.Info("Received final response on 'INVITE' request.");

                _receivedFinalInviteResponse = true;

                if (_logger.IsDebugEnabled) _logger.Debug("Changing CallState to 'Completed', and terminating the Dialog...");

                softPhone.PendingCall.ChangeState(CallState.Completed);

                softPhone.PendingInvite.ClientDialog.Terminate();

                if (_logger.IsDebugEnabled) _logger.Debug("Dialog terminated.");
            }
            
            //TODO: use locks.
            if (_receivedFinalInviteResponse && _receivedFinalCancelResponse)
            {
                /*go to idle*/
                if (_logger.IsDebugEnabled) _logger.Debug("Both 'CANCEL' & 'INVITE' Tx have received a final response. Transitioning to Idle...");
                softPhone.ChangeState(softPhone.StateProvider.GetIdle());
            }
        }

        public void Terminate(IInternalSoftPhone softPhone)
        {
            
        }
    }
}