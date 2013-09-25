using System;

namespace Hallo.Sdp.Parsers
{
    /// <summary>
    /// Used to define which line a parser is for.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ParserForAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParserForAttribute"/> class.
        /// </summary>
        /// <param name="line">Name of the line.</param>
        public ParserForAttribute(string line)
        {
            Line = line;
        }

        /// <summary>
        /// Gets name of line
        /// </summary>
        public string Line { get; private set; }
    }
}