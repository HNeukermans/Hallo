using System;
using Hallo.Parsers;

namespace Hallo.Sip.Headers
{
    /// <summary>
    /// Used to associate the for which header name the type is for.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HeaderForAttribute : Attribute
    {
        public HeaderForAttribute(string headerName)
        {
            Name = headerName;
        }

        public string Name { get; private set; }
    }
}