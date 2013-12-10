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
            int input = 1;
            while(input > 0)
            {
                Console.WriteLine("Which workshop would you like to start?");
                Console.WriteLine("Press a number, Q to quit <ENTER>");
                var line = Console.ReadLine();
           
                if(!int.TryParse(line, out input)) break;

                string workShopName = string.Format("Hallo.Workshops.WorkShop{0}", input);
                var workShopType = Type.GetType(workShopName);
                var workShop = (WorkShopBase) Activator.CreateInstance(workShopType);

                workShop.Setup();
                workShop.Start();
                Console.ReadLine();
                workShop.Stop();
                Console.WriteLine("Press <ENTER>");
            }
        }

        
    }
}
