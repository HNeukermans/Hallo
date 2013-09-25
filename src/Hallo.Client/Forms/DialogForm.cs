using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Hallo.Client.Logic;
using Hallo.Component;
using Hallo.Component.Logic;
using Hallo.Component.Logic.Extensions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.Sip.Util;
using Hallo.Util;

namespace Hallo.Client.Forms
{
    public enum PhoneState
    {
        Idle,
        Ringing,
        WaitingForAck,
        CallerEstablished,
        CalleeEstablished,
        WaitForProvisional,
        WaitForFinal,
        WaitingForByeOK
    }

    
    public partial class DialogForm : ClientChildForm, ISipListener
    {
        private PhoneState _state = PhoneState.Idle;
        private SipAbstractDialog _dialog;

        private int commandSequence;

        public DialogForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(DialogForm_Load);
            this.Closing += new CancelEventHandler(DialogForm_Closing);
        }

        void DialogForm_Closing(object sender, CancelEventArgs e)
        {
            MainForm.MainSipListener.UnRegisterListener();
        }

        void DialogForm_Load(object sender, EventArgs e)
        {
            _state = PhoneState.Idle;
            _txtFromAlias.Text = "alice";
            _txtToAlias.Text = "bob";
            _chkPeerToPeer.Checked = false;
            _chkPeerToPeer_CheckedChanged(null,null);
            _txtState.Text = StateToLabel(_state);
            MainForm.MainSipListener.RegisterListener(this);
            if(_chbStubTimerFactory.Checked) MainForm.SipStack.SetTimerFactory(new TimerFactoryStubBuilder().Build());
        }

        private String StateToLabel(PhoneState state)
        {
            return _state.ToString().ToUpper();
        }

        private void Invite()
        {
                FormHelper.ValidateCondition(SipUtil.IsSipUri(_txtFromUri.Text), "From-uri");

                FormHelper.ValidateCondition(SipUtil.IsSipUri(_txtToUri.Text), "To-uri");

                var fromUri = SipUtil.ParseSipUri(_txtFromUri.Text);
                var toUri = SipUtil.ParseSipUri(_txtToUri.Text);

                if (_chkPeerToPeer.Checked) FormHelper.ValidateCondition(SipUtil.IsIpEndPoint(toUri.Host), "To-uri");

                var requestUri = toUri;
                var toAddress = AddressFactory.CreateAddress(_txtToAlias.Text, toUri);
                var fromAddress = AddressFactory.CreateAddress(_txtFromAlias.Text, fromUri);
                var toHeader = HeaderFactory.CreateToHeader(toAddress);
                var fromHeader = HeaderFactory.CreateFromHeader(fromAddress, SipUtil.CreateTag());
                var cseqHeader = HeaderFactory.CreateSCeqHeader(SipMethods.Invite, ++commandSequence);
                var callIdheader = HeaderFactory.CreateCallIdHeader(SipUtil.CreateCallId());
                var viaHeader = HeaderFactory.CreateViaHeader(SipProvider.ListeningPoint.Address,
                                                              SipProvider.ListeningPoint.Port, SipConstants.Udp,
                                                              SipUtil.CreateBranch());
                var maxForwardsHeader = HeaderFactory.CreateMaxForwardsHeader(1);
                var request = MessageFactory.CreateRequest(
                    requestUri,
                    SipMethods.Invite,
                    callIdheader,
                    cseqHeader,
                    fromHeader,
                    toHeader,
                    viaHeader,
                    maxForwardsHeader);

                /*add routes and contacts*/
                if (!_chkPeerToPeer.Checked)
                {
                    var proxyServerUri = AddressFactory.CreateUri(null, MainForm.SipStack.OutBoundProxy.ToString());
                    proxyServerUri.IsLooseRouting = true;
                    var routeHeader = HeaderFactory.CreateRouteHeader(proxyServerUri);
                    request.Routes.Add(routeHeader);
                }
                var contactUri = AddressFactory.CreateUri(fromUri.User, viaHeader.SentBy.ToString());
                var contactHeader = HeaderFactory.CreateContactHeader(contactUri);
                request.Contacts.Add(contactHeader);

                var transaction = SipProvider.CreateClientTransaction(request);
                _dialog = SipProvider.CreateClientDialog(transaction);
                transaction.SendRequest();
        }

        private void RefreshDialogForm(SipAbstractDialog dialog)
        {
            if (dialog == null) return;
            if (dialog.State != DialogState.Null)
            {
                _txtCallId.Text = dialog.CallId;
                _txtLocalTag.Text = dialog.LocalTag;
                _txtRemoteTag.Text = dialog.RemoteTag;
                _txtLocalSeqNr.Text = dialog.LocalSequenceNr.ToString();
                _txtRemoteSeqNr.Text = dialog.RemoteSequenceNr.ToString();
                _txtLocalUri.Text = dialog.LocalUri.FormatToString();
                _txtRemoteUri.Text = dialog.RemoteUri.FormatToString();
                _txtRemoteTarget.Text = dialog.RemoteTarget.FormatToString();
                StringBuilder sb = new StringBuilder();
                dialog.RouteSet.ForEach(r => sb.AppendLine(r.FormatBodyToString()));
                _txtRouteSet.Text = sb.ToString();
            }
            else
            {
                _txtCallId.Text = _txtLocalTag.Text = _txtRemoteTag.Text = _txtLocalSeqNr.Text =
               _txtRemoteSeqNr.Text =_txtLocalUri.Text = _txtRemoteUri.Text =_txtRemoteTarget.Text = "NULL";
                _txtRouteSet.Text = "NULL";
            }
        }

        private void SendAck(SipInviteClientDialog dialog)
        {
            var ackRequest = dialog.CreateAck();
            dialog.SendAck(ackRequest);
        }
        
        private void _btnCallerSendCancel_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //var ackRequest = _clientDialog.();
            //var transaction = SipProvider.CreateClientTransaction(ackRequest);
            //_clientDialog.SendRequest(transaction);
        }

        public void GoToState(PhoneState state)
        {
            _state = state;
            this.OnUIThread(() =>
                                {
                                    _txtState.Text = StateToLabel(_state);

                                    if (_state == PhoneState.WaitForProvisional)
                                    {
                                        //_btnPhone.Enabled = false;
                                    }
                                    if (_state == PhoneState.Ringing)
                                    {
                                        _btnPhone.Enabled = true;
                                        _btnPhone.Text = "Answer";
                                    }
                                    if (_state == PhoneState.CallerEstablished)
                                    {
                                        _btnPhone.Enabled = true;
                                        _btnPhone.Text = "End Call";
                                    }

                                    //if (_state == PhoneState.Idle)
                                    //{
                                    //    Foreach(_grpbForm.Controls, (c) => c.Enabled = true);
                                    //}
                                    //else
                                    //{
                                    //    Foreach(_grpbForm.Controls, (c) => c.Enabled = false);
                                    //}

                                    //if (_state == PhoneState.Idle)
                                    //{
                                    //    Foreach(_grpbCalleeActions.Controls, (c) => c.Enabled = true);
                                    //    Foreach(_grpbCallerActions.Controls, (c) => c.Enabled = true);
                                    //}
                                    //else if (_state == PhoneState.WaitForProvisional)
                                    //{
                                    //    Foreach(_grpbCalleeActions.Controls, (c) => c.Enabled = false);
                                    //    _btnInvite.Enabled = false;
                                    //}
                                    //else if (_state == PhoneState.WaitForFinal)
                                    //{

                                    //}
                                    //else if (_state == PhoneState.CallerEstablished)
                                    //{
                                    //    _btnCallerSendBye.Enabled = true;
                                    //}
                                    //else if (_state == PhoneState.Ringing)
                                    //{
                                    //    Foreach(_grpbCallerActions.Controls, (c) => c.Enabled = false);
                                    //    _btnCalleeSendBye.Enabled = false;
                                    //    _btnCalleeSendCancel.Enabled = _btnCalleeSendOk.Enabled = true;
                                    //}
                                    //else if (_state == PhoneState.WaitingForAck)
                                    //{
                                    //    _btnCalleeSendCancel.Enabled = false;
                                    //}
                                    //else if (_state == PhoneState.CalleeEstablished)
                                    //{
                                    //    _btnCalleeSendBye.Enabled = true;
                                    //}

                                });


        }

        AutoResetEvent _waitHandle = new AutoResetEvent(false);
        private SipRequestEvent _pendingRequestEvent;
        private SipInviteServerTransaction _inviteTransaction;
        private SipResponse _ringingResponse;
        private SipRequest _inviteRequest;
        private IDisposable _ringingRetransmitSubscription;

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            Log("Received a request. RequestLine:'{0}'", requestEvent.Request.RequestLine.FormatToString());

            if (requestEvent.Request.RequestLine.Method == SipMethods.Invite)
            {
                Log("Received an INVITE request.");
                if (_state == PhoneState.Idle)
                {
                    _inviteRequest = requestEvent.Request;
                    _ringingRetransmitSubscription = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(5)).ObserveOn(this).Subscribe((i) =>
                    {
                        Log("Sending a ringing response...");
                        SendRinging();
                        Log("Ringing response send.");
                        if(i == 0)
                        {
                            GoToState(PhoneState.Ringing);
                            RefreshDialogForm(_dialog);
                        }
                    });
                }
                else
                {
                    Log("Currently busy...");
                    //send busy here
                }
            }


            if(_state == PhoneState.Idle)
            {
                if (requestEvent.Request.RequestLine.Method == SipMethods.Invite)
                {
                    GoToState(PhoneState.Ringing);
                    _pendingRequestEvent = requestEvent;
                    
                    this.OnUIThread(() =>
                                        {
                                            //show the name of the caller in the from textboxes.
                                             _txtFromAlias.Text = requestEvent.Request.From.DisplayInfo;
                                             _txtFromAlias.Text = requestEvent.Request.From.DisplayInfo;
                                             //clear all other from-to textboxes
                                            _txtFromUri.Text = ""; requestEvent.Request.From.SipUri.FormatToString();
                                            _txtToUri.Text = _txtToAlias.Text = "";
                                        });
                    
                    //block the processing thread
                    _waitHandle.WaitOne(30*1000);
                }
            }
            else if (_state == PhoneState.Ringing)
            {
                if (requestEvent.Request.RequestLine.Method == SipMethods.Cancel)
                {
                    //TODO: send 487 + stop ringing
                    GoToState(PhoneState.Idle);
                }
            }
            else if (_state == PhoneState.WaitingForAck)
            {
                if (requestEvent.Request.RequestLine.Method == SipMethods.Ack)
                {
                    Log("Received an ACK request. Going to 'CALLERESTABLISHED' state");
                    GoToState(PhoneState.CallerEstablished);
                }
            }
            else if (_state == PhoneState.CallerEstablished)
            {
                if (requestEvent.Request.RequestLine.Method == SipMethods.Bye)
                {
                    Debugger.Break();
                    Log("Received an BYE request.");
                    Log("Sending a OK response...");
                    /*send ok*/
                    var okResponse = requestEvent.Request.CreateResponse(SipResponseCodes.x200_Ok);
                    var serverTransaction = SipProvider.CreateServerTransaction(requestEvent.Request);
                    serverTransaction.SendResponse(okResponse);
                    Log("OK response send.");
                    if (requestEvent.Dialog != null)
                    {
                        requestEvent.Dialog.Terminate();
                        Log("Terminating dialog.");
                    }
                    GoToState(PhoneState.Idle);
                }
            }
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
            Log("Received a response. Status:'{0}'", responseEvent.Response.StatusLine.FormatToString());
            
            var div100 = responseEvent.Response.StatusLine.StatusCode / 100;
            if (div100 == 1)
            {
                Log("Received a provisional response. Waiting for a final response");
                GoToState(PhoneState.WaitForFinal);;
                RefreshDialogForm(responseEvent.Dialog);
            }
            else if (div100 == 2)
            {
                Log("Received a OK response. Sending Ack...");
                SendAck(_dialog as SipInviteClientDialog);
                Log("Ack send. Call established.");
                GoToState(PhoneState.CallerEstablished);
                RefreshDialogForm(responseEvent.Dialog);
            }
            else if (div100 > 2)
            {
                Log("Received a final response other then OK. Terminating dialog...");
                if (responseEvent.Dialog != null)
                {
                    responseEvent.Dialog.Terminate();
                    GoToState(PhoneState.Idle);
                    Log("Dialog terminated. Phone in idle state");
                }
                
            }
        }

        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {
            throw new NotImplementedException();
        }

        private void _chbUseTx_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void _chbStubTimerFactory_CheckedChanged(object sender, EventArgs e)
        {
            if (_chbStubTimerFactory.Checked)
            {
                MainForm.SipStack.SetTimerFactory(new TimerFactoryStubBuilder().Build());
            }
            else
            {
                MainForm.SipStack.SetTimerFactory(null);
            }
        }

        private void _chkPeerToPeer_CheckedChanged(object sender, EventArgs e)
        {
            _txtFromUri.Text = _chkPeerToPeer.Checked ? string.Format("sip:alice@{0}", MainForm.SipProvider.ListeningPoint) : "sip:alice@ocean.com";
            _txtToUri.Text = _chkPeerToPeer.Checked ? string.Format("sip:bob@{0}",
                new IPEndPoint(MainForm.SipProvider.ListeningPoint.Address, MainForm.SipProvider.ListeningPoint.Port + 1)) : "sip:bob@ocean.com";
        }

        private void Foreach(Control.ControlCollection controls, Action<Control> action)
        {
            foreach (Control control in controls)
            {
                action(control);
            }
        }

        private void SendRinging()
        {
            if (_ringingResponse == null)
            {
                _ringingResponse = _inviteRequest.CreateResponse(SipResponseCodes.x180_Ringing);
                _ringingResponse.To.Tag = SipUtil.CreateTag();
                var contactUri = AddressFactory.CreateUri("", MainForm.SipProvider.ListeningPoint.ToString());
                _ringingResponse.Contacts.Add(HeaderFactory.CreateContactHeader(contactUri));

                _inviteTransaction =
                    (SipInviteServerTransaction)SipProvider.CreateServerTransaction(_inviteRequest);
                _dialog = SipProvider.CreateServerDialog(_inviteTransaction);
                _inviteTransaction.SendResponse(_ringingResponse);
            }
            else
            {
                _inviteTransaction.SendResponse(_ringingResponse);
            }
        }

        private void SendBye(SipAbstractDialog dialog)
        {
            var byeRequest = dialog.CreateRequest(SipMethods.Bye);
            var ctx = SipProvider.CreateClientTransaction(byeRequest);
            dialog.SendRequest(ctx);
        }

        private void SendOk()
        {
            var okResponse = MessageFactory.CreateResponse(_pendingRequestEvent.Request, SipResponseCodes.x200_Ok);
            okResponse.To.Tag = _dialog.LocalTag;
            _inviteTransaction.SendResponse(okResponse);

        }

        private void _btnPhone_Click(object sender, EventArgs e)
        {
            if (_state == PhoneState.Idle || _state == PhoneState.WaitForProvisional)
            {
                Log("Sending INVITE...");
                Invite();
                Log("INVITE send. Waiting for provisional response");
                RefreshDialogForm(_dialog);
                GoToState(PhoneState.WaitForProvisional);
            }
            else if(_state == PhoneState.Ringing)
            {
                if (_ringingRetransmitSubscription != null) _ringingRetransmitSubscription.Dispose();
                Log("Sending OK...");
                SendOk();
                Log("OK send. Waiting for ACK...");
                RefreshDialogForm(_dialog);
                GoToState(PhoneState.WaitingForAck);
            }
            else if (_state == PhoneState.CallerEstablished)
            {
                Log("Sending Bye...");
                SendBye(_dialog);
                Log("Bye send. Waiting for OK...");
                RefreshDialogForm(_dialog);
                GoToState(PhoneState.WaitingForByeOK);
            }
            else if (_state == PhoneState.CalleeEstablished)
            {
                Log("Sending Bye...");
                SendBye(_dialog);
                Log("Bye send. Waiting for OK...");
                RefreshDialogForm(_dialog);
                GoToState(PhoneState.WaitingForByeOK);
            }
        }

        
        private void Log(string message, params object[] args)
        {
            _txtLog.Text += string.Format(message, args) + "\r\n";
        }
    }
    //public void ProcessRequest(SipRequestEvent requestEvent)
    //{
    //        if(_state == PhoneState.Idle)
    //        {
    //            if (requestEvent.Request.RequestLine.Method == SipMethods.Invite)
    //            {
    //                //TODO: send 180 + ring the phone.

    //                _state = PhoneState.Ringing;
    //            }
    //        }
    //        else if(_state == PhoneState.Ringing)
    //        {
    //            if (requestEvent.Request.RequestLine.Method == SipMethods.Cancel)
    //            {
    //                //TODO: send 487 + stop ringing
    //                _state = PhoneState.Idle;
    //            }
    //        }
    //        else if(_state == PhoneState.WaitingForAck)
    //        {
    //            if (requestEvent.Request.RequestLine.Method == SipMethods.Ack &&
    //                    requestEvent.Request.CSeq.Command == SipMethods.Invite)
    //            {
    //                _state = PhoneState.Established;
    //            }
    //        }
    //        else if(_state == PhoneState.Established)
    //        {
    //            if (requestEvent.Request.RequestLine.Method == SipMethods.Bye)
    //            {
    //                //TODO: send OK
    //                _state = PhoneState.Idle;
    //            }
    //        }

    //    //var inviteTransaction = (SipInviteServerTransaction)_provider.CreateServerTransaction(requestEvent.Request);
    //        //var dialog = _provider.CreateServerDialog(inviteTransaction);
    //        ///*let the phone of the receiver ring to indicate someone is calling to you+
    //        // send back a ringing response to inform to callee, you received the invite*/
    //        //WavePlayer player = new WavePlayer(AudioOut.Devices[0]);
    //        //player.Play(ResManager.GetStream("ringing.wav"), 10);
    //        //var response = CreateRingingResponse(requestEvent.Request);
    //        //inviteTransaction.SendResponse(response);

    //        //start timer that does the above untill the dialog state != Early
    //        //the listener is in some state. In this state the callee can answer or cancel the call.
    //        //when it does it goes to another state.
    //}

    //public void ProcessResponse(SipResponseEvent responseEvent)
    //{
    //    if(_state == PhoneState.WaitForProvisional)
    //    {
    //        var mod100 = responseEvent.Response.StatusLine.StatusCode%100;
    //        if(mod100 == 1)
    //        {
    //            _state = PhoneState.WaitForFinal;
    //        }
    //        else if(mod100 == 2)
    //        {
    //            _state = PhoneState.Established;
    //        }
    //    }
    //}

}
