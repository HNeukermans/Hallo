using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.Component.Forms
{
    /// <summary>
    /// Mdi form that is the child of CoreMainForm 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChildForm<T> : FormWithKey where T : CoreMainForm
    {
        public T MainForm
        {
            get { return MdiParent as T; }
        }

        public SipStack SipStack
        {
            get { return MainForm.SipStack; }
        }

        public SipProvider SipProvider
        {
            get { return MainForm.SipProvider; }
        }


        public SipAddressFactory AddressFactory
        {
            get { return MainForm.AddressFactory; }
        }

        public SipMessageFactory MessageFactory
        {
            get { return MainForm.MessageFactory; }
        }

        public SipHeaderFactory HeaderFactory
        {
            get { return MainForm.HeaderFactory; }
        }
    }
}