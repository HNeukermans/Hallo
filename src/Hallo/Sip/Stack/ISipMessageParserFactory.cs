using Hannes.Net.Parsers;

namespace Hannes.Net.Sip
{
    public interface ISipMessageParserFactory
    {
        SipMessageParser CreateMessageParser();
    }
}