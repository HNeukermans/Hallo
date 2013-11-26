using System;
using System.Net;
using Hallo.Component;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.UnitTest.Helpers;
using System.Threading;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal abstract class IntegrationTestBase : Specification
    {
        protected ISoftPhone _phoneAlice;
        protected ISoftPhone _phoneBob;
        protected IPhoneCall _incomingCallAlice;
        protected CallState _callStateAlice;
        protected IPhoneCall _incomingCallBob;
        protected CallState _callStateBob;
        protected CallErrorObject _callErrorAlice;
        protected CallErrorObject _callErrorBob;
        protected IPEndPoint _aliceEndPoint;
        protected IPEndPoint _bobEndPoint;
        protected IPhoneCall _outgoingCallAlice;
        protected IPhoneCall _outgoingCallBob;

        protected override void Given()
        {
            _aliceEndPoint = TestConstants.LocalEndPoint1;
            _bobEndPoint = TestConstants.LocalEndPoint2;

            _phoneAlice = SoftPhoneFactory.CreateSoftPhone(_aliceEndPoint);
            _phoneAlice.IncomingCall += _phoneAlice_IncomingCall;
            _phoneAlice.Start();

            _phoneBob = SoftPhoneFactory.CreateSoftPhone(_bobEndPoint);
            _phoneBob.IncomingCall += _phoneBob_IncomingCall;
            _phoneBob.Start();

            GivenOverride();
        }

        void _callBob_StateChanged(object sender, VoipEventArgs<CallState> e)
        {
            _callStateBob = e.Item;
            OnBobCallStateChanged();
        }

        void _phoneBob_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {
            _incomingCallBob = e.Item;
            WireEventsBob(_incomingCallBob);
            OnBobIncomingCallReceived();
        }

        void _callAlice_StateChanged(object sender, VoipEventArgs<CallState> e)
        {
            _callStateAlice = e.Item;
            OnAliceCallStateChanged();
        }

        void _phoneAlice_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {
            _incomingCallAlice = e.Item;
            WireEventsAlice(_incomingCallAlice);
            OnAliceIncomingCallReceived();
        }

        protected void CallAlice()
        {
            _outgoingCallAlice = _phoneBob.CreateCall();
            WireEventsBob(_outgoingCallAlice);
            _outgoingCallAlice.Start(_aliceEndPoint.ToString());
        }

        protected void CallBob()
        {
            _outgoingCallBob = _phoneAlice.CreateCall();
            WireEventsAlice(_outgoingCallBob);
            _outgoingCallBob.Start(_bobEndPoint.ToString());
        }

        public override void  CleanUp()
        {
 	        _phoneAlice.Stop();
            _phoneBob.Stop();
        }

        protected virtual void OnBobCallStateChanged()
        {
            
        }

        protected virtual void OnAliceCallStateChanged()
        {

        }

        protected virtual void AliceIncomingCallOverride()
        {

        }

        

        protected virtual void OnBobIncomingCallReceived()
        {
            
        }
        protected virtual void OnAliceIncomingCallReceived()
        {

        }

        

        protected virtual void GivenOverride()
        {

        }

        private void WireEventsAlice(IPhoneCall call)
        {
            call.CallErrorOccured += callAlice_ErrorOccured;
            call.CallStateChanged += _callAlice_StateChanged;
        }

        private void WireEventsBob(IPhoneCall call)
        {
            call.CallErrorOccured += callBob_ErrorOccured;
            call.CallStateChanged += _callBob_StateChanged;
        }

        private void UnWireEventsBob(IPhoneCall call)
        {
            call.CallErrorOccured -= callBob_ErrorOccured;
            call.CallStateChanged -= _callBob_StateChanged;
        }

        private void UnWireEventsAlice(IPhoneCall call)
        {
            call.CallErrorOccured -= callAlice_ErrorOccured;
            call.CallStateChanged -= _callAlice_StateChanged;
        }

        private void callBob_ErrorOccured(object sender, VoipEventArgs<CallErrorObject> e)
        {
            _callErrorBob = e.Item;
            OnBobErrorOccured();
        }

        void callAlice_ErrorOccured(object sender, VoipEventArgs<CallErrorObject> e)
        {
            _callErrorAlice = e.Item;
            OnAliceErrorOccured();
        }

        protected virtual void OnAliceErrorOccured()
        {
            
        }

        protected virtual void OnBobErrorOccured()
        {

        }

        protected void SleepSeconds(int seconds)
        {
            Thread.Sleep(1000*seconds);
        }

        protected void SleepMilliSeconds(int value)
        {
            Thread.Sleep(value);
        }
    }
}