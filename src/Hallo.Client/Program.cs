using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Hallo.Client;

namespace Hallo.WinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            Application.Run(new ClientMainForm());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Error", MessageBoxButtons.OK);
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = e.ExceptionObject as Exception;
                MessageBox.Show(string.Format("CurrentDomainUnhandledException caught: {0}", e.ExceptionObject.ToString()), "Error", MessageBoxButtons.OK);
            }
            finally
            {
                Application.Exit();
            }
        }
  
    }
}
