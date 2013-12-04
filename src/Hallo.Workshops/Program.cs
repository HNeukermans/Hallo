using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Hallo.Sip;
using Hallo.Sip.Stack;

namespace Hallo.Workshops
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkShop1 ws1 = new WorkShop1();
            ws1.Setup();
            ws1.Start();

            Console.WriteLine("Waiting...");
            Console.ReadLine();

            ws1.Stop();
        }

        
    }
}
