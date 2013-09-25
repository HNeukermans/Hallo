namespace Hallo.Sdk
{
    public interface IPhoneLine
    {
        SipAccount SipAccount { get;  }
        PhoneLineState State { get;  }
        void Register();
    }
}