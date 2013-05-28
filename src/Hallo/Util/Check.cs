using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Util
{
    /// <summary>
    /// Small design by contract implementation.
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Check whether a parameter is empty.
        /// </summary>
        /// <param name="value">Parameter value</param>
        /// <param name="parameterOrErrorMessage">Parameter name, or error description.</param>
        /// <exception cref="ArgumentException">value is empty.</exception>
        public static void NotEmpty(string value, string parameterOrErrorMessage)
        {
            if (!string.IsNullOrEmpty(value))
                return;

            if (parameterOrErrorMessage.IndexOf(' ') == -1)
                throw new ArgumentException("'" + parameterOrErrorMessage + "' cannot be empty.", parameterOrErrorMessage);

            throw new ArgumentException(parameterOrErrorMessage);
        }

        /// <summary>
        /// Checks whether a parameter is null.
        /// </summary>
        /// <param name="value">Parameter value</param>
        /// <param name="parameterOrErrorMessage">Parameter name, or error description.</param>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        public static void Require(object value, string parameterOrErrorMessage)
        {
            if (value != null)
                return;

            if (parameterOrErrorMessage.IndexOf(' ') == -1)
                throw new ArgumentNullException("'" + parameterOrErrorMessage + "' cannot be null.", parameterOrErrorMessage);

            throw new ArgumentNullException(parameterOrErrorMessage);

        }

        /// <summary>
        /// Checks whether a parameter is null.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="value">Parameter value</param>
        /// <param name="parameterOrErrorMessage">Parameter name, or error description.</param>
        /// <exception cref="ArgumentException">value is null.</exception>
        public static void Min(int minValue, object value, string parameterOrErrorMessage)
        {
            if (value != null)
                return;

            if (parameterOrErrorMessage.IndexOf(' ') == -1)
                throw new ArgumentException("'" + parameterOrErrorMessage + "' must be at least " + minValue + ".", parameterOrErrorMessage);

            throw new ArgumentException(parameterOrErrorMessage);

        }

        internal static void NotNullOrEmpty(string text, string argument)
        {
           if(string.IsNullOrEmpty(text)) throw new ArgumentException(argument, argument + " can not be null or empty.");
        }

        public static void IsTrue<T>(T value, Func<T, bool> condition, string parameterOrErrorMessage)
        {
            if(!condition(value))
            {
                if (parameterOrErrorMessage.IndexOf(' ') == -1)
                    throw new ArgumentException(parameterOrErrorMessage, parameterOrErrorMessage + " is not a valid argument");

                throw new ArgumentException(parameterOrErrorMessage);
            }
        }

        public static void IsTrue(bool condition, string parameterOrErrorMessage)
        {
            if (!condition)
            {
                if (parameterOrErrorMessage.IndexOf(' ') == -1)
                    throw new ArgumentException(parameterOrErrorMessage, parameterOrErrorMessage + " is not a valid argument");

                throw new ArgumentException(parameterOrErrorMessage);
            }
        }
    }
}
