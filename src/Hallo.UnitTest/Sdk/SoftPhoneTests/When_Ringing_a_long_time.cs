using FluentAssertions;
using Hallo.Sdk;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.Sip.Util;
using Hallo.UnitTest.Builders;
using Hallo.UnitTest.Helpers;
using Hallo.UnitTest.Stubs;
using NUnit.Framework;
using Hallo.Component;
using System.Linq;
using System;
using System.Threading;

namespace Hallo.UnitTest.Sdk.SoftPhoneTests
{
    internal class When_Ringing_a_long_time : SoftPhoneSpecificationBase
    {
        int _ringingSendCounter = 0;
        int _ringingReceivedCounter = 0;
        int _periodicity = 1000;
        int _longtimeSpan = 3000;
        private TxTimerStub _ringingTimer;
        private object _lock;
        
        public When_Ringing_a_long_time()
        {
            _lock = new object();
            _wait = new ManualResetEvent(false);
            _timerFactory = new TimerFactoryStubBuilder().WithRingingTimerInterceptor((a) => OnCreateRingingTimer(a, _periodicity)).Build();
        }


        protected override void _calleePhone_IncomingCall(object sender, VoipEventArgs<IPhoneCall> e)
        {

        }

        protected override void _calleePhone_StateChanged(object sender, VoipEventArgs<SoftPhoneState> e)
        {
            
        }

        protected override void GivenOverride()
        {
            _network.SendTo(SipFormatter.FormatMessage(_invite), TestConstants.IpEndPoint1, TestConstants.IpEndPoint2);
            _wait.WaitOne(TimeSpan.FromMilliseconds(_longtimeSpan + 500));
            //_wait.WaitOne();

            _calleePhone.InternalState.Should().Be(_stateProvider.GetRinging()); /*required assertion*/
        }

        protected virtual ITimer OnCreateRingingTimer(Action action, int periodicity)
        {
            _ringingTimer = new TxTimerStub(action, periodicity, true, () => _ringingSendCounter++);
            return _ringingTimer;
        }

        protected override void OnReceive(SipContext sipContext)
        {
            if (sipContext.Response.StatusLine.ResponseCode == SipResponseCodes.x180_Ringing)
            {
                lock (_lock)
                {
                    _ringingReceivedCounter++;
                    if (_ringingReceivedCounter > (_longtimeSpan / _periodicity))
                    {
                        _wait.Set();
                    }
                }                
            }
        }

        protected override void When()
        {

        }

        [Test]
        public void Expect_ringing_responses_to_be_sent_periodicly()
        {
            _ringingReceivedCounter.Should().BeGreaterThan((_longtimeSpan / _periodicity));
        }


        protected override void _calleePhone_InternalStateChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
