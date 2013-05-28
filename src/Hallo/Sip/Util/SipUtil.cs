using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Hallo.Parsers;
using Hallo.Util;
using Hallo.Util;

namespace Hallo.Sip.Util
{
    public class SipUtil
    {
        public static string CreateCallId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string CreateTag()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(8);
        }


        public static string CreateBranch()
        {
            return SipConstants.BranchPrefix + Guid.NewGuid().ToString().Replace("-", "");
        }

        public static byte[] ToUtf8Bytes(string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }

        public static bool IsIPAddress(string address)
        {
            return ParseUtil.IsIPAddress(address);
        }

        internal static IPEndPoint ConvertToIpEndPoint(SipUri sipUri)
        {
            if(!IsIPAddress(sipUri.Host)) return null;
            return new IPEndPoint(IPAddress.Parse(sipUri.Host), sipUri.Port);
        }

        public static IPEndPoint ParseIpEndPoint(string text)
        {
            return ParseUtil.ParseIpEndPoint(text);
        }

        public static bool IsIpEndPoint(string text)
        {
            return ParseUtil.IsIpEndPoint(text);
        }

        public static IPAddress ParseIpAddress(string text)
        {
            return ParseUtil.ParseIPAddress(text);
        }

        public static void DisposeTimer(Timer timer)
        {
            Check.Require(timer, "timer");
            var waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            timer.Dispose(waitHandle);
            waitHandle.WaitOne(1000);
        }

        public static bool IsSipUri(string text)
        {
            try
            {
                new SipUriParser().Parse(text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static SipUri ParseSipUri(string text)
        {
            return new SipUriParser().Parse(text);
        }
    }
}
