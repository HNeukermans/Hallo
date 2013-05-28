using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip
{
    public class SipMethods
    {
        public const string Ack = "ACK";

        public const string Register = "REGISTER";

        public const string Invite = "INVITE";

        public const string Bye = "BYE";

        public const string Cancel = "Cancel";

        internal static bool IsMethod(string word)
        {
            return word.Equals(Ack) || word.Equals(Register) || word.Equals(Invite) || word.Equals(Bye);
        }
    }
}
