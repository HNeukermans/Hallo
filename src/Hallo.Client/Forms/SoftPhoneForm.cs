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
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.Sip.Util;
using Hallo.Util;

namespace Hallo.Client.Forms
{
    public partial class SoftPhoneForm : ClientChildForm
    {
        private ISoftPhone _softPhone;
        private IPhoneCall _incomingCall;
        private CallState? _callState;
        private IPhoneCall _outgoingCall;

        public SoftPhoneForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form_Load);
            this.Closing += new CancelEventHandler(Form_Closing);
        }

        void Form_Closing(object sender, CancelEventArgs e)
        {
            _softPhone.Stop();
        }

        void Form_Load(object sender, EventArgs e)
        {
            _txtFromAlias.Text = "alice";
            _txtToAlias.Text = "bob";
            _chkPeerToPeer.Checked = false;
            _chkPeerToPeer_CheckedChanged(null,null);
            
            _softPhone = SoftPhoneFactory.CreateSoftPhone(SipUtil.ParseIpEndPoint(this.MainForm.Configuration.BindIpEndPoint));
            _softPhone.IncomingCall += _softPhone_IncomingCall;
            _softPhone.Start();
        }

        void _softPhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {
            _incomingCall = e.Item;
            WireEvents(_incomingCall);
            Log("Received incoming call");
        }
        
        private void _chkPeerToPeer_CheckedChanged(object sender, EventArgs e)
        {
            var bindEndPoint = SipUtil.ParseIpEndPoint(MainForm.Configuration.BindIpEndPoint);
            _txtFromUri.Text = _chkPeerToPeer.Checked ? string.Format("sip:alice@{0}", bindEndPoint) : "sip:alice@ocean.com";
            _txtToUri.Text = _chkPeerToPeer.Checked ? string.Format("sip:bob@{0}",
                new IPEndPoint(bindEndPoint.Address, bindEndPoint.Port + 1)) : "sip:bob@ocean.com";
        }
        
        private void Log(string message, params object[] args)
        {
            this.OnUIThread(()=> _txtLog.Text += string.Format(message, args) + "\r\n");
        }

        private void _btnPhone_Click(object sender, EventArgs e)
        {
            if (!_callState.HasValue || _callState.Value == CallState.Completed)
            {
                FormHelper.ValidateCondition(SipUtil.IsSipUri(_txtToUri.Text), "To-uri");

                _outgoingCall = _softPhone.CreateCall();
                WireEvents(_outgoingCall);
                _outgoingCall.Start(_txtToUri.Text);
                Log("Call started");
            }
            else if (_callState.Value == CallState.Ringing)
            {
                _incomingCall.Accept();
            }
            else if (_callState.Value == CallState.Ringback || _callState.Value == CallState.InCall)
            {
                _outgoingCall.Stop();
            }
        }

        private void WireEvents(IPhoneCall call)
        {
            call.CallErrorOccured += call_CallErrorOccured;
            call.CallStateChanged += call_CallStateChanged;
        }

        void call_CallStateChanged(object sender, VoipEventArgs<CallState> e)
        {
            Log("Call state changed to: {0}", e.Item);

            _callState = e.Item;

            var text = "Call";

            switch (_callState)
            {
                case CallState.InCall:
                case CallState.Ringback: text = "Hang up";
                    break;
                case CallState.Ringing: text = "Accept";
                    break;
                case CallState.Completed: text = "Call";
                    break;
            }
            
            this.OnUIThread(() => _btnPhone.Text = text);
        }

        void call_CallErrorOccured(object sender, VoipEventArgs<CallErrorObject> e)
        {
            Log("Call error occured: {0}", e.Item);
        }
    }
  

}
