using Hallo.Client.MediaPlayer;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Dialogs;
using Hallo.Sip.Stack.Transactions.InviteClient;
using Hallo.Sip.Stack.Transactions.InviteServer;
using Hallo.Sip.Util;
using LumiSoft.Net.Media;

namespace Hallo.Client.Logic
{
    public class DialogListener : ISipListener
    {
        private readonly SipProvider _provider;

        private string _toTag = SipUtil.CreateTag();

        public DialogListener(SipProvider provider)
        {
            _provider = provider;
        }

        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {
            
        }

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            if (requestEvent.Request.RequestLine.Method == SipMethods.Invite)
            {
                var inviteTransaction = (SipInviteServerTransaction)_provider.CreateServerTransaction(requestEvent.Request);
                var dialog = _provider.CreateServerDialog(inviteTransaction);
                /*let the phone of the receiver ring to indicate someone is calling to you+
                 send back a ringing response to inform to callee, you received the invite*/
                WavePlayer player = new WavePlayer(AudioOut.Devices[0]);
                player.Play(ResManager.GetStream("ringing.wav"), 10);
                var response = CreateRingingResponse(requestEvent.Request);
                inviteTransaction.SendResponse(response);

                //start timer that does the above untill the dialog state != Early
                //the listener is in some state. In this state the callee can answer or cancel the call.
                //when it does it goes to another state.
            }
            else if (requestEvent.Request.RequestLine.Method == SipMethods.Ack &&
                requestEvent.Request.CSeq.Command == SipMethods.Invite)
            {
               /*TODO: */
            }
        }

        private SipResponse CreateRingingResponse(SipRequest request)
        {
            var r = request.CreateResponse(SipResponseCodes.x180_Ringing);
            r.To.Tag = _toTag;
            return r;
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
            if (responseEvent.Response.CSeq.Command == SipMethods.Invite)
            {
                if (responseEvent.Response.StatusLine.ResponseCode == SipResponseCodes.x180_Ringing)
                {
                    /*the callee party sends a ring tone. Play ringing tone locally*/
                }
                else if (responseEvent.Response.StatusLine.StatusCode/100 == 2)
                {
                    var inviteTransaction = (SipInviteClientTransaction) responseEvent.ClientTransaction;
                    var dialog = (SipInviteClientDialog)inviteTransaction.GetDialog();
                    var ackRequest = dialog.CreateAck();
                    dialog.SendAck(ackRequest);
                    //TODO: inform user that the remote party has picked up de phone
                }
            }
        }
    }
}