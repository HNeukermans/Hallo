using System;
using System.Windows.Forms;

namespace Hallo.Component.Logic
{
    public delegate void ExecuteActionDelegate();
    public static class ExecuteActionHelper
    {
        public static void ExecuteAction(ExecuteActionDelegate action)
        {
            try
            {
                if (action == null)
                {
                    return;
                }
                action.Invoke();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}