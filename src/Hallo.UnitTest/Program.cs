using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Ninject;

namespace Hallo.UnitTest
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            bool useMeleeWeapons = false;
            
            string[] my_args = { Assembly.GetExecutingAssembly().Location };

            int returnCode = NUnit.ConsoleRunner.Runner.Main(my_args);



            if (returnCode != 0)
                Console.Beep();
        }
    }
}
