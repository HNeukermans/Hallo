using System;
using System.Net;
using System.Reactive.Linq;
using Hallo.Sdk.Commands;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.Sip.Util;
using Hallo.Util;

namespace Hallo.Sdk
{

    /// <summary>
    /// serves as a facade.
    /// </summary>
    internal class PhoneCall : IInternalPhoneCall
    {
        public event EventHandler<VoipEventArgs<CallErrorObject>> CallErrorOccured;

        private SipInviteServerTransaction _inviteTransaction;
        private SipResponse _ringingResponse;
        private SipRequest _inviteRequest;
        private SipAddress _from;
        private SipAbstractDialog _dialog;
        private IDisposable _ringingRetransmit;
        private int _counter = 0;
        private bool _isIncoming;
        private readonly ICommand<IInternalPhoneCall> _startCommand;
        private readonly ICommand _stopCommand;
        private readonly ICommand _acceptCommand;
        private SoftPhone _softPhone;
        private SipUri _toUri;
        private ICommand _rejectCommand;
        private CallState _state;

        public PhoneCall(SoftPhone softPhone, bool isIncoming, ICommand<IInternalPhoneCall> startCommand, ICommand stopCommand)
        {
            _softPhone = softPhone;
            _isIncoming = isIncoming;
            _startCommand = startCommand;
            _stopCommand = stopCommand;
        }

        public PhoneCall(SoftPhone softPhone, bool isIncoming, SipUri from, ICommand acceptCommand, ICommand rejectCommand, ICommand stopCommand)
        {
            _softPhone = softPhone;
            _isIncoming = isIncoming;
            _acceptCommand = acceptCommand;
            _rejectCommand = rejectCommand;
            _stopCommand = stopCommand;
        }

        public bool IsIncoming
        {
            get { return _isIncoming; }
        }

        public bool IsOutgoing
        {
            get { return !_isIncoming; }
        }

        public SipAddress From
        {
            get { return _from; }
        }

        public CallState State
        {
            get { return _state; }
        }

        public void Start(string to)
        {
            Check.IsTrue(!_isIncoming, "Failed to start a call. Only outgoing calls can be started.");

            if (!to.StartsWith("sip:")) to = "sip:" + to;

            if (!SipUtil.IsSipUri(to)) throw new ArgumentException("'to' argument is has invalid format.");

            var uri = SipUtil.ParseSipUri(to);

            if (_softPhone.RegisteredPhoneLine == null)
            {
                if (uri.HasNamedHost()) throw new ArgumentException("'to' argument is not valid. Since phone is not registered, 'to' can not be a named host. Only numeric IP hosts are supported.");
                _toUri = uri;
            }
            else
            {
                _toUri = uri;
            }
            
            _startCommand.Execute(this);
        }

        public SipUri GetToUri()
        {
            return _toUri;
        }

        public void Accept()
        {
            Check.IsTrue(_isIncoming, "Failed to accept the call. Only incoming calls can be accepted.");

            _acceptCommand.Execute();
        }

        //internal void InternalSendRinging()
        //{
        //    _ringingRetransmit = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(5)).Subscribe((i) =>
        //    {
        //        if (_ringingResponse == null)
        //        {
        //            _ringingResponse = _inviteRequest.CreateResponse(SipResponseCodes.x180_Ringing);
        //            _ringingResponse.To.Tag = SipUtil.CreateTag();
        //            var contactUri = _addressFactory.CreateUri("", _provider.ListeningPoint.ToString());
        //            _ringingResponse.Contacts.Add(_headerFactory.CreateContactHeader(contactUri));
        //            _inviteTransaction = (SipInviteServerTransaction)_provider.CreateServerTransaction(_inviteRequest);
        //            _dialog = _provider.CreateServerDialog(_inviteTransaction);
        //            _inviteTransaction.SendResponse(_ringingResponse);
        //        }
        //        else
        //        {
        //            _inviteTransaction.SendResponse(_ringingResponse);
        //        }
        //    });
        //}

        public void RaiseCallErrorOccured(CallError error, string message = null)
        {
            CallErrorOccured(this, new VoipEventArgs<CallErrorObject>(new CallErrorObject() {Type = error, Message = message }));
        }

        public void Stop()
        {
            _stopCommand.Execute();
        }

        public void RaiseCallStateChanged(CallState state)
        {
            _state = state;
            Action raiseOnNewThread = () =>  CallStateChanged(this, new VoipEventArgs<CallState>(state));
            raiseOnNewThread.BeginInvoke(null,null);
        }

        public event EventHandler<VoipEventArgs<CallState>> CallStateChanged = delegate { };
        

        public void Reject()
        {
            Check.IsTrue(_isIncoming, "Failed to reject the call. Only incoming calls can be rejected.");

            _rejectCommand.Execute();
        }
    }
}