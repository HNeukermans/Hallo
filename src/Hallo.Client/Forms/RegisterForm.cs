using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Forms;
using Hallo.Component.Logic;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack;
using Hallo.Sip.Util;

namespace Hallo.Client.Forms
{
    public partial class RegisterForm : ClientChildForm
    {
        private int commandSequence = 0;

        public RegisterForm()
        {
            InitializeComponent();
            this.Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            _lblDomain.Text = MainForm.Configuration.RegistrarDomain;
            _txtCallId.Text = SipUtil.CreateCallId();
            _txtContactAddress.Text = SipProvider.ListeningPoint.ToString();
            _txtUser.Text = "hannes";
        }

        private SipRequest CreateRegisterRequest(string user)
        {
            FormHelper.ValidateIsNotEmpty(_txtCallId.Text, "device id");
            
            var requestUri = AddressFactory.CreateUri(null, MainForm.Configuration.RegistrarDomain);
            var toAddressUri = AddressFactory.CreateUri(user, MainForm.Configuration.RegistrarDomain);
                var toAddress = AddressFactory.CreateAddress(null, toAddressUri);
                var toHeader = HeaderFactory.CreateToHeader(toAddress);
                var fromHeader = HeaderFactory.CreateFromHeader(toAddress, SipUtil.CreateTag());
                var cseqHeader = HeaderFactory.CreateSCeqHeader(SipMethods.Register, ++commandSequence);
                var callIdheader = HeaderFactory.CreateCallIdHeader(_txtCallId.Text);
                var viaHeader = HeaderFactory.CreateViaHeader(SipProvider.ListeningPoint.Address,
                                                              SipProvider.ListeningPoint.Port, SipConstants.Udp,
                                                              SipUtil.CreateBranch());
                var maxForwardsHeader = HeaderFactory.CreateMaxForwardsHeader();
                var request = MessageFactory.CreateRequest(
                    requestUri,
                    SipMethods.Register,
                    callIdheader,
                    cseqHeader,
                    fromHeader,
                    toHeader,
                    viaHeader,
                    maxForwardsHeader);

                /*add routes and contacts*/
                var proxyServerUri = AddressFactory.CreateUri(null, MainForm.SipStack.OutBoundProxy.ToString());
                var routeHeader = HeaderFactory.CreateRouteHeader(proxyServerUri);
                var localHostUri = AddressFactory.CreateUri(null, _txtContactAddress.Text);
                var contactHeader = HeaderFactory.CreateContactHeader(localHostUri);
                request.Routes.Add(routeHeader);

                return request;
            }
  
        private void _btnRegister_Click(object sender, EventArgs e)
        {
            int expirationTime = 0;
            FormHelper.ValidateIsNotEmpty(_txtUser.Text, "user");
            FormHelper.ValidateCondition(SipUtil.IsIpEndPoint(_txtContactAddress.Text), "host");
            FormHelper.ValidateCondition(int.TryParse(_txtExpirationTime.Text, out expirationTime), "expiration time");
            SendRequest(() =>
                            {
                                var request = CreateRegisterRequest(_txtUser.Text);
                                var localHostUri = AddressFactory.CreateUri(null, _txtContactAddress.Text);
                                var contactHeader = HeaderFactory.CreateContactHeader(localHostUri);
                                contactHeader.Expires = expirationTime;
                                request.Contacts.Add(contactHeader);
                                return request;
                            });
        }

        private void SendRequest(Func<SipRequest> createRequest)
        {
            try
            {
                var request = createRequest();
                
                if (_chbSendTx.Checked)
                {
                    var transaction = SipProvider.CreateClientTransaction(request);
                    transaction.SendRequest();
                }
                else
                {
                    SipProvider.SendRequest(request);
                }

            }
            catch (Exception ex)
            {
                MainForm.HandleException(ex);
            }
        }
        
        private void _btnQueryBindings_Click(object sender, EventArgs e)
        {
            FormHelper.ValidateIsNotEmpty(_txtQueryUser.Text, "user");
            SendRequest(() =>
            {
                var request = CreateRegisterRequest(_txtQueryUser.Text);
                return request;
            });
        }

        private void ChangeEnabledChildControls(GroupBox panel)
        {
            foreach (var c in panel.Controls)
            {
                (c as Control).Enabled = _grpRemoveBindings.Enabled;
            }
        }

        private void _btnRemoveBindings_Click(object sender, EventArgs e)
        {
            FormHelper.ValidateIsNotEmpty(_txtRemoveUser.Text, "user");
            SendRequest(() =>
            {
                var request = CreateRegisterRequest(_txtRemoveUser.Text);
                request.Contacts.Add(SipContactHeader.CreateWildCard());
                request.Expires = HeaderFactory.CreateExpiresHeader(0);
                return request;
            });
        }


        //private void DisableRadioButtonsExcept(RadioButton button)
        //{
        //    foreach (var c in _grpRadioButtons.Controls)
        //    {
        //        if (c == button) continue;
        //        (c as RadioButton).Checked = false;
        //    }
        //}
    }
}
