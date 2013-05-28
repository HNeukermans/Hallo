using System;
using NLog;

namespace Hallo.Util
{
    public class LogUtil
    {
         private static Logger _logger;
         private static string _loggerName = "NLogLogger";
         static LogUtil()
         {
             _logger = LogManager.CreateNullLogger();
         }

         public static void Debug(Exception exception, string format, params object[] args)
         {
             if (!_logger.IsDebugEnabled) return;
             var logEvent = GetLogEvent(_loggerName, LogLevel.Debug, exception, format, args);
             _logger.Log(typeof(LogUtil), logEvent);
         }

         public static void Error(Exception exception, string format, params object[] args)
         {
             if (!_logger.IsErrorEnabled) return;
             var logEvent = GetLogEvent(_loggerName, LogLevel.Error, exception, format, args);
             _logger.Log(typeof(LogUtil), logEvent);
         }

         public static void Fatal(Exception exception, string format, params object[] args)
         {
             if (!_logger.IsFatalEnabled) return;
             var logEvent = GetLogEvent(_loggerName, LogLevel.Fatal, exception, format, args);
             _logger.Log(typeof(LogUtil), logEvent);
         }

         public static void Info(Exception exception, string format, params object[] args)
         {
             if (!_logger.IsInfoEnabled) return;
             var logEvent = GetLogEvent(_loggerName, LogLevel.Info, exception, format, args);
             _logger.Log(typeof(LogUtil), logEvent);
         }

         public static void Trace(Exception exception, string format, params object[] args)
         {
             if (!_logger.IsTraceEnabled) return;
             var logEvent = GetLogEvent(_loggerName, LogLevel.Trace, exception, format, args);
             _logger.Log(typeof(LogUtil), logEvent);
         }

         public static void Warn(Exception exception, string format, params object[] args)
         {
             if (!_logger.IsWarnEnabled) return;
             var logEvent = GetLogEvent(_loggerName, LogLevel.Warn, exception, format, args);
             _logger.Log(typeof(LogUtil), logEvent);
         }

         public static void Debug(Exception exception)
         {
             Debug(exception, string.Empty);
         }

         public static void Error(Exception exception)
         {
             Error(exception, string.Empty);
         }

         public static void Fatal(Exception exception)
         {
             Fatal(exception, string.Empty);
         }

         public static void Info(Exception exception)
         {
             Info(exception, string.Empty);
         }

         public static void Trace(Exception exception)
         {
             Trace(exception, string.Empty);
         }

         public static void Warn(Exception exception)
         {
             Warn(exception, string.Empty);
         }

         private static LogEventInfo GetLogEvent(string loggerName, LogLevel level, Exception exception, string format, object[] args)
         {
             string assemblyProp = string.Empty;
             string classProp = string.Empty;
             string methodProp = string.Empty;
             string messageProp = string.Empty;
             string innerMessageProp = string.Empty;

             var logEvent = new LogEventInfo
                 (level, loggerName, string.Format(format, args));

             if (exception != null)
             {
                 assemblyProp = exception.Source;
                 classProp = exception.TargetSite.DeclaringType.FullName;
                 methodProp = exception.TargetSite.Name;
                 messageProp = exception.Message;

                 if (exception.InnerException != null)
                 {
                     innerMessageProp = exception.InnerException.Message;
                 }
             }

             logEvent.Properties["error-source"] = assemblyProp;
             logEvent.Properties["error-class"] = classProp;
             logEvent.Properties["error-method"] = methodProp;
             logEvent.Properties["error-message"] = messageProp;
             logEvent.Properties["inner-error-message"] = innerMessageProp;

             return logEvent;
         }
    }
}