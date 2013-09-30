using System;
using System.Net;
using System.Text;
using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sdk.SoftPhoneStates;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack;
using Hallo.Sip.Stack.Transactions;
using Hallo.UnitTest.Helpers;
using Hallo.UnitTest.Stubs;
using Moq;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    public class SoftPhoneTest 
    {
        protected SipHeaderFactory _headerFactory;
        protected SipFromHeader _fromHeader;
        protected SipHeaderBase _fromHeaderParsed;
        protected string _fromHeaderBodyString;

        [Test]
        public void Test()
        {
            //send invite


            var network = new FakeNetwork();
            var cs1 = new FakeSipContextSource(TestConstants.IpEndPoint1);
            var cs2 = new FakeSipContextSource(TestConstants.IpEndPoint2);

            //var idleState = new IdleState(null);
            //var sendCommand = (IdleSendCommand) idleState.ProcessRequest();

            //var contextSourceMock = new Mock<ISipContextSource>();
            //contextSourceMock.Setup((cs)=> cs.SendTo(It.IsAny<byte[]>(), It.IsAny<IPEndPoint>())).Callback((byte[] bytes, IPEndPoint endPoint) =>
            //                                                                                                   {
            //                                                                                                       var sipContext = ConvertToSipContext(bytes);
            //                                                                                                       idleState.ProcessRequest(sipContext);
            //                                                                                                       network.SendTo();
            //                                                                                                   })
            //cs1.AddToNetwork(network);
            //cs2.AddToNetwork(network);

            //var sipProvider1 = new SipProvider(new SipStack(), cs1);
            ////var sipProvider2 = new SipProvider(new SipStack(), cs2);
            
            ////sendCommand.Response;
            //var sipProviderMock = new Mock<ISipProvider>();
            //sipProviderMock.Setup((p) => p.ListeningPoint).Returns(new SipListeningPoint(TestConstants.IpEndPoint2));
            ////sipProviderMock.Setup((p) => p.CreateServerDialog(It.IsAny<ISipServerTransaction>())).Returns();
            //var commandFactoryMock = new Mock<ICommandFactory>();
            //bool fired1,fired2,fired3;
            //SoftPhone calleePhone = new SoftPhone(sipProvider1, new SipMessageFactory(), new SipHeaderFactory(), new SipAddressFactory(), new CommandFactory());
            //calleePhone.IncomingCall += CalleePhoneOnIncomingCall;
            //calleePhone.StateChanged += CalleePhoneOnStateChanged;
            //calleePhone.Start();
            //IPhoneLine calleePhoneLine = calleePhone.CreatePhoneLine(new SipAccount());
            //calleePhoneLine.Register();

            //SoftPhone callerPhone = new SoftPhone(sipProvider2, new SipMessageFactory(), new SipHeaderFactory(), new SipAddressFactory(), new CommandFactory());
            //callerPhone.StateChanged += CallerPhoneOnStateChanged;
            //callerPhone.Start();
            //IPhoneCall phoneCall = callerPhone.CreateCall();
            //phoneCall.Invite(TestConstants.IpEndPoint1.ToString());

            //callerPhone.State.Should().BeOfType<RingingState>();
            //calleePhone.State.Should().BeOfType<RingingState>();
            //var softPhone = 

            /*simulate receive invite.*/

            /*verify ringingresponse sent*/
            /*verify both caller + callee to ringingstate*/
        }

        private void CallerPhoneOnStateChanged(object sender, VoipEventArgs<SoftPhoneState> voipEventArgs)
        {
            
        }

        private void CalleePhoneOnStateChanged(object sender, VoipEventArgs<SoftPhoneState> voipEventArgs)
        {
            
        }

        private void CalleePhoneOnIncomingCall(object sender, VoipEventArgs<IPhoneCall> voipEventArgs)
        {
            
        }
    }
}