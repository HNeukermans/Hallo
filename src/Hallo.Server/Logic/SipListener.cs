using System.Text;
using System.Threading;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.Server.Logic
{
    public class OkServer : ISipListener
    {
        public ServerMainForm MainForm { get; set; }

        public OkServer(ServerMainForm mainForm)
        {
            MainForm = mainForm;
        }

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            var response = requestEvent.Request.CreateResponse(SipResponseCodes.x200_Ok);

            if(requestEvent.ServerTransaction != null)
            {
                requestEvent.ServerTransaction.SendResponse(response);
            }
            else
            {
                requestEvent.Response = response;
                requestEvent.IsHandled = true;
                MainForm.SipProvider.SendResponse(requestEvent.Response);
            }
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
            
        }


        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}
