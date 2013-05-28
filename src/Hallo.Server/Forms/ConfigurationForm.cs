using System;

namespace Hallo.Server.Forms
{
    public partial class ConfigurationForm : ServerChildForm
    {
        public ConfigurationForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(ConfigurationForm_Load);
        }

        void ConfigurationForm_Load(object sender, EventArgs e)
        {
            _propGridConfiguration.SelectedObject = this.MainForm.Configuration;
        }
    } 
}
