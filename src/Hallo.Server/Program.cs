using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Hallo.Server
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.ThreadException += UIThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServerMainForm());
        }

        private static void UIThreadException(object sender, ThreadExceptionEventArgs t)
        {
            DialogResult result = DialogResult.Cancel;

            try
            {
                result = MessageBox.Show(string.Format("UIThreadException caught: {0}", t.Exception.ToString()), "Error", MessageBoxButtons.OK);
            }
            catch
            {
                Application.Exit();
            }

            // Exits the program when the user clicks Abort.
            if (result == DialogResult.Abort)
            {
                Application.Exit();
            }
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
