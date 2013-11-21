using System.Collections.Generic;
using Hallo.Sdk.Commands;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions;
using Hallo.Util;
using NLog;

namespace Hallo.Sdk.SoftPhoneStates
{
    internal class RingingState : ISoftPhoneState
    {
        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        
        public RingingState()
        {           
        }
         
        public void Initialize(IInternalSoftPhone softPhone)
        {
            Check.Require(softPhone, "softPhone");
            Check.Require(softPhone.PendingInvite, "softPhone.PendingInvite");

            if (softPhone.PendingInvite.IsIncomingCall)
            {
                softPhone.RetransmitRingingTimer.Start();
            }

            _logger.Debug("Initialized.");
        }

        public void AfterInitialize(IInternalSoftPhone softPhone)
        {
            Check.Require(softPhone, "softPhone");
            Check.Require(softPhone.PendingCall, "softPhone.PendingCall");

            softPhone.PendingCall.ChangeState(CallState.Ringing);
        }

        public void ProcessRequest(IInternalSoftPhone softPhone, SipRequestEvent requestEvent)
        {
            string method = requestEvent.Request.RequestLine.Method;

            if (method != SipMethods.Cancel)
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Received request: '{0}'. Request ignored.", method);
                return;
            }

            if (_logger.IsInfoEnabled) _logger.Info("'CANCEL' received. Start processing...");

            #region attempt to match transaction
            
            /*terminate pending invite with 487 (assumption: PendingInvite.OriginalRequest= requestEvent.ServerTransaction.Request)*/

            if (_logger.IsDebugEnabled) _logger.Debug("Attempting to match 'CANCEL' target-tx against a tx in the ServerTransactionTable...");
            
            var txIdAttempts = CreateTxIdAttempts(requestEvent.Request);

            ISipServerTransaction serverTransaction = null;
            foreach (var txIdAttempt in txIdAttempts)
            {
                serverTransaction = softPhone.SipProvider.FindServerTransactionById(txIdAttempt);
                if(serverTransaction !=null) break;
            }

            #endregion

            if (serverTransaction == null)
            {
                #region no match

                if (_logger.IsDebugEnabled) _logger.Debug("Could not match 'CANCEL' to an existing transaction. Sending x487 response...");

                var cancelResponse = requestEvent.Request.CreateResponse(SipResponseCodes.x481_Call_Transaction_Does_Not_Exist);
                var cancelTx = softPhone.SipProvider.CreateServerTransaction(requestEvent.Request);
                cancelTx.SendResponse(cancelResponse);
                requestEvent.IsSent = true;

                if (_logger.IsDebugEnabled) _logger.Debug("x487 response send.");

                #endregion
            }
            else
            {
                #region matched
                
                #region respond to cancel with ok

                if (_logger.IsDebugEnabled) _logger.Debug("'CANCEL' target-tx matched. Answering to cancel with ok...");

                var cancelOkResponse = requestEvent.Request.CreateResponse(SipResponseCodes.x200_Ok);
                var cancelOkTx = softPhone.SipProvider.CreateServerTransaction(requestEvent.Request);
                cancelOkTx.SendResponse(cancelOkResponse);
                requestEvent.IsSent = true;

                if (_logger.IsDebugEnabled) _logger.Debug("Answered.");

                #endregion

                #region respond to matched tx with x487
                
                if (serverTransaction.GetId() != softPhone.PendingInvite.InviteServerTransaction.GetId())
                {
                    if (_logger.IsInfoEnabled) _logger.Info("'CANCEL' target-tx does NOT match 'INVITE.' Processing ABORTED. The 'CANCEL' target-tx is expected, to match only to the pending 'INVITE' servertransaction.");
                }
                else
                {
                    if (_logger.IsDebugEnabled) _logger.Debug("Creating '487-Request terminated' response and sending...");

                    var requestToCancel = serverTransaction.Request;
                    var terminateResponse = requestToCancel.CreateResponse(SipResponseCodes.x487_Request_Terminated);

                    /*terminate pending invite.*/
                    softPhone.PendingInvite.InviteServerTransaction.SendResponse(terminateResponse);

                    if (_logger.IsDebugEnabled) _logger.Debug("Response send.");

                    if (_logger.IsDebugEnabled) _logger.Debug("Changing callstate to 'CANCELLED'.");

                    softPhone.PendingCall.ChangeState(CallState.Cancelled);
                    
                    if (_logger.IsInfoEnabled) _logger.Info("'CANCEL' Processed. Transitioning (back) to 'Idle'");

                    softPhone.ChangeState(softPhone.StateProvider.GetIdle());
                }

                #endregion

                #endregion
            }

        }

        /// <summary>
        /// creates the ids that must be attempted to match a transaction against the table
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private List<string> CreateTxIdAttempts(SipRequest request)
        {
            var result = new List<string>();

            /*up to now the only request that can be cancelled is 'INVITE'*/
            var cancellableMethods = new[] {SipMethods.Invite};

            foreach (var method in cancellableMethods)
            {
                result.Add(SipProvider.GetServerTransactionId(request, method));
            }

            return result;
        }

        public void ProcessResponse(IInternalSoftPhone softPhone, SipResponseEvent responseEvent)
        {
            
        }
        
        public void Terminate(IInternalSoftPhone softPhone)
        {
            softPhone.RetransmitRingingTimer.Stop();
        }
    }

    
}