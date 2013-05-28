using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip.Stack;
using Moq;

namespace Hallo.UnitTest.Builders
{
    public class TimerFactoryMockBuilder
    {
        public TimerFactoryMockBuilder()
        {
            _nonInviteCtxLeaveCompleted = CreateDoNothingTimer();
            _nonInviteCtxRetransmit = CreateDoNothingTimer();
            _nonInviteCtxTimeout = CreateDoNothingTimer();

            _inviteCtxLeaveCompleted = CreateDoNothingTimer();
            _inviteCtxRetransmit = CreateDoNothingTimer();
            _inviteCtxTimeout = CreateDoNothingTimer();
        }
        
        TxTimer _nonInviteCtxLeaveCompleted;

        #region NonInviteClient
        
        public TimerFactoryMockBuilder WithNonInviteCtxLeaveCompleted(TxTimer value)
        {
            _nonInviteCtxLeaveCompleted = value;
            return this;
        }

        TxTimer _nonInviteCtxRetransmit;

        public TimerFactoryMockBuilder WithNonInviteCtxRetransmit(TxTimer value)
        {
            _nonInviteCtxRetransmit = value;
            return this;
        }

        TxTimer _nonInviteCtxTimeout;

        public TimerFactoryMockBuilder WithNonInviteCtxTimeout(TxTimer value)
        {
            _nonInviteCtxTimeout = value;
            return this;
        }

        #endregion

        #region InviteClient
        
        TxTimer _inviteCtxLeaveCompleted;
        
        public TimerFactoryMockBuilder WithInviteCtxLeaveCompleted(TxTimer value)
        {
            _inviteCtxLeaveCompleted = value;
            return this;
        }

        TxTimer _inviteCtxRetransmit;

        public TimerFactoryMockBuilder WithInviteCtxRetransmit(TxTimer value)
        {
            _inviteCtxRetransmit = value;
            return this;
        }

        TxTimer _inviteCtxTimeout;
        
        public TimerFactoryMockBuilder WithInviteCtxTimeout(TxTimer value)
        {
            _inviteCtxTimeout = value;
            return this;
        }

        #endregion
        
        public ITimerFactory Build()
        {
            Mock<ITimerFactory>  mock = new Mock<ITimerFactory>();
            mock.Setup(f => f.CreateNonInviteCtxEndCompletedTimer(It.IsAny<Action>())).Returns(_nonInviteCtxLeaveCompleted);
            mock.Setup(f => f.CreateNonInviteCtxRetransmitTimer(It.IsAny<Action>())).Returns(_nonInviteCtxRetransmit);
            mock.Setup(f => f.CreateNonInviteCtxTimeOutTimer(It.IsAny<Action>())).Returns(_nonInviteCtxTimeout);

            mock.Setup(f => f.CreateInviteCtxEndCompletedTimer(It.IsAny<Action>())).Returns(_inviteCtxLeaveCompleted);
            mock.Setup(f => f.CreateInviteCtxRetransmitTimer(It.IsAny<Action>())).Returns(_inviteCtxRetransmit);
            mock.Setup(f => f.CreateInviteCtxTimeOutTimer(It.IsAny<Action>())).Returns(_inviteCtxTimeout);

            return mock.Object;
        }

        private TxTimer CreateDoNothingTimer()
        {
            return new TxTimer(()=> { }, int.MaxValue, false);
        }
    }
}
