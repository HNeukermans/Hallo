using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Util;
using Hallo.Util;

namespace Hallo.Sip.Stack.Transactions.InviteClient
{
    public partial class SipInviteClientTransaction : SipAbstractClientTransaction
    {
        private readonly SipHeaderFactory _headerFactory;
        private readonly SipMessageFactory _messageFactory;
        internal static readonly CallingCtxState CallingState = new CallingCtxState();
        internal static readonly ProceedingCtxState ProceedingState = new ProceedingCtxState();
        internal static readonly CompletedCtxState CompletedState = new CompletedCtxState();
        internal static readonly TerminatedCtxState TerminatedState = new TerminatedCtxState();
        private SipAbstractDialog _dialog;

        internal SipInviteClientTransaction(
            SipClientTransactionTable table,
            ISipMessageSender messageSender,
            ISipListener listener, 
            SipRequest request, 
            ITimerFactory timerFactory, 
            SipHeaderFactory headerFactory,
            SipMessageFactory messageFactory)
            : base(table, request, messageSender, listener, timerFactory)
        {
            Check.Require(headerFactory, "headerFactory");
            Check.Require(messageFactory, "messageFactory");

            Check.IsTrue(request.RequestLine.Method == SipMethods.Invite, "Method other then 'INVITE' is not allowed");

            _logger = NLog.LogManager.GetCurrentClassLogger();

            _headerFactory = headerFactory;
            _messageFactory = messageFactory;

            ReTransmitTimer = timerFactory.CreateInviteCtxRetransmitTimer(OnReTransmit);
            TimeOutTimer = timerFactory.CreateInviteCtxTimeOutTimer(OnTimeOut);
            EndCompletedTimer = timerFactory.CreateInviteCtxEndCompletedTimer(OnCompletedEnded);
        }
        
        private void OnReTransmit()
        {
            lock (_lock)
            {
                /* ignore callbacks when the timer is disposed. These can potentially happen when
                * timercallbacks, queued by the threadpool, are invoked (with a certain delay !!)*/

                if(!ReTransmitTimer.IsDisposed) State.Retransmit(this);
            }
        }

        private void OnTimeOut()
        {
            //ChangeState(SipInviteClientTransaction.TerminatedState);

            Dispose();

            _listener.ProcessTimeOut(new SipClientTxTimeOutEvent() { Request = Request, ClientTransaction = this });
        }

        private void OnCompletedEnded()
        {
            //changing to TerminatedState ni the dispose method.
            // ChangeState(SipInviteClientTransaction.TerminatedState);
            Dispose();
        }

        public override void Dispose()
        {
            lock (_lock)
            {
                if (_isDisposed) return;
                _isDisposed = true;
                ReTransmitTimer.Dispose();
                TimeOutTimer.Dispose();
                EndCompletedTimer.Dispose();
                
                SipAbstractClientTransaction tx;
                _table.TryRemove(this.GetId(), out tx);

                //removed comment. but not sure. 03.06.2013
                State = TerminatedState; //is done outside of this method?
            }

            if (_stateObserver != null)
            {
                //removed comment. but not sure. 03.06.2013
                _stateObserver.OnNext(CreateStateInfo(State.Name)); //is done outside of this method?
                _stateObserver.OnCompleted();
            }
        }

        internal AbstractCtxState State { get; private set; }
        internal SipResponse LatestResponse { get; set; }
        private ITimer ReTransmitTimer { get; set; }
        private ITimer TimeOutTimer { get; set; }
        private ITimer EndCompletedTimer { get; set; }

       
        public override void SendRequest()
        {
            if (!_table.TryAdd(this.GetId(), this))
                throw new Exception(
                    string.Format("Could not add client transaction. The id already exists. Id:'{0}'. .", this.GetId()));
            
            ChangeState(CallingState);

            SendRequestInternal();
        }

        public override void ProcessResponse(SipResponseEvent responseEvent)
        {
            if (_logger.IsDebugEnabled) _logger.Debug("InviteCtx[Id={0}]. Processing reponse[StatusCode:'{1}']", GetId(), responseEvent.Response.StatusLine.StatusCode);

            if (_logger.IsDebugEnabled) _logger.Debug("InviteCtx[Id={0}]. State '{1}' is handling response...", GetId(), State.Name);
            

            responseEvent.ClientTransaction = this;
            StateResult result;
            lock (_lock)
            {
                LatestResponse = responseEvent.Response;
                result = State.HandleResponse(this, responseEvent.Response);
            }

            if (_logger.IsDebugEnabled) _logger.Debug("InviteCtx[Id={0}]. Response handled by state. CurrentState:'{1}', InformToUser:'{2}', Dispose:'{3}'", GetId(), State.Name, result.InformToUser, result.Dispose);
            
            if (result.InformToUser)
            {
                if (GetDialog() != null)
                {
                    if (_logger.IsDebugEnabled) _logger.Debug("Tx is holding a dialog. Invoking ProcessResponse on Dialog.");
           
                    GetDialog().ProcessResponse(responseEvent);
                }
                else
                {
                    if (_logger.IsDebugEnabled) _logger.Debug("Passing response to listener: '{0}'", _listener.GetType().Name);

                    _listener.ProcessResponse(responseEvent);
                }
            }
            if(result.Dispose)
            {
                if (_logger.IsDebugEnabled) _logger.Debug("InviteCtx[Id={0}]. Disposing...", GetId());
            
                Dispose();

                if (_logger.IsDebugEnabled) _logger.Debug("InviteCtx[Id={0}]. Disposed.", GetId());
            }
        }
        
        internal void ChangeState(AbstractCtxState newstate)
        {
            if (_logger.IsDebugEnabled) _logger.Debug("InviteCtx[Id={0}]. State transition: '{1}'-->'{2}'", GetId(), State != null ? State.Name.ToString() : "", newstate.Name);

            /*attention: no locking. Is already called thread safely via call to AdaptToResponse*/
            State = newstate;
            newstate.Initialize(this);

           if(_stateObserver != null) _stateObserver.OnNext(CreateStateInfo(newstate.Name));
        }
        
        internal void SendAck()
        {
            var ackRequest = CreateAckRequest();
            _messageSender.SendRequest(ackRequest);
        }
        
        private SipRequest CreateAckRequest()
        {
            if (State != CompletedState) throw new InvalidOperationException(string.Format("The Tx is unable to create an 'ACK' Request. To be able to create an 'ACK' request, the Tx must be in 'Completed' state. CurrentState:{0}", State.Name));

            Check.Require(LatestResponse, "LatestResponse");

            var requestUri = Request.RequestLine.Uri.Clone();
            var callIdheader = (SipCallIdHeader)Request.CallId.Clone();
            var cseqHeader = _headerFactory.CreateSCeqHeader(SipMethods.Ack, Request.CSeq.Sequence);
            var fromHeader = (SipFromHeader)Request.From.Clone();
            var toHeader = (SipToHeader) LatestResponse.To.Clone();
            var viaHeader = (SipViaHeader) Request.Vias.GetTopMost().Clone();
            var maxForwardsHeader = _headerFactory.CreateMaxForwardsHeader();
            var ackRequest = _messageFactory.CreateRequest(
                requestUri,
                SipMethods.Ack,
                callIdheader,
                cseqHeader,
                fromHeader,
                toHeader,
                viaHeader,
                maxForwardsHeader);
            
            //TODO: not clear from the rfc if the routes have to be copied from request or response
            foreach (var route in Request.Routes)
            {
                ackRequest.Routes.Add((SipRouteHeader) route.Clone());
            }

            return ackRequest;
        }

        public SipRequest CreateCancelRequest()
        {
            Check.IsTrue(State != null, "The Tx is is unable to create an 'CANCEL' request while in 'NULL state'. A request has to be sent first by this Tx.");
            
            var requestUri = Request.RequestLine.Uri.Clone();
            var callIdheader = (SipCallIdHeader)Request.CallId.Clone();
            var cseqHeader = _headerFactory.CreateSCeqHeader(SipMethods.Cancel, Request.CSeq.Sequence);
            var fromHeader = (SipFromHeader)Request.From.Clone();
            var toHeader = (SipToHeader)Request.To.Clone();
            var viaHeader = (SipViaHeader)Request.Vias.GetTopMost().Clone();
            var maxForwardsHeader = _headerFactory.CreateMaxForwardsHeader();
            var cancelRequest = _messageFactory.CreateRequest(
                requestUri,
                SipMethods.Cancel,
                callIdheader,
                cseqHeader,
                fromHeader,
                toHeader,
                viaHeader,
                maxForwardsHeader);

            foreach (var route in Request.Routes)
            {
                cancelRequest.Routes.Add((SipRouteHeader)route.Clone());
            }

            return cancelRequest;
        }

        public override SipTransactionType Type
        {
            get { return SipTransactionType.InviteClient; }
        }
        
    }
}
