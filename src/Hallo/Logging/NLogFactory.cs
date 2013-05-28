using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace Hannes.Net.Logging
{
    public class NLogFactory : ILogFactory
    {
        public NLogFactory()
        {
        }

        public ILogger CreateLogger(Type type)
        {
            var logger = NLog.LogManager.GetLogger(type.Name);
            return new NLogger(logger);
        }
    }
}
