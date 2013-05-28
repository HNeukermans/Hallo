using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Component.Forms;
using Hallo.Sip;
using Hallo.Sip.Stack;
using Hallo.WinForms;

namespace Hallo.Client.Forms
{
    /// <summary>
    /// Serves as the childform that can be extended within this project. 
    /// </summary>
    /// <remarks>
    /// In windows forms a form can only extend a non generic typed form. 
    /// This form is designer compatible bc it is not generic.
    /// </remarks>
    public class ClientChildForm : ChildForm<ClientMainForm>
    {
    }
}
