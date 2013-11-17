using Hallo.Sip;
using Hallo.Util;
using NLog;

namespace Hallo.Sdk.SoftPhoneStates
{
    internal class WaitForByeOkState : ISoftPhoneState
    {
        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private bool _receivedFinalCancelResponse;
        private bool _receivedFinalInviteResponse;

        public void ProcessResponse(Sip.Stack.SipResponseEvent responseEvent)
        {

        }

        public void Initialize(IInternalSoftPhone softPhone)
        {
            Check.Require(softPhone, "softPhone");
            Check.Require(softPhone.PendingInvite, "softPhone.PendingInvite");

            _logger.Debug("Initialized.");
        }

        public void AfterInitialize(IInternalSoftPhone softPhone)
        {
            Check.Require(softPhone, "softPhone");
            Check.Require(softPhone.PendingCall, "softPhone.PendingCall");
            Check.Require(softPhone.PendingInvite, "softPhone.PendingInvite");
            Check.Require(softPhone.PendingInvite.Dialog, "softPhone.PendingInvite.Dialog");
            
            softPhone.PendingCall.ChangeState(CallState.Completed);

            if (_logger.IsInfoEnabled) _logger.Info("CallState changed to 'Completed'.");

            softPhone.PendingInvite.Dialog.Terminate();

            if (_logger.IsDebugEnabled) _logger.Debug("Dialog terminated.");
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

            /*wait for ok on bye*/

            if (_logger.IsInfoEnabled) _logger.Info("'{0}XX' response. Begin processing...", statusCodeDiv100);
            
            if (statusCodeDiv100 != 2)
            {
                if (_logger.IsInfoEnabled) _logger.Info("Processing ABORTED. Only 'x200-299' responses are processed in this state.");
                return;
            }

            if (responseEvent.ClientTransaction.Request.CSeq.Command == SipMethods.Bye)
            {
                if (_logger.IsInfoEnabled) _logger.Info("Received final response on 'BYE' request.Transitioning to Idle...");
               
                softPhone.ChangeState(softPhone.StateProvider.GetIdle());
            }
            else
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Response ignored. This is not the response to the 'BYE' request.");
            }
            
        }

        public void Terminate(IInternalSoftPhone softPhone)
        {
            
        }
    }
}