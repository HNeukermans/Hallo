using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip.Util;

namespace Hallo.Sip
{
    public class SipFormatter
    {
        public static string FormatHeader(SipHeader header)
        {
            var b = new StringBuilder();
            var formatString = b.Append(header.Name).Append(": ").AppendLine(header.FormatBodyToString()).ToString();
            return formatString;
        }

        public static string FormatHeader<T>(IEnumerable<T> header) where T : SipHeader
        {
            var b = new StringBuilder();
            header.ToList().ForEach(i => b.Append(FormatHeader(i)));
            return b.ToString();
        }

        public static string FormatMessageEnvelope(SipMessage message, bool isNewlineTerminated = false)
        {
            var b = new StringBuilder();
            if (message is SipRequest)
            {
                b.AppendLine(((SipRequest) message).RequestLine.FormatToString());
            }
            else
            {
                b.AppendLine(((SipResponse) message).StatusLine.FormatToString());
            }

            FormatHeaders(message, b);

            if(isNewlineTerminated) b.AppendLine(string.Empty);

            return b.ToString();
        }
        
        public static byte[] FormatMessage(SipMessage message)
        {
            var envelope = FormatMessageEnvelope(message, true);
            
            var msg = SipUtil.ToUtf8Bytes(envelope);

            if (message.Body == null) return msg;

            var bytes = new List<byte>(msg);
            bytes.AddRange(message.Body);

            return bytes.ToArray();
        }

        private static void FormatHeaders(SipMessage message, StringBuilder b)
        {
            foreach (var header in message.GetHeaders())
            {
                if(header.IsList)
                {
                    b.Append(FormatHeader((IEnumerable<SipHeader>)header));
                }
                else
                {
                    b.Append(FormatHeader((SipHeader)header));
                }
            }
        }

        public static byte[] FormatToBytes(string value)
        {
            return SipUtil.ToUtf8Bytes(value);
        }
    }
}
