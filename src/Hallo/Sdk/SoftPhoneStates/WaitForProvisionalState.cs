using Hallo.Sip;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Util;
using NLog;

namespace Hallo.Sdk.SoftPhoneStates
{
    internal class WaitForProvisionalState : ISoftPhoneState
    {
        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public void Initialize(IInternalSoftPhone softPhone)
        {
            _logger.Debug("Initialized.");
        }

        public void AfterInitialize(IInternalSoftPhone softPhone)
        {
            Check.Require(softPhone, "softPhone");
            Check.Require(softPhone.PendingCall, "softPhone.PendingCall");

            softPhone.PendingCall.ChangeState(CallState.Setup);
        }

        public void ProcessRequest(IInternalSoftPhone softPhone, Sip.Stack.SipRequestEvent requestEvent)
        {
            
        }

        public void ProcessResponse(IInternalSoftPhone softPhone, Sip.Stack.SipResponseEvent responseEvent)
        {
            SipStatusLine statusLine = responseEvent.Response.StatusLine;

            _logger.Debug("processing response: {0} ...", statusLine.ResponseCode);

            if (statusLine.ResponseCode == SipResponseCodes.x100_Trying)
            {
                if (_logger.IsDebugEnabled) _logger.Debug("'Trying' response received. Ignored");
                return;
            }

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

            if (responseEvent.ClientTransaction.GetId() != softPhone.PendingInvite.InviteClientTransaction.GetId())
            {
                if (_logger.IsInfoEnabled) _logger.Info("Processing ABORTED. The '{0}XX' ResponseEvent.ClientTransaction, is expected to match only to the InviteSendTransaction of the PendingInvite. DebugInfo: TxId created from RESPONSE: '{1}'. This case is not supposed to occur, since the phone can only process ONE PendingInvite at a time. Check what's going on !!", statusCodeDiv100, SipProvider.GetClientTransactionId(responseEvent.Response));
                return;
            }

            if (responseEvent.Dialog == null)
            {
                if (_logger.IsInfoEnabled) _logger.Info("Processing ABORTED. In this state the 'ResponseEvent' is expected to have a 'NOT NULL' dialog. DebugInfo: DialogId created from Response: '{0}'. This Id could not be matched to a dialog in the provider's dialogtable.", SipProvider.GetDialogId(responseEvent.Response, false));
                return;
            }

            if (responseEvent.Dialog.GetId() != softPhone.PendingInvite.Dialog.GetId())
            {
                if (_logger.IsInfoEnabled) _logger.Info("Processing ABORTED. In this state the 'ResponseEvent' it's Dialog, is expected to match only to the Dialog of the PendingInvite. DebugInfo: DialogId created from response: '{0}'. This case is not supposed to occur, since the phone can only process ONE dialog at a time. Check what's going on !!", SipProvider.GetDialogId(responseEvent.Response, false));
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

                Check.IsTrue(responseEvent.Dialog is SipInviteClientDialog, "Failed to respond to '200' response with ACK request. The 'ACK' can not be created. The responseEvent.Dialog is expected to be of type SipInviteClientDialog.");

                var clientDialog = (SipInviteClientDialog)softPhone.PendingInvite.Dialog;
                var ack = clientDialog.CreateAck();
                clientDialog.SendAck(ack);

                softPhone.ChangeState(softPhone.StateProvider.GetEstablished());
            }
            else
            {
                //change callstate. don't go automatically to 'IDLE' state, but wait for the api user to invoke Call.Stop() 

                if (statusLine.ResponseCode == SipResponseCodes.x486_Busy_Here)
                {
                    if (_logger.IsDebugEnabled) _logger.Debug("Changing CallState to 'BusyHere'");
                    softPhone.PendingCall.ChangeState(CallState.BusyHere);
                }
                else
                {
                    if (_logger.IsDebugEnabled) _logger.Debug("Changing CallState to 'Error'");
                    softPhone.PendingCall.ChangeState(CallState.Error);
                }

                softPhone.PendingInvite.Dialog.Terminate();
               
            }
        }

        public void Terminate(IInternalSoftPhone softPhone)
        {

        }
        
    }
}