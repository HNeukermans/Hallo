using Hallo.Sip;

namespace Hallo.Workshops
{
    public class WorkShop1 : WorkShopBase
    {
        public override void Start()
        {
            /*create a listener and register it against the receiver provider*/
            var listener = new WorkShop1Listener();
            _receiverProvider.AddSipListener(listener);
            
            /*create message*/
            SipRequest request = CreateRequest(SipMethods.Invite);

            /*send the request*/
            _senderProvider.SendRequest(request);
        }
    }
}