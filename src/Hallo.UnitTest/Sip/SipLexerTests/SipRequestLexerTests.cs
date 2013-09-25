using System;
using FluentAssertions;
using Hallo.Parsers;
using Hallo.Sip;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip.SipParserTests
{
    [TestFixture]
    public class SipRequestLexerTests
    {
        [Test]
        public void Parse_ValidFormats_ExpectNotToFail()
        {
            String[] strings = 
            {
                "REGISTER sip:192.168.0.1 SIP/2.0\r\nName: Value\r\n\r\n", /*default format*/
                "REGISTER sip:192.168.0.1 SIP/2.0\r\nNAME : \r\n" +    /*folded line*/
                " Value\r\n\r\n",
                "REGISTER sip:192.168.0.1 SIP/2.0\r\nNAME: Value, Value\r\n\r\n",  /*comma separated values on same line*/
                "REGISTER sip:192.168.0.1 SIP/2.0\r\nName    :Value\r\n\r\n" /*support white spaces after name*/
                //"REGISTER sip:192.168.0.1 SIP/2.0\r\nNAME: Value\r\n\r\n",
                //"REGISTER sip:192.168.0.1 SIP/2.0\r\nname:  Value\r\n\r\n",
                //"REGISTER sip:192.168.0.1 SIP/2.0\r\n naME :  Value\r\n\r\n", fails because first line is folded line and so does not end with SIP/2.0
            };

            foreach (string s in strings)
            {
                var listenerStub = new SipParserListenerStub();
                var bytes = SipFormatter.FormatToBytes(s);
                new SipParser(listenerStub).Parse(bytes, 0, bytes.Length);

                listenerStub.OnRequestExecuteReceived.Should().NotBeNull();
                listenerStub.OnResponseExecuteReceived.Should().BeNull();
                listenerStub.OnCompleteExecuteReceived.Should().BeTrue();
                listenerStub.OnHeaderExecuteReceived.Should().NotBeEmpty();
                listenerStub.OnHeaderExecuteReceived["Name"].Should().NotBeEmpty();
            }
        }

        [Test]
        public void Parse_InValidFormats_ExpectToThrowExceptions()
        {
            String[] strings = 
            {
                "\r\n\r\n", /*empty message*/
                "  ", /*empty message*/
                "junk\r\n", /*invalid firsline*/
                "\r\n\r\n invalid firsline\r\n", /*invalid firsline*/
                "REGISTER sip:192.168.0.1 SIP/2.0\r\n NAME : \r\n"    /*fails, first line is folded = ends with NAME*/
            };

            int exceptions = 0;
            
            foreach (string s in strings)
            {
                try
                {
                    var listenerStub = new SipParserListenerStub();
                    var bytes = SipFormatter.FormatToBytes(s);
                    new SipParser(listenerStub).Parse(bytes, 0, bytes.Length);
                }
                catch (ParseException e)
                {
                    exceptions++;
                }
            }
            exceptions.Should().Be(strings.Length);
        }
    }
}