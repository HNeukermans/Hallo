namespace Hallo.Sip
{
    public abstract class SipHeader : SipHeaderBase , IBodyStringFormatable
    {
        public abstract string FormatBodyToString();

        protected SipHeader():base(false)
        {
        }

        public abstract SipHeader Clone();
    }
}