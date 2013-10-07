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
                _logger.Info("'ACK' received. Begin processing... Transitioning to 'ESTABLISHED'...");
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
