using Hallo.Sdk.Commands;

namespace Hallo.Sdk
{
    internal class PhoneLine : IPhoneLine
    {
        private readonly ICommand<IPhoneLine> _onRegister;
        public SipAccount SipAccount { get; private set; }

        public PhoneLine(SipAccount sipAccount, ICommand<IPhoneLine> onRegister)
        {
            _onRegister = onRegister;
            SipAccount = sipAccount;
        }


        public PhoneLineState State { get; private set; }
        
        public void Register()
        {
            _onRegister.Execute(this);
        }
    }
}