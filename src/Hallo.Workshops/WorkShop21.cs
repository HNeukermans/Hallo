using Hallo.Sip;

namespace Hallo.Workshops
{
    public class WorkShop21 : WorkShopBase
    {
        public override void Start()
        {
            /*create a listener and register it against both providers*/
            var listener = new WorkShop21Listener();
            _receiverProvider.AddSipListener(listener);
            _senderProvider.AddSipListener(listener);

            /*create message*/
            SipRequest request = CreateRequest(SipMethods.Register);

            /*create the transaction*/
            var clientTransaction = _senderProvider.CreateClientTransaction(request);

            /*send the request*/
            clientTransaction.SendRequest();
        }
    }
}