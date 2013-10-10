using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.Util;

namespace Hallo.Sdk.SoftPhoneStates
{
    internal class WaitForAckState : ISoftPhoneState
    {
        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public void ProcessResponse(Sip.Stack.SipResponseEvent responseEvent)
        {

        }

        public SoftPhoneState StateName
        {
            get { return SoftPhoneState.Ringing; }
        }

        public void Initialize(IInternalSoftPhone softPhone)
        {
            Check.Require(softPhone.EndWaitForAckTimer, "softPhone.EndWaitForAckTimer");

            softPhone.EndWaitForAckTimer.Start();
            _logger.Debug("Initialized.");
        }

        public void ProcessRequest(IInternalSoftPhone softPhone, Sip.Stack.SipRequestEvent requestEvent)
        {
            string method = requestEvent.Request.RequestLine.Method;

            _logger.Debug("processing request: {0} ...", method);

            if (method != SipMethods.Ack)
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Received request: '{0}'. Request ignored.", method);
                return;
            }

            if (_logger.IsInfoEnabled)
            {
                _logger.Info("'ACK' received. Begin processing...");
            }

            if (requestEvent.Dialog == null)
            {
                if (_logger.IsInfoEnabled) _logger.Info("Processing ABORTED. An 'ACK' RequestEvent is expected to have a 'NOT NULL' dialog. DebugInfo: DialogId created from ACK: '{0}'. This Id could not be matched to a dialog in the provider's dialogtable.", SipProvider.GetDialogId(requestEvent.Request, true));
                return;
            }

            if (requestEvent.Dialog.GetId() != softPhone.PendingInvite.Dialog.GetId())
            {
                if (_logger.IsInfoEnabled) _logger.Info("Processing ABORTED. The 'ACK' RequestEvent it's Dialog, is expected to match only to the Dialog of the PendingInvite. DebugInfo: DialogId created from ACK: '{0}'. This case is not supposed to occur, since the phone can only process ONE dialog at a time. Check what's going on !!", SipProvider.GetDialogId(requestEvent.Request, true));
                return;
            }
            
            if (_logger.IsInfoEnabled)
            {
                _logger.Info("Transitioning to 'ESTABLISHED'...");
            }

            softPhone.ChangeState(softPhone.StateProvider.GetEstablished());           
        }



        public void ProcessResponse(IInternalSoftPhone softPhone, Sip.Stack.SipResponseEvent responseEvent)
        {

        }

        public void Terminate(IInternalSoftPhone softPhone)
        {
            Check.Require(softPhone.EndWaitForAckTimer, "softPhone.EndWaitForAckTimer");

             softPhone.EndWaitForAckTimer.Stop();
        }
    }

}
