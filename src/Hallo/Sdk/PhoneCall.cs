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
    internal class PhoneCall : IPhoneCall
    {
        public event EventHandler<VoipEventArgs<CallError>> CallErrorOccured;

        private SipInviteServerTransaction _inviteTransaction;
        private SipResponse _ringingResponse;
        private SipRequest _inviteRequest;
        private SipAddress _from;
        private SipAbstractDialog _dialog;
        private IDisposable _ringingRetransmit;
        private int _counter = 0;
        private bool _isIncoming;
        private readonly ICommand<IPhoneCall> _startCommand;
        private readonly ICommand _answerCommand;
        private SoftPhone _softPhone;
        private SipUri _toUri;
        private ICommand _rejectCommand;

        public PhoneCall(SoftPhone softPhone, bool isIncoming, ICommand<IPhoneCall> startCommand)
        {
            _softPhone = softPhone;
            _isIncoming = isIncoming;
            _startCommand = startCommand;
        }

        public PhoneCall(SoftPhone softPhone, bool isIncoming, SipUri from, ICommand answerCommand, ICommand rejectCommand)
        {
            _softPhone = softPhone;
            _isIncoming = isIncoming;
            _answerCommand = answerCommand;
            _rejectCommand = rejectCommand;
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
        
        public CallState State { get; set; }

        public void Start(string to)
        {
            Check.IsTrue(!_isIncoming, "Failed to start a call. Only outgoing calls can be started.");

            if (_softPhone.RegisteredPhoneLine == null)
            {
                /*to has to be ipendpoint*/
                if (SipUtil.IsIpEndPoint(to)) _toUri = _softPhone.AddressFactory.CreateUri(string.Empty, to);
                else throw new ArgumentException("'to' argument is not valid. 'To' argument must have ipendpoint-format");
            }
            else
            {
                if (!to.StartsWith("sip:")) to = "sip:" + to;
                if (SipUtil.IsSipUri(to)) _toUri = SipUtil.ParseSipUri(to);
                else throw new ArgumentException("'to' argument is not valid. 'To' argument must have ipendpoint- or sipuri-format");
            }

            _startCommand.Execute(this);
        }

        public SipUri GetToUri()
        {
            return _toUri;
        }

        public void Answer()
        {
            Check.IsTrue(_isIncoming, "Failed to Answer the call. Only incoming calls can be answered.");
            _answerCommand.Execute();
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

       
    }
}