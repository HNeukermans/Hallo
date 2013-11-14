using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip;
using Hallo.Util;
using NLog;

namespace Hallo.Sdk.SoftPhoneStates
{
    internal class EstablishedState : ISoftPhoneState
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

            softPhone.PendingCall.ChangeState(CallState.InCall);
        }

        public void ProcessRequest(IInternalSoftPhone softPhone, Sip.Stack.SipRequestEvent requestEvent)
        {
            string method = requestEvent.Request.RequestLine.Method;

            _logger.Debug("processing request: {0} ...", method);

            if (method != SipMethods.Bye)
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Received request: '{0}'. Request ignored.", method);
                return;
            }

            if (_logger.IsInfoEnabled)
            {
                _logger.Info("'BYE' received. Begin processing...");
            }

            if (requestEvent.Dialog == null)
            {
                if (_logger.IsInfoEnabled) _logger.Info("Processing ABORTED. A 'BYE' RequestEvent is expected to have a 'NOT NULL' dialog. DebugInfo: DialogId created from BYE: '{0}'. This Id could not be matched to a dialog in the provider's dialogtable.", SipProvider.GetDialogId(requestEvent.Request, true));
                return;
            }

            if (requestEvent.Dialog.GetId() != softPhone.PendingInvite.ServerDialog.GetId())
            {
                if (_logger.IsInfoEnabled) _logger.Info("Processing ABORTED. The 'BYE' RequestEvent.Dialog, is expected to match only to the Dialog of the PendingInvite. DebugInfo: DialogId created from BYE: '{0}'. This case is not supposed to occur, since the phone can only process ONE dialog at a time. Check what's going on !!", SipProvider.GetDialogId(requestEvent.Request, true));
                return;
            }

            if (_logger.IsDebugEnabled) _logger.Debug("Sending OK response to BYE..");
             
            var okResponse = requestEvent.Request.CreateResponse(SipResponseCodes.x200_Ok);

            var tx = softPhone.SipProvider.CreateServerTransaction(requestEvent.Request);

            tx.SendResponse(okResponse);

            requestEvent.IsSent = true;

            softPhone.PendingInvite.ServerDialog.Terminate();

            if (_logger.IsDebugEnabled) _logger.Debug("OK Send.");

            if (_logger.IsInfoEnabled)
            {
                 _logger.Info("Transitioning to 'IDLE'...");
            }

            softPhone.ChangeState(softPhone.StateProvider.GetIdle());          
        }

        public void ProcessResponse(IInternalSoftPhone softPhone, Sip.Stack.SipResponseEvent responseEvent)
        {
            
        }

        public void Terminate(IInternalSoftPhone softPhone)
        {
            
        }
        
    }
}
