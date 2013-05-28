using Hallo.Sip;

namespace Hallo.Parsers
{
    public interface ISipMessageParser
    {
        SipMessage ParseSipMessage(byte[] dataBytes,  string messageContent);
    }
}