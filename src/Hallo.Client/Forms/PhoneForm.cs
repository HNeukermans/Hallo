using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hallo.Client.Forms
{
    public partial class PhoneForm : ClientChildForm
    {
        private bool _isStarted = false;

        public PhoneForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form_Load);
            this.Closing += new CancelEventHandler(Form_Closing);
       
        }

        private void Form_Closing(object sender, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _btnStart_Click(object sender, EventArgs e)
        {
            _isStarted = !_isStarted;
            
            if(_isStarted)
            {
                _btnStartStop.Text = "Stop";
            }
            else
            {
                _btnStartStop.Text = "Start";
            }
            
        }
    }
}
