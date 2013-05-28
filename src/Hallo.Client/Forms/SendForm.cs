using System;
using System.Reactive.Linq;
using System.Windows.Forms;
using Hallo.Component.Logic.Reactive;
using Hallo.Sip;
using Hallo.Sip.Util;

namespace Hallo.Client.Forms
{
    public partial class SendForm : ClientChildForm
    {
        private Timer _timer = new Timer();
        

        public SendForm()
        {
            InitializeComponent();
            
        }

        private IDisposable _subsciption;
        private void _chbSendRegister_CheckedChanged(object sender, EventArgs e)
        {
            _btnRegister.Enabled = !_chbSendRegister.Checked;
            _nudSendRegisterRate.Enabled = !_chbSendRegister.Checked;

            if (_chbSendRegister.Checked && _nudSendRegisterRate.Value > 0)
            {
                int rate = (int)((60 / _nudSendRegisterRate.Value) * 1000);
                _subsciption = Observable.Interval(TimeSpan.FromMilliseconds(rate)).Subscribe((i) => _btnRegister_Click(null, null));
            }
            else
            {
                _subsciption.Dispose();
            }
        }

        private void _btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                var requestUri = AddressFactory.CreateUri(null, "registrar."+ MainForm.Configuration.RegistrarDomain);
                var toAddressUri = AddressFactory.CreateUri("hannes", MainForm.Configuration.RegistrarDomain);
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

                EventAggregator.Instance.Publish(
                    new LogEvent(">>>>" + SipFormatter.FormatMessageEnvelope(request) + Environment.NewLine));
                
                if(_chbSendStateFull.Checked)
                {
                    var transaction =  SipProvider.CreateClientTransaction(request);
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

        private void _nudSendRegisterRate_ValueChanged(object sender, EventArgs e)
        {

        }

        
    }
}
