using Hallo.Sdk.Commands;
using Hallo.Sip;
using Hallo.Sip.Stack;
using NLog;

namespace Hallo.Sdk.SoftPhoneStates
{
    internal class RingingState : ISoftPhoneState
    {
        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        
        public RingingState()
        {           
        }
                           
        public SoftPhoneState StateName
        {
            get { return SoftPhoneState.Ringing; }
        }

        public void Initialize(IInternalSoftPhone softPhone)
        {
            if (softPhone.PendingInvite.IsIncomingCall)
            {
                softPhone.RetransmitRingingTimer.Start();
            }

            _logger.Debug("Initialized.");
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

            #region terminate pending invite

            if (_logger.IsDebugEnabled) _logger.Debug("Step 1: create and send '487-Request terminated' response.");
                
            /*terminate pending invite with 487 (assumption: PendingInvite.OriginalRequest= requestEvent.ServerTransaction.Request)*/
            var requestToCancel = requestEvent.ServerTransaction.Request;
            var terminateResponse = requestToCancel.CreateResponse(SipResponseCodes.x487_Request_Terminated);

            /*terminate pending invite.*/
            softPhone.PendingInvite.InviteTransaction.SendResponse(terminateResponse);

            if (_logger.IsDebugEnabled) _logger.Debug("Step 1 ended.");
                
            #endregion

            #region respond to cancel request with ok

            if (_logger.IsDebugEnabled) _logger.Debug("Step 2: answer to cancel with ok.");
               
            var cancelOkResponse = requestEvent.Request.CreateResponse(SipResponseCodes.x200_Ok);
            var cancelOkTx = softPhone.SipProvider.CreateServerTransaction(requestEvent.Request);
            cancelOkTx.SendResponse(cancelOkResponse);
            requestEvent.IsSent = true;

            if (_logger.IsDebugEnabled) _logger.Debug("Step 2 ended.");
      
            #endregion

            if (_logger.IsInfoEnabled) _logger.Info("'CANCEL' Processed. Transitioning (back) to 'Idle'");

            softPhone.ChangeState(softPhone.StateProvider.GetIdle()); 
                                   
        }

        public void ProcessResponse(IInternalSoftPhone softPhone, SipResponseEvent responseEvent)
        {
            var statusCode = responseEvent.Response.StatusLine.StatusCode;

            if (statusCode >= 200 && statusCode < 300)
            {
                //softPhone.ChangeState(new WaitAckState());
            }
        }
        
        public void Terminate(IInternalSoftPhone softPhone)
        {
            if (softPhone.PendingInvite.IsIncomingCall)
            {
                softPhone.RetransmitRingingTimer.Stop();
            }
        }
    }

    //internal class WaitAckState : ISoftPhoneState
    //{
    //    public void Initialize(IInternalSoftPhone softPhone)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public ICommand ProcessRequest(IInternalSoftPhone softPhone, SipRequestEvent requestEvent)
    //    {
    //        if (requestEvent.Request.RequestLine.Method == SipMethods.Ack)
    //        {
    //            var r = new TransitionResult();
    //            r.TransitionTo = new EstablishedState();
    //            return r;
    //        }

    //        return new EmptyResult();
    //    }

    //    public void ProcessResponse(IInternalSoftPhone softPhone, SipResponseEvent responseEvent)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}

    //internal class EstablishedState : ISoftPhoneState
    //{
    //    public void Initialize(IInternalSoftPhone softPhone)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public ICommand ProcessRequest(IInternalSoftPhone softPhone, SipRequestEvent requestEvent)
    //    {
    //        if (requestEvent.Request.RequestLine.Method == SipMethods.Bye)
    //        {
    //            //var r = new SendRequestResult();
    //            //r.Response = null;//todo send ok;
    //            //r.TransitionTo = new IdleState();
    //            //return r;
    //        }

    //        return new EmptyCommand();
    //    }

    //    public void ProcessResponse(IInternalSoftPhone softPhone, SipResponseEvent responseEvent)
    //    {
            
    //    }
    //}
}