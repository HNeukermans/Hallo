using System;
using Hallo.Parsers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    public class SipAllowHeaderTests
    {
        [Test]
        public void Parse_ValidFormats_ExpectNotToFail()
        {
            String[] strings = 
                {
                    " INVITE,ACK,CANCEL,BYE,NOTIFY,REFER,OPTIONS,INFO,SUBSCRIBE,UPDATE,PRACK",
                };


            foreach (string s in strings)
            {
                var h = new SipAllowHeaderParser().Parse(s);
            }
        }

    }
}