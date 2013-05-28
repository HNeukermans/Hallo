using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Hallo.Component.Logic.Reactive;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.WinForms;

namespace Hallo.Client.Logic
{
    /// <summary>
    /// Pipeline listener. this is the default siplisterer for the cient sipstack.
    /// Allows other listeners to register and thus receive sip messages from the stack.
    ///  </summary>
    public class SipPipeLineListener : ISipListener
    {
        public ClientMainForm MainForm { get; set; }

        private ISipListener _siplistener; 

        public void RegisterListener(ISipListener listener)
        {
            _siplistener = listener;
        }

        public void UnRegisterListener()
        {
            _siplistener = null;
        }


        public SipPipeLineListener(ClientMainForm mainForm)
        {
            MainForm = mainForm;
        }

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            EventAggregator.Instance.Publish(new LogEvent("<<<< [RECEIVED REQUEST] " + SipFormatter.FormatMessageEnvelope(requestEvent.Request)));
            
            if(_siplistener != null) _siplistener.ProcessRequest(requestEvent);

            //MainForm.SendMessage(new LogMessage() { Text = "<<<<" + SipFormatter.FormatMessageEnvelope(requestEvent.Response) });
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
            EventAggregator.Instance.Publish(new LogEvent("<<<< [RECEIVED RESPONSE]" + SipFormatter.FormatMessageEnvelope(responseEvent.Response)));

            if (_siplistener != null) _siplistener.ProcessResponse(responseEvent);
        }


        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {
            EventAggregator.Instance.Publish(new LogEvent("<<<< [RECEIVED TimeOutEvent]" + timeOutEvent.Request.RequestLine.FormatToString() + ":" + timeOutEvent.Request.Vias.GetTopMost().Branch));

        }
    }
}
