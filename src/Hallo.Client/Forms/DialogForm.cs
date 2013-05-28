using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
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
        WaitForFinal
    }

    
    public partial class DialogForm : ClientChildForm, ISipListener
    {
        private PhoneState _state = PhoneState.Idle;
        private SipInviteClientDialog _clientDialog;
        private SipInviteServerDialog _serverDialog;

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

        private void _btnInvite_Click(object sender, EventArgs e)
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
                _clientDialog = SipProvider.CreateClientDialog(transaction);
                transaction.SendRequest();

                RefreshDialogForm(_clientDialog);
                GoToState(PhoneState.WaitForProvisional);
        }

        private void RefreshDialogForm(AbstractDialog dialog)
        {
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

        private void _btnSendAck_Click(object sender, EventArgs e)
        {
            var ackRequest = _clientDialog.CreateAck();
            var transaction = SipProvider.CreateClientTransaction(ackRequest);
            _clientDialog.SendRequest(transaction);
            RefreshDialogForm(_clientDialog);
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

                                    if (_state == PhoneState.Idle)
                                    {
                                        Foreach(_grpbForm.Controls, (c) => c.Enabled = true);
                                    }
                                    else
                                    {
                                        Foreach(_grpbForm.Controls, (c) => c.Enabled = false);
                                    }

                                    if (_state == PhoneState.Idle)
                                    {
                                        Foreach(_grpbCalleeActions.Controls, (c) => c.Enabled = true);
                                        Foreach(_grpbCallerActions.Controls, (c) => c.Enabled = true);
                                    }
                                    else if (_state == PhoneState.WaitForProvisional)
                                    {
                                        Foreach(_grpbCalleeActions.Controls, (c) => c.Enabled = false);
                                        _btnInvite.Enabled = false;
                                    }
                                    else if (_state == PhoneState.WaitForFinal)
                                    {

                                    }
                                    else if (_state == PhoneState.CallerEstablished)
                                    {
                                        _btnCallerSendBye.Enabled = true;
                                    }
                                    else if (_state == PhoneState.Ringing)
                                    {
                                        Foreach(_grpbCallerActions.Controls, (c) => c.Enabled = false);
                                        _btnCalleeSendBye.Enabled = false;
                                        _btnCalleeSendCancel.Enabled = _btnCalleeSendOk.Enabled = true;
                                    }
                                    else if (_state == PhoneState.WaitingForAck)
                                    {
                                        _btnCalleeSendCancel.Enabled = false;
                                    }
                                    else if (_state == PhoneState.CalleeEstablished)
                                    {
                                        _btnCalleeSendBye.Enabled = true;
                                    }

                                });


        }

        AutoResetEvent _waitHandle = new AutoResetEvent(false);
        private SipRequestEvent _pendingRequestEvent;
        private SipInviteServerTransaction _inviteTransaction;
        private SipResponse _ringingResponse;

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            if(_state == PhoneState.Idle)
            {
                if (requestEvent.Request.RequestLine.Method == SipMethods.Invite)
                {
                    GoToState(PhoneState.Ringing);
                    _pendingRequestEvent = requestEvent;
                    
                    this.OnUIThread(() =>
                                        {
                                            //show the name of the caller
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
                if (requestEvent.Request.RequestLine.Method == SipMethods.Ack &&
                        requestEvent.Request.CSeq.Command == SipMethods.Invite)
                {
                    GoToState(PhoneState.CallerEstablished);
                }
            }
            else if (_state == PhoneState.CallerEstablished)
            {
                if (requestEvent.Request.RequestLine.Method == SipMethods.Bye)
                {
                    //TODO: send OK
                   GoToState(PhoneState.Idle);
                }
            }
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
            var mod100 = responseEvent.Response.StatusLine.StatusCode / 100;
            if (mod100 == 1)
            {
                GoToState(PhoneState.WaitForFinal);
                RefreshDialogForm(_clientDialog);
               
            }
            else if (mod100 == 2)
            {
                GoToState(PhoneState.CallerEstablished);
                RefreshDialogForm(_clientDialog);
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

        private void _btnCalleeSendRinging_Click(object sender, EventArgs e)
        {
            if (_ringingResponse == null)
            {
                _ringingResponse = _pendingRequestEvent.Request.CreateResponse(SipResponseCodes.x180_Ringing);
                _ringingResponse.To.Tag = SipUtil.CreateTag();
                var contactUri = AddressFactory.CreateUri("", MainForm.SipProvider.ListeningPoint.ToString());
                _ringingResponse.Contacts.Add(HeaderFactory.CreateContactHeader(contactUri));

                _inviteTransaction =
                    (SipInviteServerTransaction) SipProvider.CreateServerTransaction(_pendingRequestEvent.Request);
                _serverDialog = SipProvider.CreateServerDialog(_inviteTransaction);
                _inviteTransaction.SendResponse(_ringingResponse);
                RefreshDialogForm(_serverDialog);
                //signal the waiting thread
                _waitHandle.Set();
            }
            else
            {
                _inviteTransaction.SendResponse(_ringingResponse);
            }
        }

        private void _btnCalleeSendBye_Click(object sender, EventArgs e)
        {
            var byeRequest = _serverDialog.CreateRequest(SipMethods.Bye);
            var ctx = SipProvider.CreateClientTransaction(byeRequest);
            _serverDialog.SendRequest(ctx);

            RefreshDialogForm(_serverDialog);
        }

        private void _btnCalleeSendOk_Click(object sender, EventArgs e)
        {
            var okResponse = MessageFactory.CreateResponse(_pendingRequestEvent.Request, SipResponseCodes.x200_Ok);
            //TODO: okResponse.To.Tag = _serverDialog.LocalTag;
            _inviteTransaction.SendResponse(okResponse);
            //TODO: make method public _inviteTransaction.GetDialog();
            RefreshDialogForm(_serverDialog);
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
