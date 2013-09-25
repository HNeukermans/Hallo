using System.Globalization;
using System.Net.Sockets;
using Hallo.Parsers;

namespace Hallo.Sdp.Parsers
{
    [ParserFor("o")]
    public class SdpOriginParser : AbstractParser<SdpOrigin>
    {
        public override SdpOrigin Parse(Util.StringReader r)
        {
            string[] splitted = r.OriginalString.Split(' ');

            if (splitted.Length != 6) throw new ParseException(ExceptionMessage.InvalidSdpOriginLineFormat);

            long id=-1, version=-1;

            SdpOrigin result = new SdpOrigin();
            
            IfNullOrEmptyThrowParseExceptionInvalidFormat(splitted[0]);
            IfFalseThrowParseException(long.TryParse(splitted[1], out id), ExceptionMessage.InvalidFormat);
            IfFalseThrowParseException(long.TryParse(splitted[2], out version), ExceptionMessage.InvalidFormat);
            IfFalseThrowParseException(splitted[3] == "IN", ExceptionMessage.InvalidFormat);
            IfFalseThrowParseException(splitted[4] == "IP4", ExceptionMessage.InvalidFormat); //TODO support IP6
            IfNullOrEmptyThrowParseExceptionInvalidFormat(splitted[5]);
            
            result.UserName = splitted[0];
            result.SessionId = id;
            result.SessionVersion = version;
            result.NetType = splitted[3];
            result.AddressFamily = AddressFamily.InterNetwork;
            result.UnicastAddress = splitted[5];
            return result;
        }
    }
}