using System.Windows.Forms;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.Component.Forms
{
    public partial class CoreMainForm : Form
    {
        public SipStack SipStack { get; set; }
        public SipProvider SipProvider { get; set; }
        public SipAddressFactory AddressFactory { get; set; }
        public SipMessageFactory MessageFactory { get; set; }
        public SipHeaderFactory HeaderFactory { get; set; }

        private bool _isStarted = false;

        public CoreMainForm()
        {
            InitializeComponent();
        
        }
    }
}
