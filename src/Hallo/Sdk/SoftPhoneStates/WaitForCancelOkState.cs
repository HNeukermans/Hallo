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
            _logger.Debug("Initialized.");
        }

        public void AfterInitialize(IInternalSoftPhone softPhone)
        {
            Check.Require(softPhone, "softPhone");
            Check.Require(softPhone.PendingCall, "softPhone.PendingCall");
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


            if (statusCodeDiv100 != 2)
            {
                if (_logger.IsInfoEnabled) _logger.Info("Processing ABORTED. Only '2XX' responses are processed in this state.");
                return;
            }

            if (responseEvent.ClientTransaction.GetId() == softPhone.PendingInvite.CancelTransaction.GetId())
            {
                if (_logger.IsInfoEnabled) _logger.Info("Received response on 'CANCEL' request.");

                _receivedFinalCancelResponse = true;

                return;
            }

            if (responseEvent.ClientTransaction.GetId() == softPhone.PendingInvite.InviteClientTransaction.GetId())
            {
                if (_logger.IsInfoEnabled) _logger.Info("Received response on 'INVITE' request.");

                _receivedFinalInviteResponse = true;

                return;
            }

            if (_receivedFinalInviteResponse && _receivedFinalCancelResponse)
            {
                /*go to idle*/
            }

            if (_logger.IsInfoEnabled) _logger.Info("OK response to 'CANCEL' received. Transitioning to 'IDLE'...");

            softPhone.ChangeState(softPhone.StateProvider.GetIdle());
        }

        public void Terminate(IInternalSoftPhone softPhone)
        {
            
        }
    }
}