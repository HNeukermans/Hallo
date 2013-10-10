using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Threading;
using Hallo.Server;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.Sip.Stack.Transactions.NonInviteClient;
using Hallo.Sip.Stack.Transactions.NonInviteServer;
using Hallo.Sip.Util;
using Hallo.Sip.Stack;
using Hallo.Util;
using NLog;
using Hallo.Util;
using System.Reactive.Linq;
using System.Reactive;

namespace Hallo.Sip
{
    public class SipProvider : ISipProvider
    {
        private readonly ISipContextSource _contextSource;
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private ISipListener _sipListener = new NullListener();
        private readonly SipStack _stack;
        private volatile bool _isRunning;

        private readonly SipClientTransactionTable _ctxTable;
        private readonly SipServerTransactionTable _stxTable;
        private readonly SipDialogTable _dialogTable;
        private IObserver<SipMessage> _requestSentObserver;
        private IObserver<SipMessage> _responseSentObserver;
        private IObserver<SipMessage> _requestReceivedObserver;
        private IObserver<SipMessage> _responseReceivedObserver;
        private IExceptionHandler _exceptionHandler;

        #region props

        internal SipClientTransactionTable ClientTransactionTable
        {
            get { return _ctxTable; }
        }

        internal SipServerTransactionTable ServerTransactionTable
        {
            get { return _stxTable; }
        }

        internal SipDialogTable DialogTable
        {
            get { return _dialogTable; }
        }

        public SipListeningPoint ListeningPoint
        {
            get { return new SipListeningPoint(_contextSource.ListeningPoint); }
        }

        /// <summary>
        /// Gets the ClientTransactionTable. Only used for testing code.
        /// </summary>
        /// <returns></returns>
        internal SipClientTransactionTable GetClientTransactions()
        {
            return _ctxTable;
        }

        public ISipListener SipListener
        {
            get { return _sipListener; }
        }

        #endregion

        //want follow the jain-sip implementation. I just use a guid. SipUtil.CreateCallId();
        //public SipCallIdHeader GetNewCallId()
        //{
        //    String callId = SipUtil.GenerateCallIdentifier(this.getListeningPoint()
        //            .getIPAddress());
        //    SipCallIdHeader callid = new SipCallIdHeader();
            
        //    return callid;
        //}
        
        internal SipProvider(SipStack stack, ISipContextSource contextSource)
        {
            Check.Require(stack, "stack");
            Check.Require(contextSource, "contextSource");

            _stack = stack;
            _contextSource = contextSource;
            contextSource.NewContextReceived +=(s,e) => OnNewContextReceived(e.Context);
            contextSource.UnhandledException += (s, e) => OnContextSourceUnhandledException(e.Exception); 
            
            _ctxTable = new SipClientTransactionTable();
            _stxTable = new SipServerTransactionTable();
            _dialogTable = new SipDialogTable();
        }
        
        public void Start()
        {
            _logger.Trace("Starting SipProvider...");

            if (!_isRunning)
            {
                _contextSource.Start();
               _isRunning = true;
            }
            else
            {
                _logger.Warn("Start was called on an already started instance.");
            }
        }
        
        public void Stop()
        {
            _contextSource.Stop();
            if (_requestSentObserver != null) _requestSentObserver.OnCompleted();
            if (_requestReceivedObserver != null) _requestReceivedObserver.OnCompleted();
            if (_responseSentObserver != null) _responseSentObserver.OnCompleted();
            if (_responseReceivedObserver != null) _responseReceivedObserver.OnCompleted();
        }
        
        #region send methods
        
        public void SendRequest(SipRequest request)
        {
            Check.Require(request, "request");
            
            if (_logger.IsTraceEnabled)
                _logger.Trace("Sending request...");

            var result = new SipValidator().ValidateRequest(request);

            if (!result.IsValid) ThrowSipException(result);
            
            var via = request.Vias.GetTopMost();
            if (string.IsNullOrEmpty(via.Branch))
            {
                via.Branch = SipUtil.CreateBranch();
            }

            var bytes = SipFormatter.FormatMessage(request);

            SipUri sipUri = GetDestinationUri(request);

            IPEndPoint ipEndPoint = GetDestinationEndPoint(sipUri);

            SendBytes(bytes, ipEndPoint);

            if (_logger.IsDebugEnabled)
                _logger.Debug("Send request '{0}' --> {1}. # bytes:{2}.", request.RequestLine.Method, ipEndPoint, bytes.Length);

            if(_requestSentObserver != null) _requestSentObserver.OnNext(request);
        }

        public void SendResponse(SipResponse response)
        {
            Check.Require(response, "response");

            var result = new SipValidator().ValidateMessage(response);

            if (!result.IsValid) ThrowSipException(result);

            var sendToEndPoint = SipProvider.DetermineSendTo(response);

            var bytes = SipFormatter.FormatMessage(response);

            SendBytes(bytes, sendToEndPoint);
            
            if (_logger.IsDebugEnabled)
                _logger.Debug("Send response '{0}' --> {1}. # bytes:{2}.", response.StatusLine.StatusCode, sendToEndPoint, bytes.Length);

            if (_responseSentObserver != null) _responseSentObserver.OnNext(response);
        }

        private void SendBytes(byte[] bytes, IPEndPoint ipEndPoint)
        {
            _contextSource.SendTo(bytes, ipEndPoint);
        }

        #endregion

        #region factory methods

        public ISipClientTransaction CreateClientTransaction(SipRequest request)
        {
            Check.Require(request, "request");
            Check.IsTrue(SipMethods.IsMethod(request.RequestLine.Method), "Request method is not supported");

            ISipClientTransaction tx;

            if (request.RequestLine.Method == SipMethods.Ack)
            {
                throw new ArgumentException("Can not create a transaction for the 'ACK' request");
            }

            ISipListener txListener = _sipListener;
            
            SipAbstractDialog dialog;
            if (_dialogTable.TryGetValue(GetDialogId(request, true), out dialog))
            {
                txListener = dialog;
            }

            if(request.RequestLine.Method == SipMethods.Invite)
            {
                tx = new SipInviteClientTransaction(
                    ClientTransactionTable,
                    this,
                    txListener,
                    request, 
                    _stack.GetTimerFactory(),
                    _stack.CreateHeaderFactory(),
                    _stack.CreateMessageFactory());
            }
            else
            {
                tx = new SipNonInviteClientTransaction(
                    ClientTransactionTable,
                    this,
                    txListener,
                    request, 
                    _stack.GetTimerFactory());
            }

            return tx;
        }

        public ISipServerTransaction CreateServerTransaction(SipRequest request)
        {
            Check.Require(request, "request");
            Check.IsTrue(SipMethods.IsMethod(request.RequestLine.Method), "Request method is not supported");

            ISipServerTransaction tx;

            if (request.RequestLine.Method == SipMethods.Ack)
            {
                throw new ArgumentException("Can not create a transaction for the 'ACK' request");
            }

            ISipListener txListener = _sipListener;

            SipAbstractDialog dialog;
            if (_dialogTable.TryGetValue(GetDialogId(request, true), out dialog))
            {
                txListener = dialog;
            }

            if (request.RequestLine.Method == SipMethods.Invite)
            {
                tx = new SipInviteServerTransaction(
                    ServerTransactionTable,
                    this,
                    txListener,
                    request,
                    _stack.GetTimerFactory());
            }
            else
            {
                tx = new SipNonInviteServerTransaction(
                    ServerTransactionTable,
                    request,
                    txListener,
                    this,
                    _stack.GetTimerFactory());
            }

            return tx;
        }
       
        public SipInviteServerDialog CreateServerDialog(ISipServerTransaction transaction)
        {
            var inviteTx = transaction as SipInviteServerTransaction;
            
            Check.Require(transaction, "transaction");
            Check.IsTrue(inviteTx != null, "The transaction must be of type 'SipInviteServerTransaction'");
            Check.Require(inviteTx.Request, "The request can not be null");
            Check.Require(inviteTx.Request.From, "The From header can not be null");
            Check.NotNullOrEmpty(inviteTx.Request.From.Tag, "From tag");

            var dialog = new SipInviteServerDialog(
                transaction,
                _dialogTable,
                _stack.GetTimerFactory(), 
                _stack.CreateHeaderFactory(),
                _stack.CreateMessageFactory(),
                _stack.CreateAddressFactory(),
                this,
                _sipListener,
                _contextSource.ListeningPoint);

            //setting the dialog is done out of the transaction.
            //(otherwise you need to interface the SetDialog method, both on InviteClient & InviteServer transaction)
            //thus creating again two separate interfaces or adding the method on existing interface thereby poluting.
            inviteTx.SetDialog(dialog);
            return dialog;
        }

        public SipInviteClientDialog CreateClientDialog(ISipClientTransaction transaction)
        {
            var inviteTx = transaction as SipInviteClientTransaction;

            Check.Require(transaction, "transaction");
            Check.IsTrue(inviteTx != null, "The transaction must be of type 'SipInviteClientTransaction'");
            Check.Require(inviteTx.Request, "The request can not be null");
            Check.Require(inviteTx.Request.From, "The From header can not be null");
            Check.NotNullOrEmpty(inviteTx.Request.From.Tag, "From tag");

            var dialog = new SipInviteClientDialog(
                inviteTx, 
                _dialogTable,
                _stack.CreateHeaderFactory(),
                _stack.CreateMessageFactory(),
                _stack.CreateAddressFactory(),
                this,
                _sipListener,
                _contextSource.ListeningPoint);
            
            inviteTx.SetDialog(dialog);

            return dialog;
        }
        
        #endregion

        private void ThrowSipException(ValidateMessageResult result)
        {
            if (result.HasRequiredHeadersMissing)
            {
                throw new SipException(SipResponseCodes.x400_Bad_Request, "Missing '{0}' header.", result.MissingRequiredHeader);
            }
            if (result.HasUnSupportedSipVersion)
            {
                throw new SipException(SipResponseCodes.x504_Version_Not_Supported);
            }
            if (result.HasUnSupportedSipMethod)
            {
                throw new SipException(SipResponseCodes.x501_Not_Implemented);
            }
            var requestResult = result as ValidateRequestResult;

            if(requestResult != null)
            {
                if(requestResult.HasInvalidSCeqMethod)
                {
                    throw new SipException(SipResponseCodes.x400_Bad_Request, "Invalid CSeq method.");
                }
                if (requestResult.InviteHasNoContactHeader)
                {
                    throw new SipException(SipResponseCodes.x400_Bad_Request, "Invite must have a contact header.");
                }
            }
        }

        private IPEndPoint GetDestinationEndPoint(SipUri uri)
        {
            if (SipUtil.IsIPAddress(uri.Host))
            {
                return new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port);
            }

            throw new NotSupportedException(ExceptionMessage.NamedHostsAreNotSupported);
        }

        private SipUri GetDestinationUri(SipRequest request)
        {
            Check.Require(request, "request");

            var topMostRoute = request.Routes.GetTopMost();

            SipUri uri = null;
            if(topMostRoute != null)
            {
                if(_logger.IsDebugEnabled)_logger.Debug("Request has a route specified. Sending the request to topmost route !!");
                uri = topMostRoute.SipUri;
            }
            else
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Request has no route specified. Sending the request to request-uri");
                uri = request.RequestLine.Uri;
            }

            return uri;
        }
        
        private static IPEndPoint DetermineSendTo(SipResponse response)
        {
            var topMost = response.Vias.GetTopMost();

            int port = topMost.Rport != -1 ? topMost.Rport : topMost.SentBy.Port;
            IPAddress ipAddress = topMost.Received ?? topMost.SentBy.Address;

            return new IPEndPoint(ipAddress, port);
        }

        /// <summary>
        /// determines the send to
        /// </summary>
        /// <remarks>in case via header is missing returns the socket remote endpoint</remarks>
        private static IPEndPoint DetermineSendTo(SipContext context)
        {
            var response = context.Response;

            return DetermineSendTo(response);
        }

        public void AddSipListener(ISipListener sipListener)
        {
            Check.Require(sipListener, "sipListener");

            if (_sipListener.GetType() != typeof(NullListener)) throw new InvalidOperationException("A listener is already added.");

            _sipListener = sipListener;
        }

        public void AddExceptionHandler(IExceptionHandler handler)
        {
            Check.Require(handler, "handler");

            _exceptionHandler = handler;
        }
       
        public SipProviderDiagnosticInfo GetDiagnosticsInfo()
        {
            var info = new SipProviderDiagnosticInfo();
            info.BytesReceived = _contextSource.BytesReceived;
            info.PacketsReceived = _contextSource.PacketsReceived;
            info.BytesSent = _contextSource.BytesSent;
            info.PacketsSent = _contextSource.PacketsSent;
            info.PacketsReceived = _contextSource.PacketsReceived;
            info.ActiveThreads = (int)_contextSource.PerformanceCountersReader.ActiveThreads;
            info.InUseThreads = (int)_contextSource.PerformanceCountersReader.InUseThreads;
            info.WorkItemsProcessed = (int)_contextSource.PerformanceCountersReader.WorkItemsProcessed;
            info.WorkItemsQueued = (int)_contextSource.PerformanceCountersReader.WorkItemsQueued;
            info.AvgExecutionTime = _contextSource.PerformanceCountersReader.AvgExecutionTime;
            info.MaximumExecutionTime = _contextSource.PerformanceCountersReader.MaximumExecutionTime;
            info.MaximumQueueWaitTime = _contextSource.PerformanceCountersReader.MaximumQueueWaitTime;
            return info;
        }

        public IObservable<Timestamped<SipTransactionStateInfo>> ObserveTxDiagnosticsInfo()
        {
            var co = _ctxTable.Observe().SelectMany(f => f);
            var so = _stxTable.Observe().SelectMany(f => f);
            return so.Merge(co);
        }

        public IObservable<SipMessage> ObserveMesssageDiagnosticsInfo()
        {
            var rqo = Observable.Create<SipMessage>((o) => 
            { 
                _requestSentObserver = o;
                return Disposable.Empty;
            });

            var rso = Observable.Create<SipMessage>((o) =>
            {
                _responseSentObserver = o;
                return Disposable.Empty;
            });

            var rro = Observable.Create<SipMessage>((o) =>
            {
                _responseReceivedObserver = o;
                return Disposable.Empty;
            });

            var rqro = Observable.Create<SipMessage>((o) =>
            {
                _requestReceivedObserver = o;
                return Disposable.Empty;
            });

            return Observable.Merge(rqo, rso, rro, rqro);
        }
        
        /// <summary>
        /// Only use this if NAT traversal is to be supported. Sets the Received + Rport parameters
        /// </summary>
        /// <param name="context"></param>
        internal static void SupportFirewalTraversal(SipContext context)
        {
            Check.Require(context, "context");
            Check.Require(context.Request, "context.Request");
            Check.Require(context, "context");

            var request = context.Request;
            var topMostVia = request.Vias.GetTopMost();
            
            //Support NAT firewal
            if (!topMostVia.SentBy.Address.Equals(context.RemoteEndPoint.Address))
            {
                topMostVia.Received = context.RemoteEndPoint.Address;
            }

            //Support PAT firewall. Rfc3581 is not supported. TODO: remove.
            if (topMostVia.UseRport)
            {
                topMostVia.Rport = context.RemoteEndPoint.Port;
            }
        }

        #region OnIncomingContext
        
        private void OnIncomingResponseContext(SipContext context)
        {
            if (_responseReceivedObserver != null) _responseReceivedObserver.OnNext(context.Response);

            ISipResponseProcessor sipResponseProcessor = _sipListener;

            var responseEvent = new SipResponseEvent(context);
            
             //get dialog. if dialog found, the listener for the tx is the dialog.
            SipAbstractClientTransaction ctx;

            if (_logger.IsDebugEnabled) _logger.Debug("Searching the table for a matching tx..");
            
            if(_ctxTable.TryGetValue(GetClientTransactionId(responseEvent.Response), out ctx))
            {
                if (_logger.IsTraceEnabled) _logger.Trace("Found a matching tx. Setting it as the responseProcessor.");

                sipResponseProcessor = ctx;

                if (_logger.IsDebugEnabled) _logger.Debug("Setting the Dialog on responseEvent.");

                SipAbstractDialog found;
                /*dialog can come from cxt or table*/
                if(IsDialogInitiatingRequest(ctx.Request))
                {
                    if (_logger.IsDebugEnabled) _logger.Debug("Received a response, for a clientransaction. Getting dialog of the Ctx.");
                    found = ((SipInviteClientTransaction) ctx).GetDialog();
                }
                else
                {
                    if (_logger.IsDebugEnabled) _logger.Debug("Searching the table for a matching dialog...");
                    if(_dialogTable.TryGetValue(GetDialogId(responseEvent.Response, false), out found))
                    {
                        if (_logger.IsDebugEnabled) _logger.Debug("Found a matching dialog.");
                    }
                    //stx.SetDialog((SipInviteServerDialog)inTableDialog);
                }
                responseEvent.Dialog = found;

                if (_logger.IsDebugEnabled) _logger.Debug("Dialog on responseEvent set. Dialog is null:{0}", responseEvent.Dialog == null);
            }
            else
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Could not find a matching tx..");

                if (_logger.IsDebugEnabled) _logger.Debug("Searching the table for a matching dialog...");

                /*try dialog as processor*/
                SipAbstractDialog dialog;
                if (_dialogTable.TryGetValue(GetDialogId(responseEvent.Response, false), out dialog))
                {
                    if (_logger.IsTraceEnabled) _logger.Trace("Found a matching dialog. Setting it as the responseProcessor.");
                    
                    sipResponseProcessor = dialog;
                    responseEvent.Dialog = dialog;
                    //stx.SetDialog((SipInviteServerDialog)inTableDialog);
                }
                else
                {
                    if (_logger.IsTraceEnabled) _logger.Trace("Could not find a matching dialog. Using the SipProvider's SipListener as the responseProcessor.");
                }
            }

            try
            {
                sipResponseProcessor.ProcessResponse(responseEvent);
            }
            catch (Exception err)
            {
                _logger.ErrorException("Response failed.", err);
                throw;
            }
        }

        private void OnIncomingRequestContext(SipContext context)
        {
            if (_requestReceivedObserver != null) _requestReceivedObserver.OnNext(context.Request);

            if (_logger.IsDebugEnabled) _logger.Debug("Validating the request...");

            var result = new SipValidator().ValidateMessage(context.Request);
            
            if (!result.IsValid)
            {
                if (_logger.IsDebugEnabled) _logger.Debug("The received request is not valid. Throwing a sip exception.");
                
                ThrowSipException(result);
            }

            if (_logger.IsDebugEnabled) _logger.Debug("Request valid.");

            /*firewall traversal not supported. SupportFirewalTraversal(context);*/

            var requestEvent = new SipRequestEvent(context);

            ISipRequestProcessor sipRequestProcessor = _sipListener;

            //8.2.2.2: merged requests, not implemented
            
            SipAbstractServerTransaction stx;

            if (_logger.IsDebugEnabled) _logger.Debug("Searching the table for a matching tx..");

            if(_stxTable.TryGetValue(GetServerTransactionId(context.Request), out stx))
            {
                if (_logger.IsTraceEnabled) _logger.Trace("Found a matching tx. Setting it as the requestprocessor.");

                sipRequestProcessor = stx;
                
                //if (IsDialogInitiatingRequest(stx.Request))
                //{
                    //if (_logger.IsDebugEnabled) _logger.Debug("The received request is an 'INVITE' request. Try setting the dialog on tx...");

                    //set the dialog, so it can initiate
                    SipAbstractDialog found;
                    if(TryGetDialogOnTx((SipInviteServerTransaction)stx, context.Request, out found))
                    {
                        requestEvent.Dialog = found;
                    }
                        
                    
                //}
            }
            else
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Could not find a matching tx..");

                if (_logger.IsDebugEnabled) _logger.Debug("Searching the table for a matching dialog...");
            
                SipAbstractDialog dialog;
                if (_dialogTable.TryGetValue(GetDialogId(context.Request, true), out dialog))
                {
                    if (_logger.IsTraceEnabled) _logger.Trace("Found a matching dialog. Setting it as the requestprocessor.");

                    sipRequestProcessor = dialog;
                    requestEvent.Dialog = dialog;
                }
                else
                {
                    if (_logger.IsTraceEnabled) _logger.Trace("Could not find a matching dialog. Using the SipProvider's SipListener as the requestprocessor.");
                }
            }
            
            try
            {
                sipRequestProcessor.ProcessRequest(requestEvent);

                if (requestEvent.IsSent)
                {
                    if (_logger.IsDebugEnabled) _logger.Debug("Processed '{0}' request. The response has already been sent. Skipping automatic response sending.", requestEvent.Request.RequestLine.Method);
                    return;
                }

                if (!ShouldHaveResponse(requestEvent.Request))
                {
                    if(_logger.IsDebugEnabled) _logger.Debug("Processed '{0}' request. The request does not require a response. Skipping automatic response sending.", requestEvent.Request.RequestLine.Method);
                    return;
                }

                if(requestEvent.Response == null)
                    throw new SipCoreException("Response to send can not be null. The ProcessRequest method is supposed to create a response message that is to be sent.");

                if (!requestEvent.IsSent)
                {
                    var response = requestEvent.Response;

                    var remoteEndPoint = SipProvider.DetermineSendTo(response);

                    _contextSource.SendTo(SipFormatter.FormatMessage(response), remoteEndPoint);

                    if (_responseSentObserver != null) _responseSentObserver.OnNext(response);
                }
            }
            catch (SipCoreException coreException)
            {
                throw coreException;
            }
            catch (SipException sipException)
            {
                if(ShouldHaveResponse(requestEvent.Request))
                    SendErrorResponse(sipException, requestEvent);
            }
            catch (Exception err)
            {
                _logger.ErrorException("Request failed.", err);

                try
                {
                    if (ShouldHaveResponse(requestEvent.Request))
                        SendErrorResponse(err, requestEvent);
                }
                catch (Exception errr)
                {
                    _logger.Debug("Failed to send the response.", errr);
                }
            }
        }

        private bool IsDialogInitiatingRequest(SipRequest message)
        {
            return message.RequestLine.Method == SipMethods.Invite;
        }

        private bool ShouldHaveResponse(SipRequest request)
        {
            return request.RequestLine.Method != SipMethods.Ack;
        }

        #endregion
        
        private bool TryGetDialogOnTx(SipInviteServerTransaction stx, SipRequest request, out SipAbstractDialog dialog)
        {
            dialog = null;

            if (request.To.Tag == null) return false;

            /* merged request: Based on the To tag, the UAS MAY either accept or reject the request.
             * If the request has a tag in the To header field, 
             * but the dialog identifier does not match any existing dialogs*/

            if(_logger.IsDebugEnabled)_logger.Debug("Searching the table for a matching dialog...");
            
            SipAbstractDialog inTableDialog;
            if (_dialogTable.TryGetValue(GetDialogId(request, true), out inTableDialog))
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Found a matching dialog. Setting it on tx.");
                //stx.SetDialog((SipInviteServerDialog)inTableDialog);
                dialog = inTableDialog;
                return true;
            }
            else
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Could not find a matching dialog.");
                /* If the UAS wishes to reject the request because it does not wish to
                   recreate the dialog, it MUST respond to the request with a 481
                   (Call/Transaction Does Not Exist) status code and pass that to the
                   server transaction. 
                */
                return false;
            }
        }

        internal static string GetClientTransactionId(SipMessage message)
        {
            return message.Vias.GetTopMost().Branch + "-" + message.CSeq.Sequence;
        }

        /// <summary>
        /// Determines the condition under which a request matches a server transactions, according to rfc 3261 (17.2.3)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal static string GetServerTransactionId(SipRequest request)
        {
            string idMethod = request.RequestLine.Method;
            /*for an incoming ACK request we must take INVITE, bc it needs to match the INVITE server transaction it is going to acknowledge */
            if (request.RequestLine.Method.Equals(SipMethods.Ack)) idMethod = SipMethods.Invite;
            
            /*the request that created the server transaction + incoming requests get applied this method*/
            return request.Vias.GetTopMost().Branch + "-" + request.Vias.GetTopMost().SentBy + "-" + idMethod;
        }
        
        private void OnNewContextReceived(SipContext context)
        {
            if (context.Request != null)
            {
                OnIncomingRequestContext(context);
            }
            else
            {
                OnIncomingResponseContext(context);
            }
        }

        /// <summary>
        /// processes unhandled exceptions caught by the SipContextSource
        /// </summary>
        private void OnContextSourceUnhandledException(Exception exception)
        {
            if(_exceptionHandler != null)
                _exceptionHandler.Handle(exception);
        }

        private void SendErrorResponse(Exception exception, SipRequestEvent requestEvent)
        {
            Check.Require(exception, "exception");
            Check.Require(requestEvent, "requestEvent");
            Check.Require(requestEvent.Request, "requestEvent.Request");

            var request = requestEvent.Request;
            var sipException = exception as SipException;

            string responseCode = sipException != null ? sipException.ResponseCode : SipResponseCodes.x500_Server_Internal_Error;
            SipResponse response = sipException != null ? _stack.CreateMessageFactory().CreateResponse(request, responseCode) : 
                _stack.CreateMessageFactory().CreateResponse(request, responseCode + ", " + exception.Message);
            
            if(requestEvent.ServerTransaction != null)
            {
                requestEvent.ServerTransaction.SendResponse(response);
            }
            else if (response.Vias.GetTopMost() != null)
            {
                IPEndPoint ipEndPoint = SipProvider.DetermineSendTo(response);

                try
                {
                    _contextSource.SendTo(SipFormatter.FormatMessage(response), ipEndPoint);
                    if (_responseSentObserver != null) _responseSentObserver.OnNext(response);
                }
                catch (SocketException err)
                {
                    _logger.Error("Failed to send response to " + ipEndPoint.ToString(), err);
                }
            }
            else
            {
                _logger.Warn("Response can not be sent. Via TopMost header missing.");
            }

           
        }
        
        internal static string GetDialogId(SipMessage message, bool isServer)
        {
            Check.Require(message, "message");
           
            string localTag = isServer ? message.To.Tag : message.From.Tag;
            string remoteTag = isServer ? message.From.Tag : message.To.Tag;

            return message.CallId.Value + ":" + localTag + ":" + remoteTag;
        }
    }

    
}