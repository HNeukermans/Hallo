namespace Hallo.Sip
{
    public interface ISipMessageSender
    {
        /// <summary>
        /// sends a response to the underlying transport
        /// </summary>
        new void SendResponse(SipResponse response);

        /// <summary>
        /// sends a request to the underlying transport
        /// </summary>
        new void SendRequest(SipRequest request);
    }
}