using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Hallo.Component.Logic
{
    public class ProcessLogic
    {
        public static int CountProcessesByName(string name)
        {
            return Process.GetProcesses().Count(p => p.ProcessName.Contains(name));
        }
    }
}
