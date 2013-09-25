using System;

namespace Hallo.Component.Logic
{
    public class ExceptionRowInfo
    {
        public string Message { get; set; }
        public string DateTime { get; set; }
        public string StackTrace { get; set; }
        public Exception Item { get; set; }
        
        public ExceptionRowInfo(string message, string stackTrace, Exception exception)
        {
            Message = message;
            DateTime = System.DateTime.Now.ToString("hh:mm:ss");
            StackTrace = stackTrace;
            Item = exception;
        }
    }
}