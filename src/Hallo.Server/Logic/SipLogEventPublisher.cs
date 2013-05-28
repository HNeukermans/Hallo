using Hallo.Component.Logic.Reactive;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.Server.Logic
{
    public class SipLogEventPublisher : ISipListener
    {
        private readonly ISipListener _listener;
      
        public SipLogEventPublisher(ISipListener listener)
        {
            _listener = listener;
        }

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            EventAggregator.Instance.Publish(new LogEvent("<<<< [RECEIVED REQUEST] " + SipFormatter.FormatMessageEnvelope(requestEvent.Request)));
            
            _listener.ProcessRequest(requestEvent);

            //         EventAggregator.Instance.Publish(new LogEvent(">>>> [SEND RESPONSE] " + SipFormatter.FormatMessageEnvelope(response)));

            //MainForm.SendMessage(new LogMessage() { Text = "<<<<" + SipFormatter.FormatMessageEnvelope(requestEvent.Response) });
        }

        public void ProcessResponse(SipResponseEvent responseEvent)
        {
            EventAggregator.Instance.Publish(new LogEvent("<<<< [RECEIVED RESPONSE]" + SipFormatter.FormatMessageEnvelope(responseEvent.Response)));

            _listener.ProcessResponse(responseEvent);
        }

        public void ProcessTimeOut(SipTimeOutEvent timeOutEvent)
        {
            EventAggregator.Instance.Publish(new LogEvent("<<<< [RECEIVED TIMEOUT] " + SipFormatter.FormatMessageEnvelope(timeOutEvent.Request)));

            _listener.ProcessTimeOut(timeOutEvent);
        }
    }
}