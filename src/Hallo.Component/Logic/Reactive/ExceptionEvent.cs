using System;

namespace Hallo.Component.Logic.Reactive
{
    public class ExceptionEvent
    {
        public Exception Exception { get; set; }

        public ExceptionEvent(Exception exception)
        {
            Exception = exception;
        }
    }
}