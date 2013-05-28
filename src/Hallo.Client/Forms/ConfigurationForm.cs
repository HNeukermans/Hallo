using System;
using Hallo.Sip;

namespace Hallo.Client.Forms
{
    public partial class ConfigurationForm : ClientChildForm
    {
        public SipStack SipStack { get; set; }

        public ConfigurationForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(ConfigurationForm_Load);
        }

        void ConfigurationForm_Load(object sender, EventArgs e)
        {
            _propGridConfiguration.SelectedObject = this.MainForm.Configuration;
        }

        private void _propGridConfiguration_Click(object sender, EventArgs e)
        {

        }

        private void _btnSave_Click(object sender, EventArgs e)
        {
            
        }
    }
}
