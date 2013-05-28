using System;
using Hallo.Parsers;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    public class SipParameterCollectionTests
    {
        [Test]
        public void Parse_ValidFormats_ExpectNotToFail()
        {
            String[] strings = 
            {
                //";lr;expires=3600",
               // "; lr"
               ";lr"
            };


            foreach (string s in strings)
            {
                var h = new SipParameterCollectionParser().Parse(s);
            }
        }
    }
}