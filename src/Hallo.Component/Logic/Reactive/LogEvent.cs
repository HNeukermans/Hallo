namespace Hallo.Component.Logic.Reactive
{
    public class LogEvent
    {
        public string Message { get; set; }

        public LogEvent(string message)
        {
            Message = message;
        }
    }
}