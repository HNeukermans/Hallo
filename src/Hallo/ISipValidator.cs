using Hallo.Parsers;
using Hallo.Sip;

namespace Hallo
{
    public interface ISipValidator
    {
        /// <summary>
        /// 8.1.1 Generating the Request
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        ValidateMessageResult ValidateMessage(SipMessage message);
    }
}