using System;
using NLog;

namespace Hannes.Net.Logging
{
    public class NLogger : ILogger
    {
        private readonly Logger _logger;

        public NLogger(Logger logger)
        {
            this._logger = logger;
        }

        public void Debug(string message)
        {
            this._logger.Debug(message);
        }

        public void Debug(string message, Exception exception)
        {
            this._logger.DebugException(message, exception);
        }

        public void Error(string message)
        {
            this._logger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            this._logger.ErrorException(message, exception);
        }

        public void Fatal(string message)
        {
            this._logger.Fatal(message);
        }

        public void Fatal(string message, Exception exception)
        {
            this._logger.FatalException(message, exception);
        }

        public void Info(string message)
        {
            this._logger.Info(message);
        }

        public void Info(string message, Exception exception)
        {
            this._logger.InfoException(message, exception);
        }

        public void Trace(string message)
        {
            this._logger.Trace(message);
        }

        public void Trace(string message, Exception exception)
        {
            this._logger.Trace(message, exception);
        }

        public void Warning(string message)
        {
            this._logger.Warn(message);
        }

        public void Warning(string message, params object[] args)
        {
            this._logger.Warn(message, args);
        }

        public void Warning(string message, Exception exception)
        {
            this._logger.WarnException(message, exception);
        }
    }
}
