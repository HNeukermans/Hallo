using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Util;

namespace Hallo.Client.Forms
{
    public partial class SendStringForm : ClientChildForm
    {
        public SendStringForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(SendStringForm_Load);
        }

        void SendStringForm_Load(object sender, EventArgs e)
        {
            BindHeaders();

            _txtSendTo.Text = SipStack.OutBoundProxy.ToString();
        }

        private void BindHeaders()
        {
            HeaderFactory.GetSupportedHeaders().Keys.ToList().ForEach(hn => _cmbHeaders.Items.Add(hn));
            _cmbHeaders.Items.Insert(0, string.Empty);
        }

        private void _btnSend_Click(object sender, EventArgs e)
        {
            if (_txtMessage.Text == string.Empty) MessageBox.Show("Can not send an empty message");

            UdpClient udpClient = new UdpClient(AddressFamily.InterNetwork);
            var bytes = SipFormatter.FormatToBytes(_txtMessage.Text);
            udpClient.Send(bytes, bytes.Length, SipUtil.ParseIpEndPoint(_txtSendTo.Text));
        }

        private SipRequest CreateRegisterRequest()
        {
            var requestUri = AddressFactory.CreateUri(null, "registrar.ocean.com");
            var toAddressUri = AddressFactory.CreateUri("hannes", "ocean.com");
            var toAddress = AddressFactory.CreateAddress("hannes", toAddressUri);
            var toHeader = HeaderFactory.CreateToHeader(toAddress);
            var fromHeader = HeaderFactory.CreateFromHeader(toAddress, SipUtil.CreateTag());
            var cseqHeader = HeaderFactory.CreateSCeqHeader(SipMethods.Register, 1028);
            var callId = SipUtil.CreateCallId();
            var callIdheader = HeaderFactory.CreateCallIdHeader(callId);
            var viaHeader = HeaderFactory.CreateViaHeader(SipProvider.ListeningPoint.Address, SipProvider.ListeningPoint.Port, SipConstants.Udp,
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

            var proxyServerUri = AddressFactory.CreateUri(null, MainForm.SipStack.OutBoundProxy.ToString());
            var localHostUri = AddressFactory.CreateUri(null, MainForm.SipProvider.ListeningPoint.ToString());
            var routeHeader = HeaderFactory.CreateRouteHeader(proxyServerUri);
            var contactHeader = HeaderFactory.CreateContactHeader(localHostUri);
            request.Routes.Add(routeHeader);
            request.Contacts.Add(contactHeader);
            return request;
        }
        
        private SipRequest ApplyActions(SipRequest sipRequest)
        {
            var selected =_cmbHeaders.SelectedItem != null ? _cmbHeaders.SelectedItem.ToString() : string.Empty;
            if (selected != string.Empty)
                sipRequest.RemoveHeader(selected);

            return sipRequest;
        }

        private void _chbActionExcludeViaHeaders_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void _lnkTemplateRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var request = ApplyActions(CreateRegisterRequest());
            _txtMessage.Text = SipFormatter.FormatMessageEnvelope(request, true);
        }
    }
}
