using Hallo.Sdk.Commands;
using Hallo.Sip;
using Hallo.Sip.Stack;
using NLog;

namespace Hallo.Sdk.SoftPhoneStates
{
    internal class RingingState : ISoftPhoneState
    {
        private static readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private IInternalSoftPhone _softPhone;
        private readonly bool _isOutgoingCall;

        public RingingState()
        {           
        }
                           
        public SoftPhoneState StateName
        {
            get { return SoftPhoneState.Ringing; }
        }

        public void Initialize(IInternalSoftPhone softPhone)
        {
            if (!softPhone.PendingInvite.IsIncomingCall)
            {
                softPhone.RingingTimer.Start();
            }
        }

        public void ProcessRequest(IInternalSoftPhone softPhone, SipRequestEvent requestEvent)
        {
            
        }

        public void ProcessResponse(IInternalSoftPhone softPhone, SipResponseEvent responseEvent)
        {
            var statusCode = responseEvent.Response.StatusLine.StatusCode;

            if (statusCode >= 200 && statusCode < 300)
            {
                //softPhone.ChangeState(new WaitAckState());
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