using System.Linq;
using System.Net;
using System.Net.Sockets;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack;
using Hallo.Sip.Util;

namespace Hallo.Workshops
{
    public class WorkShop1
    {
        private IPAddress _ipAddress;
        private ISipProvider _receiverProvider;
        private ISipProvider _senderProvider;
        private SipStack _stack;

        public void Setup()
        {
            _ipAddress =
                Dns.GetHostAddresses(string.Empty).Where(a => a.AddressFamily == AddressFamily.InterNetwork).First();

            /*create stack*/
            _stack = new SipStack();

            /*create sipproviders*/
            _senderProvider = _stack.CreateSipProvider(_stack.CreateUdpListeningPoint(_ipAddress, 12345));
            _receiverProvider = _stack.CreateSipProvider(_stack.CreateUdpListeningPoint(_ipAddress, 23456));

            /*create a listener and register it against the receiver provider*/
            var listener = new SipListener();
            _receiverProvider.AddSipListener(listener);
        }

        public void Start()
        {
            /*start both providers*/
            _senderProvider.Start();
            _receiverProvider.Start();

            SipRequest message = CreateInvite();

            _senderProvider.SendRequest(message);
        }

        private SipRequest CreateInvite()
        {
            /*create the 'INVITE' message*/
            SipAddressFactory addressFactory = _stack.CreateAddressFactory();
            SipHeaderFactory headerFactory = _stack.CreateHeaderFactory();
            SipMessageFactory messageFactory = _stack.CreateMessageFactory();

            SipUri senderSipUri = addressFactory.CreateUri(string.Empty, new IPEndPoint(_ipAddress, 12345).ToString());
            SipAddress senderSipAddress = addressFactory.CreateAddress(string.Empty, senderSipUri);

            SipUri receiverSipUri = addressFactory.CreateUri(string.Empty, new IPEndPoint(_ipAddress, 23456).ToString());
            SipAddress receiverSipAddress = addressFactory.CreateAddress(string.Empty, senderSipUri);

            SipToHeader toHeader = headerFactory.CreateToHeader(receiverSipAddress);
            SipFromHeader fromHeader = headerFactory.CreateFromHeader(senderSipAddress, SipUtil.CreateTag());
            SipCSeqHeader cseqHeader = headerFactory.CreateSCeqHeader(SipMethods.Invite, 1);
            SipCallIdHeader callIdheader = headerFactory.CreateCallIdHeader(SipUtil.CreateCallId());
            SipViaHeader viaHeader = headerFactory.CreateViaHeader(_ipAddress, 12345, SipConstants.Udp,
                                                                   SipUtil.CreateBranch());
            SipMaxForwardsHeader maxForwardsHeader = headerFactory.CreateMaxForwardsHeader();
            SipRequest request = messageFactory.CreateRequest(
                receiverSipUri,
                SipMethods.Invite,
                callIdheader,
                cseqHeader,
                fromHeader,
                toHeader,
                viaHeader,
                maxForwardsHeader);

            SipContactHeader contactHeader = headerFactory.CreateContactHeader(senderSipUri);
            request.Contacts.Add(contactHeader);

            return request;
        }

        public void Stop()
        {
            _senderProvider.Stop();
            _receiverProvider.Stop();
        }
    }
}