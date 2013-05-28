namespace Hallo.Server
{
    public class SipAddressBindingDiagnosticsInfo
    {
        public AddressBindingServiceOperation Operation { get; set; }
        public SipAddressBinding AddressBinding { get; set; }
    }

    public enum AddressBindingServiceOperation
    {
        Add,
        Update,
        CleanUp,
        Remove
    }

}